using UnityEngine;

public class RefreshExitNavManager : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public GameObject refreshUI;
    public EmergencyExitScript exitScript;
    public KeyCode refreshKey;

    private bool _currState  = false; 

    public void Update()
    {
        bool isExiting = playerInfo.IsExiting();

        if (isExiting != _currState)
        {
            _currState = isExiting;
            refreshUI.SetActive(_currState);
        }

        // Only check for refresh if exit navigation is enabled
        if(_currState)
        {
            CheckForRefreshKey();
        }
    }

    public void CheckForRefreshKey()
    {
        if(Input.GetKeyUp(refreshKey))
        {
            exitScript.NavigateToNearestExit();
        }
    }
}
