/*This script manages vectorized storage and context retrieval for university information.
 * It reads from CSV files and provides relevant context for user queries.*/
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class VectorStoreManager : MonoBehaviour
{
    [Header("Data Sources")]
    [Tooltip("Path to Excel Files folder (relative to Assets)")]
    public string dataFolderPath = "../Excel Files";
    
    [Tooltip("Number of top results to return")]
    public int topKResults = 5;
    
    [Tooltip("Minimum similarity score (0-1)")]
    public float similarityThreshold = 0.3f;
    
    // Data storage
    private List<UniversityData> allData = new List<UniversityData>();
    
    // Cache for loaded data
    private bool dataLoaded = false;
    
    void Start()
    {
        LoadUniversityData();
    }
    
    /// <summary>
    /// Loads all university data from CSV files
    /// </summary>
    void LoadUniversityData()
    {
        if (dataLoaded)
        {
            return;
        }
        
        string basePath = Application.dataPath;
        string fullPath = Path.Combine(basePath, dataFolderPath);
        
        // Load data from CSV files
        LoadCSVFile(Path.Combine(fullPath, "TeachersInfo.csv"), DataType.Teacher);
        LoadCSVFile(Path.Combine(fullPath, "WinterSemester.csv"), DataType.Course);
        LoadCSVFile(Path.Combine(fullPath, "SpringSemester.csv"), DataType.Course);
        LoadCSVFile(Path.Combine(fullPath, "SubjectLists.csv"), DataType.Subject);
        LoadCSVFile(Path.Combine(fullPath, "StandardCurriculum.csv"), DataType.Curriculum);
        LoadCSVFile(Path.Combine(fullPath, "DoorNames.csv"), DataType.Location);
        
        dataLoaded = true;
        Debug.Log($"VectorStoreManager: Loaded {allData.Count} data entries.");
    }
    
    /// <summary>
    /// Loads data from a CSV file
    /// </summary>
    void LoadCSVFile(string filePath, DataType type)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"VectorStoreManager: File not found: {filePath}");
            return;
        }
        
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
            {
                return; // No data rows
            }
            
            // Skip header row
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = ParseCSVLine(lines[i]);
                if (fields.Length > 0)
                {
                    UniversityData data = new UniversityData
                    {
                        type = type,
                        rawData = lines[i],
                        fields = fields
                    };
                    allData.Add(data);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"VectorStoreManager: Error loading {filePath}: {e.Message}");
        }
    }
    
    /// <summary>
    /// Parses a CSV line (handles quoted fields)
    /// </summary>
    string[] ParseCSVLine(string line)
    {
        List<string> fields = new List<string>();
        bool inQuotes = false;
        string currentField = "";
        
        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(currentField.Trim());
                currentField = "";
            }
            else
            {
                currentField += c;
            }
        }
        fields.Add(currentField.Trim());
        
        return fields.ToArray();
    }
    
    /// <summary>
    /// Gets relevant context for a user query
    /// </summary>
    public string[] GetContextForQuery(string query)
    {
        if (!dataLoaded)
        {
            LoadUniversityData();
        }
        
        if (allData.Count == 0)
        {
            return new string[] { "No university data available." };
        }
        
        // Simple keyword-based search (can be upgraded to vector similarity later)
        List<SearchResult> results = new List<SearchResult>();
        string queryLower = query.ToLower();
        string[] queryWords = queryLower.Split(' ');
        
        foreach (var data in allData)
        {
            float score = CalculateRelevanceScore(data, queryLower, queryWords);
            if (score >= similarityThreshold)
            {
                results.Add(new SearchResult { data = data, score = score });
            }
        }
        
        // Sort by score and take top K
        results = results.OrderByDescending(r => r.score).Take(topKResults).ToList();
        
        // Format results
        List<string> context = new List<string>();
        foreach (var result in results)
        {
            context.Add(FormatDataForContext(result.data));
        }
        
        return context.ToArray();
    }
    
    /// <summary>
    /// Calculates relevance score for a data entry
    /// </summary>
    float CalculateRelevanceScore(UniversityData data, string queryLower, string[] queryWords)
    {
        float score = 0f;
        string searchText = string.Join(" ", data.fields).ToLower();
        
        // Exact phrase match
        if (searchText.Contains(queryLower))
        {
            score += 1.0f;
        }
        
        // Word matches
        foreach (string word in queryWords)
        {
            if (word.Length > 2 && searchText.Contains(word))
            {
                score += 0.3f;
            }
        }
        
        // Type-specific boosts
        if (queryLower.Contains("teacher") || queryLower.Contains("professor") || queryLower.Contains("staff"))
        {
            if (data.type == DataType.Teacher) score += 0.5f;
        }
        if (queryLower.Contains("course") || queryLower.Contains("subject") || queryLower.Contains("class"))
        {
            if (data.type == DataType.Course || data.type == DataType.Subject) score += 0.5f;
        }
        if (queryLower.Contains("room") || queryLower.Contains("location") || queryLower.Contains("office"))
        {
            if (data.type == DataType.Location) score += 0.5f;
        }
        
        return score;
    }
    
    /// <summary>
    /// Formats data entry for context
    /// </summary>
    string FormatDataForContext(UniversityData data)
    {
        switch (data.type)
        {
            case DataType.Teacher:
                if (data.fields.Length >= 2)
                {
                    return $"Teacher: {data.fields[0]} - {data.fields[1]}";
                }
                break;
            case DataType.Course:
                if (data.fields.Length >= 3)
                {
                    return $"Course: {data.fields[0]} - {data.fields[1]} ({data.fields[2]})";
                }
                break;
            case DataType.Subject:
                if (data.fields.Length >= 1)
                {
                    return $"Subject: {data.fields[0]}";
                }
                break;
            case DataType.Location:
                if (data.fields.Length >= 1)
                {
                    return $"Location: {data.fields[0]}";
                }
                break;
        }
        
        return data.rawData;
    }
    
    // Data structures
    private enum DataType
    {
        Teacher,
        Course,
        Subject,
        Curriculum,
        Location
    }
    
    private class UniversityData
    {
        public DataType type;
        public string rawData;
        public string[] fields;
    }
    
    private class SearchResult
    {
        public UniversityData data;
        public float score;
    }
}

