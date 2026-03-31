/*This script handles the recognizing of destination rooms from text read from the repository(i.e. Timetable).*/
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TMP_TextEventHandler : MonoBehaviour
{
    private TMP_Text textMeshPro;
    private Camera mainCamera;
    private GameObject player;
    private List<GameObject> waypoints;
    private GameObject panel;
    private string roomName;
    private int open = 0;
    private int teach = 0;


    public void Setup(GameObject playerObject, List<GameObject> waypointList, GameObject panelToHide, string roomToGo, int t)
    {
        player = playerObject;
        waypoints = waypointList;
        panel = panelToHide;
        roomName = roomToGo;
        teach = t;
        if (teach == 1)
        {
            open = 1;
        }
    }

    void Awake()
    {
        textMeshPro = GetComponent<TMP_Text>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || open == 1)
        {
            open = 0;
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, mainCamera);
            if (linkIndex != -1 || teach == 1)
            {
                string clickedWord;
                if (teach == 0)
                {
                    TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
                    clickedWord = linkInfo.GetLinkID(); // The word that was clicked
                }
                else
                {
                    clickedWord = roomName;
                }
                GameObject room = RoomFinder.FindRoom(clickedWord, waypoints);

                GameObject targetObject = room != null ? room : player;

                if (targetObject != null && player != null)
                {
                    WaypointSystem waypointSystem = player.GetComponent<WaypointSystem>();
                    if (waypointSystem != null)
                    {
                        waypointSystem.SetLatestMenu(panel);
                        Transform s = targetObject.transform.GetChild(0).GetChild(2);
                        waypointSystem.SetTarget(s);
                    }
                }
            }
            teach = 0;
        }
    }
}
