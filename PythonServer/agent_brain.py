"""
Agent Brain - AI Agent with Tool Calling for University Assistant
Uses LangGraph ReAct agent pattern with Gemini LLM and custom tools.
"""

import os
import json
import re
from pathlib import Path
from typing import List, Dict, Any
from dotenv import load_dotenv

from langchain_google_genai import GoogleGenerativeAIEmbeddings, ChatGoogleGenerativeAI
from langchain_chroma import Chroma
from langchain_core.tools import tool
from langchain_core.messages import HumanMessage, SystemMessage
from langgraph.prebuilt import create_react_agent

# Load environment variables
load_dotenv()


class AgentBrain:
    """
    AI Agent Brain for the University Assistant NPC.
    Uses tool calling pattern for better control over retrieval and responses.
    """
    
    def __init__(self, data_directory: str = "data"):
        """Initialize the Agent Brain."""
        self.data_directory = Path(data_directory)
        self.vectorstore = None
        self.agent = None
        self.kb_articles = []
        self.curriculum = []
        
        # Check for API key
        self.api_key = os.getenv("GOOGLE_API_KEY")
        if not self.api_key:
            raise ValueError("GOOGLE_API_KEY not found in environment variables.")
        
        # Initialize components
        self._load_data()
        self._initialize_vectorstore()
        self._create_agent()
        
        print("AgentBrain initialized successfully!")
    
    def _load_data(self):
        """Load FAQ and curriculum data from JSON files."""
        # Load Knowledge Base
        kb_path = self.data_directory / "university_qa.json"
        if kb_path.exists():
            print(f"Loading Knowledge Base from {kb_path}...")
            with open(kb_path, 'r', encoding='utf-8') as f:
                data = json.load(f)
                self.kb_articles = data.get("articles", [])
            print(f"   Loaded {len(self.kb_articles)} KB articles")
        
        # Load Structured Curriculum
        curriculum_path = self.data_directory / "curriculum_structured.json"
        if curriculum_path.exists():
            print(f"Loading Structured Curriculum from {curriculum_path}...")
            with open(curriculum_path, 'r', encoding='utf-8') as f:
                self.curriculum_data = json.load(f)
                self.courses = self.curriculum_data.get("courses", [])
                self.categories = self.curriculum_data.get("categories", {})
                self.graduation_requirements = self.curriculum_data.get("graduation_requirements", {})
            print(f"   Loaded {len(self.courses)} courses")
        else:
            self.curriculum_data = {}
            self.courses = []
            self.categories = {}
            self.graduation_requirements = {}
    
    
    
    def _initialize_vectorstore(self):
        """Load existing ChromaDB vector store (Read-Only)."""
        persist_directory = "./chroma_db_agent"
        
        print("Loading vector store...")
        embeddings = GoogleGenerativeAIEmbeddings(
            model="models/gemini-embedding-001",
            google_api_key=self.api_key
        )
        
        if os.path.exists(persist_directory) and os.listdir(persist_directory):
            try:
                self.vectorstore = Chroma(
                    persist_directory=persist_directory,
                    embedding_function=embeddings,
                    collection_name="university_faqs"
                )
                print(f"   Vector store loaded successfully")
            except Exception as e:
                print(f"   Error loading vector store: {e}")
                self.vectorstore = None
        else:
            print("   Vector store not found! Run 'python init_data.py' to create it.")
            self.vectorstore = None
    
    def _create_agent(self):
        """Create the LangGraph ReAct agent with tools."""
        print("Creating AI Agent...")
        
        # Initialize the LLM - gemini-2.0-flash (1500 RPD   )
        llm = ChatGoogleGenerativeAI(
            model="gemini-2.0-flash",
            google_api_key=self.api_key,
            temperature=0.3
        )
        
        # Create references for tools closure
        vectorstore = self.vectorstore
        courses_data = self.courses
        categories_data = self.categories
        graduation_req = self.graduation_requirements
        
        # Helper functions for normalization
        def normalize_category(cat: str) -> str:
            """Normalize category: Greek -> Latin (Α->A, Β->B, Ε->E)"""
            cat = cat.upper().strip()
            for gr, en in {'Α': 'A', 'Β': 'B', 'Ε': 'E'}.items():
                cat = cat.replace(gr, en)
            return cat

        def normalize_course_code(code: str) -> str:
            """Normalize course code: Latin -> Greek (H->Η, Y->Υ) and format as ΗΥ-XXX"""
            code = code.upper().strip()
            for lat, gr in {'A': 'Α', 'B': 'Β', 'E': 'Ε', 'H': 'Η', 'I': 'Ι', 
                            'K': 'Κ', 'M': 'Μ', 'N': 'Ν', 'O': 'Ο', 'P': 'Ρ', 
                            'T': 'Τ', 'X': 'Χ', 'Y': 'Υ', 'Z': 'Ζ'}.items():
                code = code.replace(lat, gr)
            match = re.match(r'([A-ZΑ-Ω]+)[\W_]*([0-9]+.*)', code)
            if match:
                return f"{match.group(1)}-{match.group(2)}"
            return code
        
        # ==================== TOOL 1: University Policy ====================
        # Store kb_articles reference for keyword search
        kb_articles = self.kb_articles
        
        @tool
        def search_university_policy(query: str) -> str:
            """
            Αναζήτηση στη βάση γνώσης για διαδικασίες, κανονισμούς, αιτήσεις και παροχές.
            ΧΡΗΣΙΜΟΠΟΙΗΣΕ ΑΥΤΟ για: διακοπή σπουδών, πάσο, εγγραφές, ορκωμοσία, γραμματεία, 
            eservices, συγγράμματα, μετεγγραφές, πτυχιακή, Erasmus, κτλ.
            
            Args:
                query: Η ερώτηση ή το θέμα (π.χ. "διακοπή σπουδών", "πώς βγάζω πάσο")
            
            Returns:
                Σχετικά άρθρα από τη βάση γνώσης με links αν υπάρχουν
            """
            print(f"search_university_policy called with: '{query}'")
            
            if not vectorstore:
                return "Σφάλμα: Η βάση γνώσης δεν είναι διαθέσιμη."
            
            try:
                # Perform semantic search
                results = vectorstore.similarity_search(query, k=4)
                
                if not results:
                    return "Δεν βρέθηκαν σχετικές πληροφορίες στη βάση γνώσης."
                
                print(f"Found {len(results)} matching articles via Embeddings")
                formatted_results = []
                
                for i, doc in enumerate(results):
                    # Extract metadata
                    meta = doc.metadata
                    doc_type = meta.get("type", "unknown")
                    
                    if doc_type == "course":
                        # Format course result
                        code = meta.get("code", "N/A")
                        name = meta.get("name", "Άγνωστο Μάθημα")
                        ects = meta.get("ects", "?")
                        title = f"ΜΑΘΗΜΑ: {code} - {name} ({ects} ECTS)"
                    else:
                        # Format generic/KB result
                        title = meta.get("title", f"Αποτέλεσμα {i+1}")
                    
                    content = doc.page_content
                    
                    # Clean up "Θέμα:" prefix if present in content
                    if content.startswith("Θέμα:"):
                        # Optional: Remove the raw header if we already have the title
                        pass
                        
                    formatted_results.append(f"**{title}**\n{content}")
                    print(f"Match {i}: {title[:50]}...")
                
                return "\n\n---\n\n".join(formatted_results)
                
            except Exception as e:
                print(f"Error during semantic search: {e}")
                return "Παρουσιάστηκε σφάλμα κατά την αναζήτηση."
        
        # ==================== TOOL 2: Course Catalog ====================
        @tool
        def get_course_catalog(filter_type: str, value: str = "") -> str:
            """
            Αναζήτηση στον κατάλογο μαθημάτων με φίλτρα.
            
            Args:
                filter_type: Τύπος φίλτρου - "semester", "category", "all"
                value: Τιμή φίλτρου (π.χ. "3" για εξάμηνο, "B4" για κατηγορία)
            
            Returns:
                Λίστα μαθημάτων με ECTS και προαπαιτούμενα
            
            Παραδείγματα:
            - get_course_catalog("semester", "3") → Μαθήματα 3ου εξαμήνου
            - get_course_catalog("category", "B4") → Μαθήματα Τεχνητής Νοημοσύνης
            - get_course_catalog("all", "") → Όλα τα μαθήματα (περίληψη)
            """
            print(f"get_course_catalog called: filter_type='{filter_type}', value='{value}'")
            
            if filter_type == "semester":
                try:
                    sem = int(value)
                except ValueError:
                    return f"Μη έγκυρος αριθμός εξαμήνου: {value}"
                
                matching = [c for c in courses_data if c.get("semester") == sem]
                if not matching:
                    return f"Δεν βρέθηκαν μαθήματα για το {sem}ο εξάμηνο."
                
                total_ects = sum(c.get("ects", 0) for c in matching)
                result = f"**Μαθήματα {sem}ου Εξαμήνου** (Σύνολο: {total_ects} ECTS):\n\n"
                for c in matching:
                    prereqs = ", ".join(c.get("prerequisites", [])) or "Κανένα"
                    result += f"• **{c['code']}** - {c['name']} ({c.get('ects', '?')} ECTS)\n  Προαπαιτούμενα: {prereqs}\n\n"
                return result
            
            elif filter_type == "category":
                category = normalize_category(value)
                print(f"   Normalized category: {category}")
                
                matching = [c for c in courses_data if c.get("category", "").upper() == category]
                print(f"   Found {len(matching)} courses")
                
                if not matching:
                    return f"Δεν βρέθηκαν μαθήματα στην κατηγορία {category}."
                
                cat_info = categories_data.get(category, {})
                cat_name = cat_info.get("name", category)
                
                result = f"**Μαθήματα {category} ({cat_name}):**\n\n"
                for c in matching:
                    prereqs = ", ".join(c.get("prerequisites", [])) or "Κανένα"
                    result += f"• {c['code']} - {c['name']} ({c.get('ects', '?')} ECTS)\n  Προαπαιτούμενα: {prereqs}\n\n"
                return result
            
            elif filter_type == "all":
                # Summary of all categories
                result = "**Κατάλογος Μαθημάτων (Περίληψη):**\n\n"
                by_cat = {}
                for c in courses_data:
                    cat = c.get("category", "Άλλο")
                    by_cat[cat] = by_cat.get(cat, 0) + 1
                
                for cat, count in sorted(by_cat.items()):
                    cat_name = categories_data.get(cat, {}).get("name", cat)
                    result += f"• **{cat}** ({cat_name}): {count} μαθήματα\n"
                
                result += f"\n**Σύνολο:** {len(courses_data)} μαθήματα"
                result += "\n\n*Χρησιμοποίησε filter_type='category' με συγκεκριμένη κατηγορία για λεπτομέρειες.*"
                return result
            
            else:
                return f"Μη έγκυρος τύπος φίλτρου: {filter_type}. Χρησιμοποίησε 'semester', 'category' ή 'all'."
        
        # ==================== TOOL 3: Course Logic & Graduation ====================
        @tool
        def get_course_and_graduation_logic(course_code: str = "") -> str:
            """
            Επιστρέφει λογική μαθημάτων (ECTS, προαπαιτούμενα, εξαρτήσεις) ή απαιτήσεις πτυχίου.
            
            Args:
                course_code: Κωδικός μαθήματος (π.χ. "ΗΥ-340"). Αν είναι κενό, επιστρέφει τις απαιτήσεις πτυχίου.
            
            Returns:
                Αν δοθεί course_code: Πλήρεις πληροφορίες μαθήματος + ποια μαθήματα το απαιτούν
                Αν δεν δοθεί: Απαιτήσεις πτυχίου (ECTS, κατηγορίες, κανόνες)
            
            Παραδείγματα:
            - get_course_and_graduation_logic("ΗΥ-340") → Πληροφορίες για Δίκτυα
            - get_course_and_graduation_logic() → Απαιτήσεις πτυχίου
            """
            print(f"get_course_and_graduation_logic called: course_code='{course_code}'")
            
            # If no course code, return graduation requirements
            if not course_code or course_code.strip() == "":
                req = graduation_req
                if not req:
                    return "Δεν βρέθηκαν πληροφορίες για τις απαιτήσεις πτυχίου."
                
                return f"""**Απαιτήσεις για Λήψη Πτυχίου:**

1. **Συνολικά ECTS:** {req.get('total_ects', 240)} τουλάχιστον
2. **Ελάχιστα εξάμηνα:** {req.get('min_semesters', 8)}
3. **Μαθήματα Κορμού:** Όλα υποχρεωτικά (συμπ. Διπλωματική)
4. **Μαθήματα Επιλογής Ειδίκευσης (A1-C4):** 
   - Τουλάχιστον {req.get('specialization_ects', {}).get('minimum', 42)} ECTS
   - Τουλάχιστον 1 μάθημα από κάθε γενική κατηγορία (Α, Β, C)
   - Το πολύ 4 μαθήματα από την ίδια γενική κατηγορία
5. **Μαθήματα E1-E2:**
   - Τουλάχιστον 3 μαθήματα / 20 ECTS
   - Τουλάχιστον 2 από E1
   - Το πολύ 1 από E2
6. **Ελεύθερη Επιλογή:** 6 ECTS

**Διπλωματική Εργασία:** 18 ECTS, ξεκινά από 5ο εξάμηνο

**Χρήσιμα Links:**
- Αιτήσεις: https://eservices.uoc.gr/
- Φοιτητολόγιο: https://eduportal.cict.uoc.gr/
- Τμήμα: https://www.csd.uoc.gr/"""
            
            # Find the specific course by code OR name
            normalized_code = normalize_course_code(course_code)
            query_lower = course_code.lower().strip()
            print(f"   Normalized code: {normalized_code}")
            
            course = None
            # First try exact code match
            for c in courses_data:
                if normalize_course_code(c.get("code", "")) == normalized_code:
                    course = c
                    break
            
            # If not found, try name match
            if not course:
                print(f"   Code not found, searching by name: '{query_lower}'")
                for c in courses_data:
                    if query_lower in c.get("name", "").lower():
                        course = c
                        print(f"   Found by name: {c['code']} - {c['name']}")
                        break
            
            if not course:
                return f"Δεν βρέθηκε μάθημα με κωδικό ή όνομα '{course_code}'."
            
            # Get course info
            cat = course.get("category", "")
            cat_info = categories_data.get(cat, {})
            prereqs = course.get("prerequisites", [])
            prereq_str = ", ".join(prereqs) if prereqs else "Κανένα"
            prereq_note = course.get("prerequisite_note", "")
            
            result = f"""**{course['code']} - {course['name']}**

• **ECTS:** {course.get('ects', 'N/A')}
• **Κατηγορία:** {cat} - {cat_info.get('name', '')}
• **Εξάμηνο:** {course.get('semester', 'Επιλογής') or 'Επιλογής'}
• **Προαπαιτούμενα:** {prereq_str}"""
            
            if prereq_note:
                result += f"\n• **Σημείωση:** {prereq_note}"
            
            # Find dependent courses (what courses require this one)
            dependents = []
            for c in courses_data:
                c_prereqs = [normalize_course_code(p) for p in c.get("prerequisites", [])]
                if normalized_code in c_prereqs:
                    dependents.append(c)
            
            if dependents:
                result += f"\n\n**Μαθήματα που ΑΠΑΙΤΟΥΝ το {course['code']}:**\n"
                for dep in dependents[:10]:  # Limit to 10
                    result += f"• {dep['code']} - {dep['name']}\n"
                if len(dependents) > 10:
                    result += f"... και {len(dependents) - 10} ακόμα\n"
                result += f"\n**Σύνολο:** {len(dependents)} μαθήματα εξαρτώνται από αυτό."
            else:
                result += f"\n\n*Κανένα μάθημα δεν έχει το {course['code']} ως προαπαιτούμενο.*"
            
            return result
        
        # ==================== SYSTEM PROMPT ====================
        system_prompt = """Είσαι ο Βοηθός του Τμήματος Επιστήμης Υπολογιστών του Πανεπιστημίου Κρήτης (CSD).

ΕΧΕΙΣ 3 ΕΡΓΑΛΕΙΑ:

1. **search_university_policy(query)** → Για διαδικασίες, αιτήσεις, κανονισμούς
   Παραδείγματα: "διακοπή σπουδών", "πάσο", "εγγραφή", "ορκωμοσία", "eservices"

2. **get_course_catalog(filter_type, value)** → Για λίστες μαθημάτων
   - filter_type="semester", value="3" → Μαθήματα 3ου εξαμήνου
   - filter_type="category", value="ΚΟΡΜΟΣ" → Υποχρεωτικά μαθήματα κορμού
   - filter_type="category", value="B4" → Μαθήματα κατηγορίας B4
   - filter_type="all", value="" → Περίληψη όλων

3. **get_course_and_graduation_logic(course_code)** → Για λογική μαθημάτων/πτυχίου
   - course_code="ΗΥ-340" → Πληροφορίες + προαπαιτούμενα + εξαρτήσεις
   - course_code="" (κενό) → Απαιτήσεις πτυχίου

ΚΑΝΟΝΕΣ:
- Ελληνικά πάντα (Αγγλικά μόνο αν ζητηθεί)
- ΓΡΑΨΕ τα δεδομένα στην απάντηση - ο χρήστης ΔΕΝ βλέπει τα εργαλεία!
- ΠΟΤΕ μην επινοείς πληροφορίες - αν δεν βρεθούν, πες το
- Χρησιμοποίησε ΑΚΡΙΒΩΣ τα αποτελέσματα του εργαλείου
- Να είσαι ΣΥΝΤΟΜΟΣ και ΞΕΚΑΘΑΡΟΣ"""
        
        # Create the agent with 3 Tools
        tools = [
            search_university_policy,
            get_course_catalog,
            get_course_and_graduation_logic
        ]
        
        self.agent = create_react_agent(
            model=llm,
            tools=tools,
            prompt=system_prompt
        )
        
        print(f"   Agent created with {len(tools)} Tools")
    
    async def ask(self, question: str) -> str:
        """
        Ask the agent a question with auto-retry for rate limits.
        
        Args:
            question: The user's question
            
        Returns:
            The agent's response
        """
        import asyncio
        
        max_retries = 3
        retry_delays = [30, 60, 90]
        
        print(f"\n📝 Received Question: {question}")
        
        for attempt in range(max_retries):
            try:
                # Invoke the agent
                result = await self.agent.ainvoke({
                    "messages": [HumanMessage(content=question)]
                })
                
                # Extract the final response
                final_response = "Λυπάμαι, δεν μπόρεσα να επεξεργαστώ την ερώτησή σου."
                messages = result.get("messages", [])
                
                if messages:
                    # Get the last AI message
                    for msg in reversed(messages):
                        if hasattr(msg, 'content') and msg.content:
                            # Skip tool calls
                            if not hasattr(msg, 'tool_calls') or not msg.tool_calls:
                                final_response = msg.content
                                break
                            # If it has content and tool_calls, still return the content
                            if msg.content:
                                final_response = msg.content
                                break
                
                print(f"✅ Final Response: {final_response}\n" + "-"*50)
                return final_response
                
            except Exception as e:
                error_str = str(e)
                if "429" in error_str or "RESOURCE_EXHAUSTED" in error_str:
                    if attempt < max_retries - 1:
                        delay = retry_delays[attempt]
                        print(f"Rate limit hit, waiting {delay}s before retry {attempt + 1}/{max_retries}...")
                        await asyncio.sleep(delay)
                        continue
                    else:
                        print(f"Rate limit exceeded after {max_retries} retries")
                        return "Λυπάμαι, η υπηρεσία είναι προσωρινά υπερφορτωμένη. Παρακαλώ δοκίμασε ξανά σε λίγο."
                else:
                    print(f"Error in agent: {e}")
                    return "Παρουσιάστηκε σφάλμα κατά την επεξεργασία της ερώτησής σου."
        
        return "Παρουσιάστηκε απροσδόκητο σφάλμα."
    
    def ask_sync(self, question: str) -> str:
        """Synchronous version for testing."""
        import asyncio
        return asyncio.run(self.ask(question))
    
    def debug_retrieval(self, query: str) -> List[Dict]:
        """Debug method to see retrieved documents."""
        if self.vectorstore is None:
            return [{"error": "Vectorstore not initialized"}]
        
        docs = self.vectorstore.similarity_search(query, k=5)
        results = []
        for i, doc in enumerate(docs):
            results.append({
                "rank": i + 1,
                "content": doc.page_content[:300],
                "metadata": doc.metadata
            })
        return results


# For testing
if __name__ == "__main__":
    print("=" * 50)
    print("Testing Agent Brain")
    print("=" * 50)
    
    brain = AgentBrain()
    
    test_questions = [
        "Πού βρίσκεται το Τμήμα Επιστήμης Υπολογιστών;",
        "Ποιες είναι οι ώρες λειτουργίας της γραμματείας;",
        "Πώς μπορώ να βγάλω βεβαίωση σπουδών;"
    ]
    
    for q in test_questions:
        print(f"\n❓ Ερώτηση: {q}")
        answer = brain.ask_sync(q)
        print(f"Απάντηση: {answer}")
