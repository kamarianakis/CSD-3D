# 🎓 University Assistant - Python Backend Server

This is the Python backend server for the University Assistant NPC in the Unity 3D campus project.
It provides RAG (Retrieval-Augmented Generation) powered answers using LangChain, ChromaDB, and Google Gemini.

##  Architecture

```
Unity WebGL (Client) → POST /api/chat → Python FastAPI (Server) → LangChain/ChromaDB (Context) → Google Gemini (LLM) → Response
```

##  Prerequisites

- Python 3.10 or higher
- Google Gemini API Key (get one at https://makersuite.google.com/app/apikey)

##  Quick Start

### 1. Create Virtual Environment

```bash
# Windows
python -m venv venv
venv\Scripts\activate

# macOS/Linux
python3 -m venv venv
source venv/bin/activate
```

### 2. Install Dependencies

```bash
pip install -r requirements.txt
```

### 3. Configure API Key

Edit the `.env` file and add your Google Gemini API key:

```
GOOGLE_API_KEY=your_actual_api_key_here
```



### 4. Add University Data

Place your university information in the `data/` folder as JSON files.
See the [Adding New Files to the RAG](#-adding-new-files-to-the-rag) section for the expected format.

### 5. Generate Embeddings

Before starting the server for the first time (or after updating your data files), you must manually generate the vector embeddings:

```bash
python init_data.py
```

> ⚠️ **Important:** The server does **not** generate embeddings automatically on startup. You must run `init_data.py` manually whenever your data changes.

### 6. Run the Server

```bash
python main.py
```

Or using uvicorn directly:

```bash
uvicorn main:app --reload --host 0.0.0.0 --port 8000
```

The server will start at: http://localhost:8000

## 📡 API Endpoints

### `GET /` - Root
Returns server status and links to docs.

### `GET /health` - Health Check
Returns server health status and whether the AI brain is initialized.

### `POST /api/chat` - Chat Endpoint
Main endpoint for Unity NPC communication.

**Request:**
```json
{
    "text": "Where is the Computer Science department located?"
}
```

**Response:**
```json
{
    "response": "The Computer Science Department is located at the Voutes campus in Heraklion, Crete, Greece."
}
```

## 📚 API Documentation

Once the server is running, visit:
- **Swagger UI:** http://localhost:8000/docs
- **ReDoc:** http://localhost:8000/redoc

## 📁 Project Structure

```
PythonServer/
├── main.py              # FastAPI server with endpoints
├── agent_brain.py       # Agent logic (LangChain + ChromaDB + Gemini)
├── init_data.py         # Standalone script to generate embeddings
├── requirements.txt     # Python dependencies
├── .env                 # API key (DO NOT COMMIT!)
├── .gitignore          # Git ignore rules
├── README.md           # This file
└── data/               # University information files (JSON)
    ├── university_qa.json          # Knowledge base Q&A articles
    └── curriculum_structured.json  # Structured course/curriculum data
```

## 🧠 Generating Embeddings with `init_data.py`

The `init_data.py` script is a **standalone tool** for creating vector embeddings from your data files. It reads the JSON files in `data/`, generates embeddings via the Gemini Embedding API, and stores them in a local ChromaDB database (`chroma_db_agent/`).

### How to Run

```bash
# Make sure your virtual environment is activated and .env has your API key
python init_data.py
```

That's it! The script will:

1. Load articles from `data/university_qa.json`
2. Load courses from `data/curriculum_structured.json`
3. Delete any existing `chroma_db_agent/` database (clean rebuild)
4. Create new embeddings and store them in ChromaDB

> ⚠️ **Note:** This script uses the Gemini Embedding API, which has rate limits. If you hit a `429 / RESOURCE_EXHAUSTED` error, the script will automatically wait 60 seconds and retry.

### When to Run

- **First time setup** — run it once after placing your data files
- **After updating data** — re-run it whenever you modify or add new JSON files in `data/`
- You do **not** need to run this every time you start the server

## 📝 Adding New Files to the RAG

To add new knowledge to the assistant, follow these steps:

### Step 1: Prepare Your Data

The system expects **two types of JSON files** in the `data/` folder:

**Knowledge Base articles** (`university_qa.json`):
```json
{
    "articles": [
        {
            "id": "unique_id",
            "title": "Article Title",
            "content": "The full content of the article...",
            "category": "general"
        }
    ]
}
```

**Structured curriculum data** (`curriculum_structured.json`):
```json
{
    "courses": [
        {
            "code": "ΗΥ-100",
            "name": "Course Name",
            "ects": 6,
            "category": "ΚΟΡΜΟΣ",
            "semester": 1,
            "prerequisites": ["ΗΥ-050"]
        }
    ],
    "categories": {
        "ΚΟΡΜΟΣ": { "name": "Core Courses", "description": "..." }
    }
}
```

### Step 2: Add or Update the JSON Files

- Edit the existing files in `data/`, or
- Replace them with new versions (e.g., generated by `PDFtoRAG/pdf_extractor.py`)

### Step 3: Regenerate Embeddings

```bash
python init_data.py
```

### Step 4: Restart the Server

```bash
python main.py
```

The assistant will now use the updated knowledge when answering questions.

## 🔧 Configuration

### Server Settings (main.py)
- `host`: Default `0.0.0.0` (all interfaces)
- `port`: Default `8000`
- `reload`: Auto-reload on code changes (development)

### RAG Settings (agent_brain.py)
- `k`: Number of relevant chunks to retrieve (3)
- `temperature`: LLM creativity (0.3 - more focused)

## 🐛 Troubleshooting

### "GOOGLE_API_KEY not found"
Make sure your `.env` file exists and contains a valid API key.

### "Connection refused" from Unity
Ensure the Python server is running on the same machine and port 8000 is not blocked.

### ChromaDB errors
Try deleting the `chroma_db_agent/` folder and restarting the server, then re-run `python init_data.py`.

##  Integration with Unity

The Unity `GeminiAPIClient.cs` is configured to call this server at `http://localhost:8000/api/chat`.
Make sure both the server and Unity are running for the NPC chat to work.
