#!/usr/bin/env python3
"""
PDF to RAG JSON Extractor

Extracts structured university curriculum data from PDF documents
using smart chunking and Gemini AI for intelligent processing.
"""

import argparse
import json
import os
import re
import sys
from pathlib import Path
from typing import Optional

import pdfplumber
import google.generativeai as genai
from dotenv import load_dotenv


# Load environment variables
load_dotenv()


# ============================================================================
# Configuration
# ============================================================================

DEFAULT_CHUNK_SIZE = 8000  # Characters per chunk
DEFAULT_MODEL = "gemini-2.0-flash"

# Schema template for extraction
OUTPUT_SCHEMA = {
    "graduation_requirements": {
        "total_ects": "number",
        "min_semesters": "number",
        "specialization_ects": {},
        "e1_e2_requirements": {},
        "free_elective_ects": "number"
    },
    "categories": {},
    "courses": [],
    "policies": [],
    "thesis": {}
}


# ============================================================================
# PDF Extraction
# ============================================================================

def extract_text_from_pdf(pdf_path: str, verbose: bool = False) -> list[dict]:
    """
    Extract text from PDF, preserving page structure.
    Returns a list of {page_num, text, tables} dictionaries.
    """
    pages = []
    
    with pdfplumber.open(pdf_path) as pdf:
        total_pages = len(pdf.pages)
        
        for i, page in enumerate(pdf.pages):
            page_num = i + 1
            
            if verbose:
                print(f"  Extracting page {page_num}/{total_pages}...", end="\r")
            
            # Extract text
            text = page.extract_text() or ""
            
            # Extract tables
            tables = page.extract_tables() or []
            
            pages.append({
                "page_num": page_num,
                "text": text,
                "tables": tables
            })
    
    if verbose:
        print(f"  Extracted {total_pages} pages.              ")
    
    return pages


# ============================================================================
# Smart Chunking
# ============================================================================

def detect_section_headers(text: str) -> list[tuple[int, str]]:
    """
    Detect section headers in Greek text.
    Returns list of (position, header_text) tuples.
    """
    patterns = [
        # Greek numbered sections: "1.", "1.1", "1.1.1", etc.
        r'^(\d+\.(?:\d+\.?)*)\s*(.+)$',
        # Greek letter sections: "Α.", "Β.", etc.
        r'^([ΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ]\.)\s*(.+)$',
        # Capitalized section titles (all caps Greek)
        r'^([Α-Ω\s]{10,})$',
    ]
    
    headers = []
    for i, line in enumerate(text.split('\n')):
        line = line.strip()
        for pattern in patterns:
            match = re.match(pattern, line, re.MULTILINE)
            if match:
                headers.append((i, line))
                break
    
    return headers


def create_smart_chunks(pages: list[dict], chunk_size: int = DEFAULT_CHUNK_SIZE, verbose: bool = False) -> list[dict]:
    """
    Create smart chunks that respect section boundaries.
    Each chunk includes metadata about its source.
    """
    chunks = []
    current_chunk = ""
    current_pages = []
    current_section = "Introduction"
    
    for page in pages:
        page_num = page["page_num"]
        text = page["text"]
        
        # Detect section headers in this page
        headers = detect_section_headers(text)
        if headers:
            current_section = headers[0][1][:100]  # First header, truncated
        
        # Check if adding this page would exceed chunk size
        if len(current_chunk) + len(text) > chunk_size and current_chunk:
            # Save current chunk
            chunks.append({
                "chunk_id": len(chunks) + 1,
                "pages": current_pages.copy(),
                "section_hint": current_section,
                "text": current_chunk,
                "char_count": len(current_chunk)
            })
            current_chunk = ""
            current_pages = []
        
        # Add page to current chunk
        current_chunk += f"\n\n--- Page {page_num} ---\n\n{text}"
        current_pages.append(page_num)
        
        # Also add table content if present
        for table in page.get("tables", []):
            if table:
                table_text = "\n".join([" | ".join([str(cell) if cell else "" for cell in row]) for row in table])
                current_chunk += f"\n\n[TABLE]\n{table_text}\n[/TABLE]\n"
    
    # Don't forget the last chunk
    if current_chunk:
        chunks.append({
            "chunk_id": len(chunks) + 1,
            "pages": current_pages,
            "section_hint": current_section,
            "text": current_chunk,
            "char_count": len(current_chunk)
        })
    
    if verbose:
        print(f"  Created {len(chunks)} chunks from {len(pages)} pages.")
        for chunk in chunks:
            print(f"    Chunk {chunk['chunk_id']}: pages {chunk['pages']}, {chunk['char_count']} chars")
    
    return chunks


# ============================================================================
# Gemini LLM Processing
# ============================================================================

def init_gemini(api_key: str, model_name: str = DEFAULT_MODEL):
    """Initialize Gemini API."""
    genai.configure(api_key=api_key)
    return genai.GenerativeModel(model_name)


def extract_from_chunk(model, chunk: dict, verbose: bool = False) -> dict:
    """
    Use Gemini to extract structured data from a chunk.
    """
    prompt = f"""You are extracting structured data from a Greek university curriculum document (Computer Science department).

Analyze this text chunk and extract ANY relevant information into JSON format. Look for:

1. **Courses**: Extract course code (e.g., "ΗΥ-100"), name, ECTS credits, semester, category, prerequisites
2. **Graduation Requirements**: Total ECTS needed, required courses, specialization rules
3. **Categories**: Course category definitions (ΚΟΡΜΟΣ, E1, E2, A1-A4, B1-B4, C1-C4)
4. **Policies**: University rules, deadlines, procedures
5. **Thesis Info**: Requirements, process, committee structure

TEXT CHUNK (Pages {chunk['pages']}):
---
{chunk['text']}
---

Return a JSON object with these possible keys (only include keys where you found data):
{{
    "courses": [
        {{
            "code": "string",
            "name": "string", 
            "ects": number,
            "category": "string",
            "semester": number or null,
            "prerequisites": ["code1", "code2"],
            "prerequisite_note": "string or null"
        }}
    ],
    "graduation_requirements": {{...}},
    "categories": {{"CATEGORY_CODE": {{"name": "...", "description": "..."}}}},
    "policies": ["policy statement 1", "policy statement 2"],
    "thesis": {{...}},
    "other_info": ["any other useful information"]
}}

IMPORTANT:
- Return ONLY valid JSON, no markdown formatting
- Use Greek text exactly as it appears
- If a field is unknown, omit it or use null
- For prerequisites, parse "ΗΥ-100 ή ΗΥ-150" as ["ΗΥ-100", "ΗΥ-150"] and add note explaining the OR relationship
"""

    try:
        response = model.generate_content(prompt)
        response_text = response.text.strip()
        
        # Clean up response - remove markdown code blocks if present
        if response_text.startswith("```"):
            response_text = re.sub(r'^```(?:json)?\n?', '', response_text)
            response_text = re.sub(r'\n?```$', '', response_text)
        
        # Parse JSON
        extracted = json.loads(response_text)
        
        if verbose:
            courses_found = len(extracted.get("courses", []))
            policies_found = len(extracted.get("policies", []))
            print(f"    Chunk {chunk['chunk_id']}: {courses_found} courses, {policies_found} policies")
        
        return extracted
    
    except json.JSONDecodeError as e:
        if verbose:
            print(f"    Chunk {chunk['chunk_id']}: JSON parse error - {e}")
        return {"parse_error": str(e), "raw_response": response_text[:500]}
    
    except Exception as e:
        if verbose:
            print(f"    Chunk {chunk['chunk_id']}: Error - {e}")
        return {"error": str(e)}


# ============================================================================
# JSON Merging & Reorganization
# ============================================================================

def get_course_completeness_score(course: dict) -> int:
    """
    Calculate a completeness score for a course entry.
    Higher score = more complete information.
    """
    score = 0
    if course.get("name"):
        score += 10  # Name is most important
    if course.get("ects"):
        score += 5
    if course.get("prerequisites"):
        score += 3
    if course.get("category"):
        score += 2
    if course.get("semester"):
        score += 2
    if course.get("prerequisite_note"):
        score += 1
    # Penalize entries that ONLY have code + page
    if len(course) <= 2 and "page" in course:
        score = 0
    return score


def normalize_course_code(code: str) -> str:
    """
    Normalize course codes to handle variations like HY-460 vs ΗΥ-460.
    Converts Latin letters to Greek equivalents.
    """
    # Map Latin to Greek letters commonly used in course codes
    latin_to_greek = {
        'H': 'Η', 'Y': 'Υ', 'A': 'Α', 'B': 'Β', 'E': 'Ε', 'K': 'Κ',
        'M': 'Μ', 'N': 'Ν', 'O': 'Ο', 'P': 'Ρ', 'T': 'Τ', 'X': 'Χ',
        'Z': 'Ζ', 'I': 'Ι'
    }
    normalized = code.upper()
    for latin, greek in latin_to_greek.items():
        normalized = normalized.replace(latin, greek)
    return normalized


def merge_course_entries(existing: dict, new: dict) -> dict:
    """
    Merge two course entries, keeping the most complete information.
    """
    merged = existing.copy()
    
    # For each field, use the new value if existing is empty/None
    for key, value in new.items():
        if key == "code":
            continue  # Don't change the code
        existing_value = merged.get(key)
        if value and not existing_value:
            merged[key] = value
        elif key == "prerequisites" and value and existing_value:
            # Merge prerequisites lists
            existing_prereqs = set(existing_value) if isinstance(existing_value, list) else set()
            new_prereqs = set(value) if isinstance(value, list) else set()
            merged[key] = list(existing_prereqs | new_prereqs)
    
    return merged


def merge_extractions(extractions: list[dict], verbose: bool = False) -> dict:
    """
    Merge all chunk extractions into a single coherent structure.
    Handles deduplication and conflict resolution.
    Prefers more complete course entries over partial ones.
    """
    merged = {
        "graduation_requirements": {},
        "categories": {},
        "courses": [],
        "policies": [],
        "thesis": {},
        "other_info": []
    }
    
    # Use dict to track courses by normalized code
    courses_by_code = {}
    seen_policies = set()
    
    for extraction in extractions:
        if "error" in extraction or "parse_error" in extraction:
            continue
        
        # Merge courses (smart deduplication)
        for course in extraction.get("courses", []):
            code = course.get("code", "")
            if not code:
                continue
                
            normalized_code = normalize_course_code(code)
            new_score = get_course_completeness_score(course)
            
            if normalized_code in courses_by_code:
                existing_course, existing_score = courses_by_code[normalized_code]
                
                if new_score > existing_score:
                    # New entry is more complete, merge into it
                    merged_course = merge_course_entries(course, existing_course)
                    courses_by_code[normalized_code] = (merged_course, new_score)
                elif new_score > 0:
                    # Existing is better, but merge any new info
                    merged_course = merge_course_entries(existing_course, course)
                    courses_by_code[normalized_code] = (merged_course, existing_score)
                # If new_score == 0, it's just a page reference, ignore it
            else:
                courses_by_code[normalized_code] = (course, new_score)
        
        # Merge categories
        for cat_code, cat_info in extraction.get("categories", {}).items():
            if cat_code not in merged["categories"]:
                merged["categories"][cat_code] = cat_info
        
        # Merge policies (deduplicate by exact text)
        for policy in extraction.get("policies", []):
            policy_key = policy[:100] if isinstance(policy, str) else str(policy)[:100]
            if policy_key not in seen_policies:
                merged["policies"].append(policy)
                seen_policies.add(policy_key)
        
        # Merge graduation requirements (later extractions override earlier)
        if extraction.get("graduation_requirements"):
            merged["graduation_requirements"].update(extraction["graduation_requirements"])
        
        # Merge thesis info
        if extraction.get("thesis"):
            merged["thesis"].update(extraction["thesis"])
        
        # Merge other info
        for info in extraction.get("other_info", []):
            if info not in merged["other_info"]:
                merged["other_info"].append(info)
    
    # Convert courses dict back to list, filtering out incomplete entries
    for normalized_code, (course, score) in courses_by_code.items():
        # Only include courses with at least a name (score >= 10)
        if score >= 10:
            # Remove 'page' field if present (not needed in final output)
            course_clean = {k: v for k, v in course.items() if k != "page"}
            merged["courses"].append(course_clean)
        elif verbose:
            print(f"    Skipping incomplete course: {course.get('code')} (score: {score})")
    
    # Sort courses by code
    merged["courses"].sort(key=lambda c: c.get("code", ""))
    
    if verbose:
        print(f"\n  Merge complete:")
        print(f"    - {len(merged['courses'])} unique courses")
        print(f"    - {len(merged['categories'])} categories")
        print(f"    - {len(merged['policies'])} policies")
        print(f"    - {len(merged['other_info'])} other items")
    
    return merged


def validate_output(data: dict, verbose: bool = False) -> list[str]:
    """
    Validate the extracted data for common issues.
    Returns list of warnings.
    """
    warnings = []
    
    # Check courses
    for course in data.get("courses", []):
        code = course.get("code", "")
        
        # Validate code format
        if not re.match(r'^[Α-Ω]{2,}-?\d+', code) and not re.match(r'^[A-Z]{2,}-?\d+', code):
            if code:  # Only warn if code exists but has wrong format
                warnings.append(f"Unusual course code format: {code}")
        
        # Check ECTS
        ects = course.get("ects")
        if ects is not None and (not isinstance(ects, (int, float)) or ects <= 0):
            warnings.append(f"Invalid ECTS for {code}: {ects}")
    
    # Check for minimum expected data
    if len(data.get("courses", [])) < 10:
        warnings.append(f"Only {len(data.get('courses', []))} courses extracted - seems low")
    
    if not data.get("graduation_requirements"):
        warnings.append("No graduation requirements extracted")
    
    if verbose and warnings:
        print("\n  Validation warnings:")
        for w in warnings:
            print(f"    ⚠️  {w}")
    
    return warnings


# ============================================================================
# Main
# ============================================================================

def main():
    parser = argparse.ArgumentParser(
        description="Extract structured data from university curriculum PDFs"
    )
    parser.add_argument("--input", "-i", required=True, help="Path to PDF file")
    parser.add_argument("--output", "-o", default="extracted_data.json", help="Output JSON path")
    parser.add_argument("--chunk-size", type=int, default=DEFAULT_CHUNK_SIZE, help="Max chars per chunk")
    parser.add_argument("--model", default=DEFAULT_MODEL, help="Gemini model to use")
    parser.add_argument("--verbose", "-v", action="store_true", help="Show detailed progress")
    
    args = parser.parse_args()
    
    # Validate input
    if not os.path.exists(args.input):
        print(f"Error: Input file not found: {args.input}")
        sys.exit(1)
    
    # Get API key
    api_key = os.getenv("GEMINI_API_KEY")
    if not api_key:
        print("Error: GEMINI_API_KEY not found in environment. Create a .env file with your key.")
        sys.exit(1)
    
    print(f"\n PDF to RAG JSON Extractor")
    print(f"{'='*50}")
    print(f"Input:  {args.input}")
    print(f"Output: {args.output}")
    print(f"Model:  {args.model}")
    print(f"{'='*50}\n")
    
    # Step 1: Extract text from PDF
    print(" Step 1: Extracting text from PDF...")
    pages = extract_text_from_pdf(args.input, verbose=args.verbose)
    
    # Step 2: Create smart chunks
    print("\n Step 2: Creating smart chunks...")
    chunks = create_smart_chunks(pages, chunk_size=args.chunk_size, verbose=args.verbose)
    
    # Step 3: Process chunks with Gemini
    print(f"\n Step 3: Processing {len(chunks)} chunks with Gemini...")
    model = init_gemini(api_key, args.model)
    
    extractions = []
    for i, chunk in enumerate(chunks):
        print(f"  Processing chunk {i+1}/{len(chunks)}...", end="" if not args.verbose else "\n")
        extraction = extract_from_chunk(model, chunk, verbose=args.verbose)
        extractions.append(extraction)
        if not args.verbose:
            print(" ✓")
    
    # Step 4: Merge and reorganize
    print("\n Step 4: Merging extractions...")
    merged = merge_extractions(extractions, verbose=args.verbose)
    
    # Step 5: Validate
    print("\n Step 5: Validating output...")
    warnings = validate_output(merged, verbose=args.verbose)
    
    # Step 6: Save output
    print(f"\n Step 6: Saving to {args.output}...")
    with open(args.output, 'w', encoding='utf-8') as f:
        json.dump(merged, f, ensure_ascii=False, indent=4)
    
    print(f"\n{'='*50}")
    print(f" Done! Extracted data saved to: {args.output}")
    print(f"   - {len(merged['courses'])} courses")
    print(f"   - {len(merged['categories'])} categories")
    print(f"   - {len(merged['policies'])} policies")
    if warnings:
        print(f"   - {len(warnings)} warnings (see above)")
    print(f"{'='*50}\n")


if __name__ == "__main__":
    main()
