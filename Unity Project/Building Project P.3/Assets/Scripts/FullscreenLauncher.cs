/*This script handles the mobiles loading the program. It provides them with a button that when pressed, maximizes the game
 for all mobile devices*/
using UnityEngine;
using TMPro;

public class FullscreenManager : MonoBehaviour
{
    public GameObject overlayUI; // Black "Tap to Begin" screen
    public TextMeshProUGUI LoadingLabel;
    public GameObject Mobile;
    public GameObject PC;
    public GameObject Arrow;
    public GameObject player;
    public WaypointSystem waypointSystem; // Reference to WaypointSystem to start the game

    private bool hasStarted = false;

    void Start()
    {
#if UNITY_WEBGL
        if (Application.isMobilePlatform)
        {
            ShowOverlay();
        }
        else
        {
            this.enabled = false; // disable fullscreen logic on desktop
        }
#endif
    }

    void Update()
    {
        // Don't interfere if already started
        if (hasStarted)
        {
            return;
        }
        
        // If fullscreen is lost (e.g. ESC pressed), show overlay again
        if (!Screen.fullScreen && !hasStarted)
        {
            ShowOverlay();
        }
    }

    public void OnUserTap()
    {
        // Prevent multiple taps
        if (hasStarted)
        {
            return;
        }
        
        // This MUST be directly inside a UI event (like a button)
        hasStarted = true;
        
        if (!Screen.fullScreen)
        {
            // Set immediately on user input — not in coroutine
            Screen.fullScreen = true;
        }

        // Start the game regardless of fullscreen status (for testing in editor)
        ShowLoadingThenStart();
    }

    private void ShowLoadingThenStart()
    {
        Mobile.SetActive(false);
        LoadingLabel.gameObject.SetActive(true);
        Invoke(nameof(HideOverlayAndStart), 0.5f); // slight delay to simulate load
    }

    private void HideOverlayAndStart()
    {
        LoadingLabel.gameObject.SetActive(false);
        overlayUI.SetActive(false);
        
        // Start the actual game
        if (waypointSystem != null)
        {
            waypointSystem.ActualStart(true); // true = mobile mode
        }
        else if (player != null)
        {
            // Fallback: try to find WaypointSystem on player
            WaypointSystem ws = player.GetComponent<WaypointSystem>();
            if (ws != null)
            {
                ws.ActualStart(true);
            }
        }
    }

    private void ShowOverlay()
    {
        overlayUI.SetActive(true);
        Mobile.SetActive(true);
    }
}
