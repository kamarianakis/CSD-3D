using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ImageIDAssetLoader : MonoBehaviour
{
    private struct ImagePlacementData
    {
        public string Uri;
        public Vector2 dimensions;
        public Vector2 offset;
        public bool dimOverride;
        public bool offsetOverride;
    }

    public string ImageMatchCSVURL = "https://raw.githubusercontent.com/kamarianakis/CSD-3D/refs/heads/main/Excel%20Files/IDToImage.csv";
    public bool isLocalURL = false;

    private static ImageIDAssetLoader _instance = null;
    private static Dictionary<string, ImagePlacementData> _matchings = null;

    public void Start()
    {
        if(_instance != null)
        {
            Debug.LogError("Only one ImageIDAssetLoader allowed at the same time.");
            return;
        }

        _instance = this;
        _matchings = new Dictionary<string, ImagePlacementData>();

        if (isLocalURL)
        {
            string csvData = LocalFileReader.LoadText(ImageMatchCSVURL);
            ParseAndRegisterCSVMatches(csvData);
        }
        else
        {
            StartCoroutine(LoadMatchings());
        }
    }

    private IEnumerator LoadMatchings()
    {
        UnityWebRequest request = UnityWebRequest.Get(ImageMatchCSVURL);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download CSV: " + request.error);
        }
        else
        {
            string csvData = request.downloadHandler.text;
            ParseAndRegisterCSVMatches(csvData);
        }
    }

    static void ParseAndRegisterCSVMatches(string csvString)
    {
        // Split by line breaks (handles both Windows and Unix endings)
        string[] lines = csvString.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        // Start at i = 1 to skip the header row (nameId, imageId)
        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');

            // Ensure enough columns, discard extras
            if (columns.Length >= 2)
            {
                string nameId = columns[0].Trim();
                string imageId = columns[1].Trim();
                Vector2 dimensions = Vector2.one; 
                Vector2 offset = Vector2.zero;
                bool dimOverride = false;
                bool offsetOverride = false;

                try
                {
                    if (columns.Length >= 4)
                    {
                        dimensions = new Vector2(float.Parse(columns[2].Trim()), float.Parse(columns[3].Trim()));
                        dimOverride = true;
                    }

                    if (columns.Length >= 6)
                    {
                        offset = new Vector2(float.Parse(columns[4].Trim()), float.Parse(columns[5].Trim()));
                        offsetOverride = true;
                    }
                } catch (FormatException e)
                {
                    // Error => Skip this entry
                    Debug.LogError($"Failed to parse image entry:{e.Message}, skipping...");
                    continue;
                }

                ImagePlacementData imagePlacementData = new ImagePlacementData { 
                    Uri = imageId,
                    dimensions = dimensions,
                    offset = offset,
                    dimOverride = dimOverride,
                    offsetOverride = offsetOverride
                };

                if (!_matchings.ContainsKey(nameId))
                {
                    _matchings.Add(nameId, imagePlacementData);
                }
            }
        }

        Debug.Log($"Loaded {_matchings.Count} image asset entries from CSV.");
    }

    // Asynchronously sets the image linked to the provided id
    // to the 'image' object
    public static void SetImageByID(UnityEngine.UI.Image image, string id)
    {
        // Works as a proxy for _SetImageByID

        if(_instance == null)
        {
            Debug.LogError("No Image ID Asset Loader found!");
            return;
        }

        _instance.StartCoroutine(_instance._SetImageByID(image, id));
    }

    public static bool ExistsImageWithID(string id)
    {
        return _matchings != null && _matchings.ContainsKey(id);
    }

    private IEnumerator _SetImageByID(UnityEngine.UI.Image image, string id)
    {
        if (_matchings != null && _matchings.ContainsKey(id))
        {
            ImagePlacementData imageData = _matchings[id];

            if(imageData.Uri != null)
            {
                string filePath = Path.Combine(
                    UnityEngine.Application.streamingAssetsPath, 
                    "ImageAssets", 
                    imageData.Uri
                );

                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filePath))
                {
                    yield return uwr.SendWebRequest();

                    if (uwr.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Error loading image '{filePath}': " + uwr.error);
                    }
                    else
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                        AssignTextureToImage(image, texture);

                        // Modify dimensions and offset accordingly
                        if(imageData.dimOverride && image.TryGetComponent<RectTransform>(out RectTransform rect))
                        {
                            rect.sizeDelta = imageData.dimensions;
                        }

                        if(imageData.offsetOverride)
                            image.transform.position += new Vector3(0f, imageData.offset.y, imageData.offset.x);
                    }
                }
            }
        }
    }

    private void AssignTextureToImage(UnityEngine.UI.Image uiImage, Texture2D tex)
    {
        if (tex == null) return;

        // Create a Sprite from the Texture
        Sprite newSprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );

        // Assign it to the UI Image component
        uiImage.sprite = newSprite;
        uiImage.color = Color.white;
    }
}
