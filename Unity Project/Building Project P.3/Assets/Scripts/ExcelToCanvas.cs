/*This function handles the reading and assigning of the door names that float in front of the doors, from a online repository*/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExcelToCanvas : MonoBehaviour
{
    // ✅ PUBLIC REPOSITORY URL (No authentication needed) | or local file for testing
    public string doorNameCSVURL = "https://raw.githubusercontent.com/kamarianakis/CSD-3D/refs/heads/main/Excel%20Files/DoorNames.csv";
    public bool isLocalURL = false;

    void Start()
    {
        if (isLocalURL)
        {
            string csvData = LocalFileReader.LoadText(doorNameCSVURL);
            ProcessCSV(csvData);
        } else {
            StartCoroutine(DownloadCSV());
        }
    }

    IEnumerator DownloadCSV()
    {

        UnityWebRequest request = UnityWebRequest.Get(doorNameCSVURL);

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

    private GameObject FindFirstNotNull(params GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                return obj;
            }
        }

        return null;
    }

    void ProcessCSV(string csvText)
    {
        string[] allLines = csvText.Split('\n');
        int lineIndex = 0;

        foreach (string line in allLines)
        {
            lineIndex++;
            if (lineIndex <= 21) continue; // Ignore first 21 lines
            if (string.IsNullOrEmpty(line)) continue;

            // Use quoted-aware CSV parser here instead of line.Split(',')
            string[] entries = ParseCsvLineSimple(line);

            if (entries.Length < 4)
            {
                continue;
            }

            string canvasName = entries[0].Trim();
            string nameText = entries[1].Trim();
            string detailsText = entries[2].Trim();
            string extraInfoText = entries[3].Trim();

            Canvas canvas = GameObject.Find(canvasName)?.GetComponent<Canvas>();

            if (canvas != null)
            {
                TextMeshProUGUI nameTextMesh = canvas.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI detailsTextMesh = canvas.transform.Find("Details")?.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI extraInfoTextMesh = canvas.transform.Find("Extra Info")?.GetComponent<TextMeshProUGUI>();

                GameObject imageAnchor = FindFirstNotNull(
                    nameTextMesh != null ? nameTextMesh.gameObject : null,
                    detailsTextMesh != null ? detailsTextMesh.gameObject : null,
                    extraInfoTextMesh != null ? extraInfoTextMesh.gameObject : null
                );

                Transform imagePanel = canvas.transform.Find("Image");

                // If there's no existing image create one (if there's a corresponding image to load).
                if (imagePanel == null && imageAnchor != null && ImageIDAssetLoader.ExistsImageWithID(nameText))
                {
                    GameObject newImagePanelObj = Instantiate(new GameObject("Image"));
                    newImagePanelObj.AddComponent<Image>();
                    imagePanel = newImagePanelObj.transform;

                    imagePanel.name = "Image";
                    imagePanel.SetParent(canvas.transform);

                    imagePanel.SetPositionAndRotation(
                        imageAnchor.transform.position, 
                        imageAnchor.transform.rotation
                    );
                }

                Image itemImage = imagePanel != null ? imagePanel.GetComponent<Image>() : null;

                if (nameTextMesh != null) nameTextMesh.text = nameText;
                if (detailsTextMesh != null) detailsTextMesh.text = detailsText;
                if (extraInfoTextMesh != null) extraInfoTextMesh.text = extraInfoText;
                if (itemImage != null) ImageIDAssetLoader.SetImageByID(itemImage, nameText);

                canvas.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Canvas not found with name: " + canvasName);
            }
        }
    }

    // Quoted-aware CSV line parser (same as before)
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