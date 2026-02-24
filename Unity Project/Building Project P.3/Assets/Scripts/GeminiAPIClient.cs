/*This script handles communication with the Python backend server for LLM responses.
 * The server runs FastAPI + LangChain + ChromaDB + Gemini for RAG-based answers.*/
using UnityEngine;
using System.Collections;
using System.Text;

public class GeminiAPIClient : MonoBehaviour
{
    [Header("Server Configuration")]
    [Tooltip("URL of the Python backend server")]
    public string serverUrl = "http://localhost:8000";
    
    [Tooltip("API endpoint path")]
    public string chatEndpoint = "/api/chat";
    
    [Header("Settings")]
    [Tooltip("Request timeout in seconds")]
    public float requestTimeout = 30f;
    
    // Callback type for API responses
    public delegate void APIResponseCallback(string response, bool success);
    
    void Start()
    {
        Debug.Log($"GeminiAPIClient: Configured to use server at {serverUrl}{chatEndpoint}");
    }
    
    /// <summary>
    /// Sends a message to the Python backend and gets a RAG-powered response
    /// </summary>
    public IEnumerator SendMessage(string userMessage, APIResponseCallback callback)
    {
        string url = $"{serverUrl}{chatEndpoint}";
        
        // Create request body JSON
        string jsonBody = "{\"text\":\"" + EscapeJsonString(userMessage) + "\"}";
        byte[] bodyData = Encoding.UTF8.GetBytes(jsonBody);
        
        Debug.Log($"GeminiAPIClient: Sending request to {url}");
        
        // Send request
        using (UnityEngine.Networking.UnityWebRequest request = new UnityEngine.Networking.UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(bodyData);
            request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            float startTime = Time.time;
            var operation = request.SendWebRequest();
            
            // Wait for response with timeout
            while (!operation.isDone)
            {
                if (Time.time - startTime > requestTimeout)
                {
                    Debug.LogError("GeminiAPIClient: Request timeout!");
                    callback?.Invoke("Sorry, the request timed out. Please try again.", false);
                    yield break;
                }
                yield return null;
            }
            
            // Process response
            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log($"GeminiAPIClient: Received response: {responseText}");
                
                string extractedResponse = ParseResponse(responseText);
                callback?.Invoke(extractedResponse, true);
            }
            else
            {
                Debug.LogError($"GeminiAPIClient: Request failed: {request.error}");
                Debug.LogError($"GeminiAPIClient: Response code: {request.responseCode}");
                
                // Provide helpful error message based on error type
                string errorMessage = "Sorry, I couldn't connect to my brain. ";
                if (request.responseCode == 0)
                {
                    errorMessage += "The server might not be running. Please start the Python server.";
                }
                else
                {
                    errorMessage += "Please try again later.";
                }
                
                callback?.Invoke(errorMessage, false);
            }
        }
    }
    
    /// <summary>
    /// Escapes special characters for JSON string
    /// </summary>
    string EscapeJsonString(string str)
    {
        return str
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }
    
    // Response class for JSON deserialization
    [System.Serializable]
    private class APIResponse
    {
        public string response;
    }
    
    /// <summary>
    /// Parses the API response JSON to extract the text
    /// </summary>
    string ParseResponse(string jsonResponse)
    {
        try
        {
            Debug.Log($"GeminiAPIClient: Raw JSON to parse: {jsonResponse}");
            
            // Use Unity's JsonUtility for proper Unicode handling
            APIResponse apiResponse = JsonUtility.FromJson<APIResponse>(jsonResponse);
            
            if (apiResponse != null && !string.IsNullOrEmpty(apiResponse.response))
            {
                Debug.Log($"GeminiAPIClient: Successfully parsed response: {apiResponse.response.Substring(0, Mathf.Min(50, apiResponse.response.Length))}...");
                return apiResponse.response;
            }
            else
            {
                Debug.LogWarning("GeminiAPIClient: Parsed response was null or empty");
                return "Sorry, I received an empty response.";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"GeminiAPIClient: Error parsing response: {e.Message}");
            Debug.LogError($"GeminiAPIClient: JSON was: {jsonResponse}");
            return "Sorry, there was an error processing the response.";
        }
    }
}
