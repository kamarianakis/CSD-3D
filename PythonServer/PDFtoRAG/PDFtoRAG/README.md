# PDF to RAG JSON Extractor

A Python tool that extracts structured university curriculum data from PDF documents using smart chunking and Gemini AI.

## Setup

1. Install dependencies:
   ```bash
   pip install -r requirements.txt
   ```

2. Create a `.env` file with your Gemini API key:
   ```
   GEMINI_API_KEY=your_api_key_here
   ```

## Usage

```bash
python pdf_extractor.py --input "path/to/your/file.pdf"
or
python pdf_extractor.py --input "your_document.pdf" --output "output.json"
```

### Options

| Option | Description | Default |
|--------|-------------|---------|
| `--input` | Path to PDF file | Required |
| `--output` | Output JSON path | `extracted_data.json` |
| `--chunk-size` | Max characters per chunk | 8000 |
| `--verbose` | Show detailed progress | False |

## Output

Generates a structured JSON with:
- `graduation_requirements`: ECTS, course constraints, etc.
- `categories`: Course category definitions
- `courses`: All courses with code, name, ECTS, prerequisites
- `policies`: University policies and rules
- `thesis`: Thesis requirements and process
