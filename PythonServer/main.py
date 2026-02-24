"""
FastAPI Server for University Assistant NPC
This server provides the API endpoint for the Unity game to communicate with the AI backend.
"""

import os
from contextlib import asynccontextmanager
from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel, ConfigDict
from dotenv import load_dotenv

# Load environment variables
load_dotenv()

# Import our Agent Brain (new agent-based architecture)
from agent_brain import AgentBrain

# Global variable for the brain (initialized at startup)
brain: AgentBrain = None


@asynccontextmanager
async def lifespan(app: FastAPI):
    """
    Lifespan context manager for FastAPI.
    Initializes the UniversityBrain when the server starts.
    """
    global brain
    print(" Starting University Assistant Server...")
    print("=" * 50)
    
    try:
        brain = AgentBrain(data_directory="data")
        print("=" * 50)
        print(" Server is ready to receive requests!")
        print("=" * 50)
    except Exception as e:
        print(f" Failed to initialize UniversityBrain: {e}")
        print(" Server will start but AI features will not work.")
        brain = None
    
    yield  # Server runs here
    
    # Cleanup (if needed)
    print(" Shutting down server...")


# Create FastAPI app
app = FastAPI(
    title="University Assistant API",
    description="API for the University of Crete CS Department NPC Assistant",
    version="1.0.0",
    lifespan=lifespan
)

# Configure CORS - CRITICAL for Unity WebGL!
# Allow all origins for development. In production, restrict this.
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Allows all origins (necessary for Unity WebGL)
    allow_credentials=True,
    allow_methods=["*"],  # Allows all methods
    allow_headers=["*"],  # Allows all headers
)


# Request/Response Models
class Query(BaseModel):
    """Request model for chat queries."""
    text: str
    
    model_config = ConfigDict(json_schema_extra={
        "example": {
            "text": "Where is the Computer Science department located?"
        }
    })


class Response(BaseModel):
    """Response model for chat answers."""
    response: str
    
    model_config = ConfigDict(json_schema_extra={
        "example": {
            "response": "The Computer Science Department is located at the Voutes campus in Heraklion, Crete, Greece."
        }
    })


class HealthResponse(BaseModel):
    """Response model for health check."""
    status: str
    brain_initialized: bool


# API Endpoints
@app.get("/", response_model=dict)
async def root():
    """Root endpoint - just confirms the server is running."""
    return {
        "message": "University Assistant API is running!",
        "docs": "/docs",
        "health": "/health"
    }


@app.get("/health", response_model=HealthResponse)
async def health_check():
    """Health check endpoint to verify server status."""
    return HealthResponse(
        status="healthy",
        brain_initialized=(brain is not None)
    )


@app.post("/api/chat", response_model=Response)
async def chat(query: Query):
    """
    Main chat endpoint for the Unity NPC.
    
    Receives a question from the Unity client and returns an AI-generated answer
    using RAG (Retrieval-Augmented Generation).
    """
    # Validate input
    if not query.text or not query.text.strip():
        raise HTTPException(status_code=400, detail="Query text cannot be empty")
    
    # Check if brain is initialized
    if brain is None:
        raise HTTPException(
            status_code=503,
            detail="AI service is not available. Please check server configuration."
        )
    
    try:
        # Get answer from the brain
        print(f" Received query: {query.text[:50]}...")
        answer = await brain.ask(query.text)
        print(f" Sending response: {answer[:50]}...")
        
        return Response(response=answer)
        
    except Exception as e:
        print(f" Error processing query: {e}")
        raise HTTPException(
            status_code=500,
            detail="An error occurred while processing your request."
        )


@app.post("/api/debug")
async def debug_retrieval(query: Query):
    """
    Debug endpoint to see what documents are retrieved for a query.
    """
    if brain is None:
        return {"error": "Brain not initialized"}
    
    results = brain.debug_retrieval(query.text)
    return {"query": query.text, "retrieved_documents": results}


# For running with: python main.py
if __name__ == "__main__":
    import uvicorn
    
    print("=" * 50)
    print("University Assistant Server")
    print("=" * 50)
    print("Starting server on http://localhost:8000")
    print("API Documentation: http://localhost:8000/docs")
    print("=" * 50)
    
    uvicorn.run(
        "main:app",
        host="0.0.0.0",
        port=8000,
        reload=True  # Auto-reload on code changes (development only)
    )
