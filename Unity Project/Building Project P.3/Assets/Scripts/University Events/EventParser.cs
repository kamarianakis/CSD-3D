using UnityEngine;
using System.Collections.Generic;

public class EventParser
{
    public enum FileType
    {
        CSV,
        JSON
    }

    public EventConfigList Parse(string textFile, FileType fileType)
    {
        if (fileType == FileType.JSON)
        {
            return JsonUtility.FromJson<EventConfigList>(textFile);
        } 
        else if (fileType == FileType.CSV)
        {
            CSVParser csvParser = new();
            List<List<string>> csvData = csvParser.Parse(textFile, removeHeading: true);

            EventConfigList eventConfigList = new()
            {
                events = new EventConfig[csvData.Count]
            };
            for (int i = 0; i < csvData.Count; ++i)
            {
                List<string> line = csvData[i];
                eventConfigList.events[i] = new();
                if (line[0] != null) eventConfigList.events[i].title = line[0];
                if (line[1] != null) eventConfigList.events[i].location = line[1];
                if (line[2] != null) eventConfigList.events[i].posterUrl = line[2];
                if (line[3] != null) eventConfigList.events[i].bannerUrl = line[3];
                if (line[4] != null) eventConfigList.events[i].eventWebsite = line[4];
            }
            return eventConfigList;
        }
        else
        {
            Debug.Log("Unknown file type");
            return null;
        }
    }
}
