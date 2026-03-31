using System;

// this is a helper class to load an array of EventConfigs because unity doesn't support it out of the box
[Serializable]

public class EventConfigList
{
    // the name events implies that the json config file will have a field named events that will be an array of objects of type EventConfig
    public EventConfig[] events;
}
