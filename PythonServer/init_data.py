
import os
import json
import time
from pathlib import Path
from dotenv import load_dotenv
from langchain_google_genai import GoogleGenerativeAIEmbeddings
from langchain_chroma import Chroma
import shutil

# Load environment variables
load_dotenv()

def ingest_data(data_directory: str = "data"):
    """
    Ingest data from JSON files into ChromaDB.
    """
    data_path = Path(data_directory)
    persist_directory = "./chroma_db_agent"
    
    # Check for API key
    api_key = os.getenv("GOOGLE_API_KEY")
    if not api_key:
        print("GOOGLE_API_KEY not found in environment variables.")
        return

    print(f"Starting data ingestion from {data_path}...")

    # Load Knowledge Base
    kb_path = data_path / "university_qa.json"
    kb_articles = []
    if kb_path.exists():
        print(f"Loading Knowledge Base from {kb_path}...")
        with open(kb_path, 'r', encoding='utf-8') as f:
            data = json.load(f)
            kb_articles = data.get("articles", [])
        print(f"   Loaded {len(kb_articles)} KB articles")
    
    # Load Structured Curriculum
    curriculum_path = data_path / "curriculum_structured.json"
    courses = []
    categories = {}
    if curriculum_path.exists():
        print(f"Loading Structured Curriculum from {curriculum_path}...")
        with open(curriculum_path, 'r', encoding='utf-8') as f:
            curriculum_data = json.load(f)
            courses = curriculum_data.get("courses", [])
            categories = curriculum_data.get("categories", {})
        print(f"   Loaded {len(courses)} courses")

    # Initialize Embeddings
    print("Initializing embeddings model...")
    embeddings = GoogleGenerativeAIEmbeddings(
        model="models/gemini-embedding-001",
        google_api_key=api_key
    )

    # Clean existing DB to force rebuild (Safe since this is an explicit ingest script)
    if os.path.exists(persist_directory):
        print(f"Removing existing database at {persist_directory}...")
        try:
            shutil.rmtree(persist_directory)
            # Wait a bit for file system to release locks
            time.sleep(2)
        except Exception as e:
            print(f"Could not delete directory (might be in use): {e}")
            # If we can't delete, we might append duplicates. 
            # Ideally we want a clean slate.

    print("Creating new vector store...")
    vectorstore = Chroma(
        embedding_function=embeddings,
        persist_directory=persist_directory,
        collection_name="university_faqs"
    )

    # ========== BATCH 1: Knowledge Base Articles ==========
    print(f"   Preparing {len(kb_articles)} articles for batch embedding...")
    kb_texts = []
    kb_metadatas = []
    kb_ids = []
    
    for article in kb_articles:
        text = f"Θέμα: {article['title']}\nΠληροφορίες: {article['content']}"
        kb_texts.append(text)
        kb_metadatas.append({
            "id": article["id"],
            "type": "kb_article",
            "category": article.get("category", "general"),
            "title": article["title"][:200]
        })
        kb_ids.append(f"kb_{article['id']}")
    
    if kb_texts:
        try:
            print(f"   Sending KB batch ({len(kb_texts)} texts)...")
            vectorstore.add_texts(
                texts=kb_texts,
                metadatas=kb_metadatas,
                ids=kb_ids
            )
            print(f"   KB batch embedded successfully!")
        except Exception as e:
            print(f"   Error embedding KB batch: {e}")
            if "429" in str(e) or "RESOURCE_EXHAUSTED" in str(e):
                print("   Rate limit hit, waiting 60s and retrying...")
                time.sleep(60)
                vectorstore.add_texts(
                    texts=kb_texts,
                    metadatas=kb_metadatas,
                    ids=kb_ids
                )

    # Small delay before next batch
    time.sleep(5)
    
    # ========== BATCH 2: Structured Courses ==========
    print(f"   Preparing {len(courses)} courses for batch embedding...")
    course_texts = []
    course_metadatas = []
    course_ids = []
    
    for course in courses:
        # Create rich text for each course
        prereqs_str = ", ".join(course.get("prerequisites", [])) if course.get("prerequisites") else "Κανένα"
        category_name = categories.get(course.get("category", ""), {}).get("name", course.get("category", ""))
        
        text = f"""Μάθημα: {course['code']} - {course['name']}
ECTS: {course.get('ects', 'N/A')}
Κατηγορία: {course.get('category', 'N/A')} - {category_name}
Εξάμηνο: {course.get('semester', 'Επιλογής')}
Προαπαιτούμενα: {prereqs_str}
{f"Σημείωση: {course.get('prerequisite_note')}" if course.get('prerequisite_note') else ""}"""
        
        course_texts.append(text)
        course_metadatas.append({
            "code": course["code"],
            "name": course["name"],
            "type": "course",
            "category": course.get("category", ""),
            "ects": course.get("ects", 0)
        })
        course_ids.append(f"course_{course['code']}")
    
    if course_texts:
        try:
            print(f"   Sending courses batch ({len(course_texts)} courses)...")
            vectorstore.add_texts(
                texts=course_texts,
                metadatas=course_metadatas,
                ids=course_ids
            )
            print(f"   Courses batch embedded successfully!")
        except Exception as e:
            print(f"   Error embedding courses batch: {e}")
            if "429" in str(e) or "RESOURCE_EXHAUSTED" in str(e):
                print("   Rate limit hit, waiting 60s and retrying...")
                time.sleep(60)
                vectorstore.add_texts(
                    texts=course_texts,
                    metadatas=course_metadatas,
                    ids=course_ids
                )
    
    print(f"Ingestion complete! Vector store created with {vectorstore._collection.count()} documents")

if __name__ == "__main__":
    ingest_data()
