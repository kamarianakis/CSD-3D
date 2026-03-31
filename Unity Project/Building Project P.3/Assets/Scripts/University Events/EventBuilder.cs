using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventBuilder : MonoBehaviour
{
    // config loading variables
    public TextLoader textLoader;
    public EventParser.FileType fileType;

    // ui building variables
    public GameObject templatePanel;
    public Transform scrollContent;
    public float panelScreenHeightPrecentage = 0.6f;
    public List<GameObject> waypointObjects;

    // banner building variables
    public List<GameObject> banners;

    IEnumerator Start()
    {
        yield return textLoader.LoadFile();

        EventParser eventParser = new();
        EventConfigList eventConfigList = eventParser.Parse(textLoader.GetText(), fileType);

        yield return BuildUI(eventConfigList);
        yield return BuildBanners(eventConfigList);
    }

    // UI building
    private List<Texture2D> posterTextures = new();
    private IEnumerator LoadPosterImages(EventConfig[] events)
    {
        ImageLoader imageLoader = new();
        for (int i = 0; i < events.Length; ++i)
        {
            var e = events[i];
            if (e.posterUrl == null) continue;
            yield return imageLoader.Load(e.posterUrl);
            if (imageLoader.GetImage() != null) posterTextures.Add(imageLoader.GetImage());
        }
    }

    private IEnumerator BuildUI(EventConfigList eventConfigList)
    {
        yield return LoadPosterImages(eventConfigList.events);

        templatePanel.SetActive(false);
        LayoutElement le;

        int i = 0;
        foreach (var e in eventConfigList.events)
        {
            GameObject newPanel = Instantiate(templatePanel, scrollContent);
            le = newPanel.GetComponent<LayoutElement>();
            le.preferredHeight = Screen.height * panelScreenHeightPrecentage;

            PopulateInfoPanel(newPanel, e);
            if (e.posterUrl != null) PopulatePosterPanel(newPanel, posterTextures[i]);
            i++;
            newPanel.SetActive(true);
        }
    }

    private void PopulatePosterPanel(GameObject panel, Texture2D texture)
    {
        Image poster = panel.transform.Find("Poster Frame/Poster").GetComponent<Image>();
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        poster.sprite = sprite;
        poster.preserveAspect = true;
    }

    private void PopulateInfoPanel(GameObject panel, EventConfig eventConfig)
    {
        TMP_Text title = panel.transform.Find("Info Panel/Title").GetComponent<TMP_Text>();
        TMP_Text location = panel.transform.Find("Info Panel/Navigation/Location").GetComponent<TMP_Text>();
        TMP_Text linkText = panel.transform.Find("Info Panel/Link/Text (TMP)").GetComponent<TMP_Text>();
        OpenLink openLinkScript = panel.transform.Find("Info Panel/Link").GetComponent<OpenLink>();

        title.text = eventConfig.title;
        location.text = eventConfig.location;
        linkText.text = eventConfig.eventWebsite;
        openLinkScript.url = eventConfig.eventWebsite;

        // set the room (GameObject) that the event happens so the navigation code knows
        GameObject room = RoomFinder.FindRoom(eventConfig.location, waypointObjects);
        if (room != null)
        {
            panel.transform.Find("Info Panel/Navigation").GetComponent<EventNavigator>().Room = room;
        }
        else
        {
            Debug.Log("Couldn't find room with the name: " + eventConfig.location);
        }
    }

    // banner building
    private List<Texture2D> bannerTextures = new();
    private IEnumerator LoadBannerImages(EventConfig[] events)
    {
        ImageLoader imageLoader = new();
        for (int i = 0; i < events.Length; ++i)
        {
            var e = events[i];
            if (e.bannerUrl == null) continue;
            yield return imageLoader.Load(e.bannerUrl);
            if (imageLoader.GetImage() != null) bannerTextures.Add(imageLoader.GetImage());
            // if we loaded as many textures as many we have banners, stop loading new textures
            if(bannerTextures.Count == banners.Count) break;
        }
    }

    private void SetBannerImage(GameObject banner, Texture2D texture)
    {
        Renderer labelRenderer = banner.transform.Find("Label").GetComponent<Renderer>();
        if (labelRenderer != null) labelRenderer.material.mainTexture = texture;
        else Debug.Log("Failed to find renderer");
    }

    private IEnumerator BuildBanners(EventConfigList eventConfigList)
    {
        yield return LoadBannerImages(eventConfigList.events);

        int i = 0;
        int j = 0;
        int k = 0;
        while (i < eventConfigList.events.Length && j < banners.Count)
        {
            var e = eventConfigList.events[i];
            var b = banners[j];
            // if this event doesn't have a banner skip banner creation for this one
            if (e.bannerUrl == null) continue;
            SetBannerImage(b, bannerTextures[k]);
            b.SetActive(true);
            ++i;
            ++j;
            ++k;
        }

        // hide non used banners
        while (j < banners.Count)
        {
            banners[j].SetActive(false);
            j++;
        }
    }
}
