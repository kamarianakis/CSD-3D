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

    private bool hasStarted = false;

    void Start()
    {
//#if UNITY_WEBGL
        if (Application.isMobilePlatform)
        {
            ShowOverlay();
        }
        else
        {
            this.enabled = false; // disable fullscreen logic on desktop
        }
//#endif
    }

    void Update()
    {
        // If fullscreen is lost (e.g. ESC pressed), show overlay again
        if (!Screen.fullScreen)
        {
            ShowOverlay();
        }
        else
        {
            ShowLoadingThenStart();
        }
    }

    public void OnUserTap()
    {
        // This MUST be directly inside a UI event (like a button)
        if (!Screen.fullScreen)
        {
            // Set immediately on user input — not in coroutine
            Screen.fullScreen = true;
            hasStarted = true;

            // Assume fullscreen will succeed; let Unity handle failure silently
            ShowLoadingThenStart();
        }
        else
        {
            ShowLoadingThenStart();
        }
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
    }

    private void ShowOverlay()
    {
        overlayUI.SetActive(true);
        Mobile.SetActive(true);
    }
}
