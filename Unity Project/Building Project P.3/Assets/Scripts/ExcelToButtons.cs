/*This script handles the assignment of the Level 1-3 button information from the corresponding repository*/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExcelToButtons : MonoBehaviour
{
    // ✅ PUBLIC REPOSITORY URL (No authentication needed)
    public string buttonNameCSVURL = "https://raw.githubusercontent.com/kamarianakis/CSD-3D/refs/heads/main/Excel%20Files/ButtonNames.csv";
    public bool isLocalURL = false;

    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject Main1;
    public GameObject Main2;
    public GameObject Main3;
    public GameObject A1;
    public GameObject B1;
    public GameObject E1;
    public GameObject H1;
    public GameObject E2;
    public GameObject B2;
    public GameObject H2;
    public GameObject K2;
    public GameObject E3;
    public GameObject B3;
    public GameObject H3;
    public GameObject K3;

    void Start()
    {
        if (isLocalURL)
        {
            string csvData = LocalFileReader.LoadText(buttonNameCSVURL);
            ProcessCSV(csvData);
        }
        else
        {
            StartCoroutine(DownloadCSV());
        }
    }

    IEnumerator DownloadCSV()
    {

        UnityWebRequest request = UnityWebRequest.Get(buttonNameCSVURL);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download CSV: " + request.error);
        }
        else
        {
            string csvData = request.downloadHandler.text;
            ProcessCSV(csvData);
        }
    }

    void ProcessCSV(string csvText)
    {
        level1.SetActive(true);
        level2.SetActive(true);
        level3.SetActive(true);

        A1.SetActive(true);
        B1.SetActive(true);
        E1.SetActive(true);
        H1.SetActive(true);
        E2.SetActive(true);
        B2.SetActive(true);
        H2.SetActive(true);
        K2.SetActive(true);
        E3.SetActive(true);
        B3.SetActive(true);
        H3.SetActive(true);
        K3.SetActive(true);
        Main1.SetActive(true);

        string[] allLines = csvText.Split('\n');
        int lineIndex = 0;

        foreach (string line in allLines)
        {
            lineIndex++;
            if (lineIndex <= 17) continue; // Ignore first 17 lines
            if (string.IsNullOrEmpty(line)) continue;

            // Use the improved parser here
            string[] entries = ParseCsvLineSimple(line);

            if (entries.Length < 2)
            {
                continue;
            }

            string buttonName = entries[0].Trim();
            string nameText = entries[1].Trim();

            Button button = GameObject.Find(buttonName)?.GetComponent<Button>();

            if (button != null)
            {
                TextMeshProUGUI nameTextMesh = button.transform.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();

                if (nameTextMesh != null)
                {
                    nameTextMesh.text = nameText;
                }
                else
                {
                    Debug.LogWarning("Could not find TextMeshPro component on button: " + buttonName);
                }
            }
            else
            {
                Debug.LogWarning("Button not found with name: " + buttonName);
            }
        }

        // Disable buttons after processing
        A1.SetActive(false);
        B1.SetActive(false);
        E1.SetActive(false);
        H1.SetActive(false);
        E2.SetActive(false);
        B2.SetActive(false);
        H2.SetActive(false);
        K2.SetActive(false);
        E3.SetActive(false);
        B3.SetActive(false);
        H3.SetActive(false);
        K3.SetActive(false);

        level1.SetActive(false);
        level2.SetActive(false);
        level3.SetActive(false);

        Main2.SetActive(true);
        Main3.SetActive(true);
    }

    string[] ParseCsvLineSimple(string line)
    {
        List<string> fields = new List<string>();
        var current = new System.Text.StringBuilder();
        bool insideQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"'); // Escaped quote
                    i++; // Skip next quote
                }
                else
                {
                    insideQuotes = !insideQuotes; // Toggle quoted state
                }
            }
            else if (c == ',' && !insideQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString());

        // Remove wrapping quotes if present
        for (int i = 0; i < fields.Count; i++)
        {
            string f = fields[i];
            if (f.StartsWith("\"") && f.EndsWith("\"") && f.Length >= 2)
            {
                f = f.Substring(1, f.Length - 2);
            }
            fields[i] = f;
        }

        return fields.ToArray();
    }
}