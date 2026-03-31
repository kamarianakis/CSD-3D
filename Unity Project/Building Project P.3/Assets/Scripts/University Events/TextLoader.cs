using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName ="TextLoader", menuName ="ScriptableObjects/TextLoader", order = 1)]
public class TextLoader : ScriptableObject
{
    public bool isLocal = true;
    public string url = null;

    private bool isLoaded = false;

    private string text = null;

    public string GetText()
    {
        if (isLoaded)
        {
            return text;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator LoadFile()
    {
        if (url == null)
        {
            Debug.LogError("URL can't be null");
            yield break;
        }

        if (isLocal)
        {
            StreamReader reader = new StreamReader(url);
            text = reader.ReadToEnd();
            reader.Close();
            isLoaded = true;
        }
        else
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            if (request != null)
            {
                Debug.LogError("File loading failed: Couldn't create request from url: " + url);
                isLoaded = false;
                yield break;
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("File loading failed: " + request.error);
                isLoaded = false;
                yield break;
            }

            text = request.downloadHandler.text;
            isLoaded = true;
        }
    }
}
