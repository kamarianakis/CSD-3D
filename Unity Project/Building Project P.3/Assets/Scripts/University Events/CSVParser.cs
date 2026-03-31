
using System.Collections.Generic;

public class CSVParser
{
    // naive csv parser
    public List<List<string>> Parse(string csvtext)
    {
        List<List<string>> csvData = new();

        string[] lines = csvtext.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            List<string> fields = ParseLine(line);
            if (fields != null)
            {
                csvData.Add(fields);
            }
        }

        return csvData;
    }

    public List<List<string>> Parse(string csvtext, bool removeHeading)
    {
        var csvData = Parse(csvtext);
        if(removeHeading) csvData.RemoveAt(0);
        return csvData;
    }

    private List<string> ParseLine(string line)
    {
        if (string.IsNullOrEmpty(line)) return null; // skip empty lines
        if (line[0] == '#') return null; // skip comments

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
                // f = f.Substring(1, f.Length - 2);
                f = f[1..^1];
            }
            fields[i] = f;
        }

        return fields;
    }
}
