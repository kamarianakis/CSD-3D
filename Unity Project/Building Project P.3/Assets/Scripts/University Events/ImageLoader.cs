using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader
{
    private bool isLoaded = false;
    private Texture2D texture = null;

    public Texture2D GetImage()
    {
        if (isLoaded) return texture;
        else return null;
    }

    public IEnumerator Load(string url)
    {
        if (IsLocal(url))
        {
            yield return LocalLoad(url);
        }
        else
        {
            yield return WebLoad(url);
        }
    }

    private bool IsLocal(string url)
    {
        return url[..4] != "http";
    }

    private IEnumerator LocalLoad(string url)
    {
        isLoaded = false;
        if (!File.Exists(url))
        {
            Debug.LogError($"File not found: {url}");
            yield break;
        }

        byte[] fileData = File.ReadAllBytes(url);

        texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

        if (!texture.LoadImage(fileData))
        {
            Debug.LogError("Failed to load image data.");
            yield break;
        }
        isLoaded = true;
    }

    private IEnumerator WebLoad(string url)
    {
        isLoaded = false;
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        if (request != null)
        {
            Debug.LogError($"Image loading failed: Couldn't create request from url: {url}");
            isLoaded = false;
            yield break;
        }
#if UNITY_WEBGL
        request.SetRequestHeader("Accept", "image/*");
#endif
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to load image '{url}': {request.error}");
            isLoaded = false;
            yield break;
        }

        texture = DownloadHandlerTexture.GetContent(request);

        if (texture == null)
        {
            Debug.LogError("Downloaded texture is null.");
            yield break;
        }

        isLoaded = true;
    }
}
