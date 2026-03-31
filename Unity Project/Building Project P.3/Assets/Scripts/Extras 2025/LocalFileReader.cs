using System;
using System.IO;
using UnityEngine;

public static class LocalFileReader
{
    public static string LoadText(string URL)
    {
        try
        {
            StreamReader reader = new StreamReader(URL);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }
        catch (IOException e)
        {
            Debug.LogException(e);
        }

        return null;
    }
}
