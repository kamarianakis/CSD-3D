/*This script some of the most necessary functionalities of the platform*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro; // For TextMeshPro
using System;
using System.Collections;

public class WaypointSystem : MonoBehaviour
{
    public Transform target;
    public LineRenderer lineRenderer;
    public NavMeshAgent agent;

    public Canvas uiCanvas;
    public TextMeshProUGUI promptLabel;  // "Press E for Destinations"
    public TextMeshProUGUI cancelLabel;  // "Press X to Cancel"
    // public GameObject destinationButtonsPanel; // Panel that contains destination buttons

    public GameObject MainMenuButtons;

    private GameObject latestMenu;

    private GameObject latestRoom;

    public GameObject player;

    public Toggle destinationModeToggle; // Reference to the Toggle checkbox
    public Transform[] potentialDestinations; // Array of possible destination targets

    public bool elevatorUse = false;

    public string elevatorToBeUsed;

    public int level;
    public Transform elevatorFinalTarget;
    public int floorchangerelevator;

    public string axis;
    public GameObject Game;

    [Header("NPC Interaction")]
    public bool NPCInteractionEnabled = true;
    public NPCInteractionDetector npcDetector;

    private bool EbeingAbledToUse = true;
    public bool XbeingAbledToUse = false;
    private GameObject forwintersummer;

    public GameObject minimapPanels;
    public GameObject minimap;
    public GameObject minimapbirdview;
    public GameObject Marker;
    public GameObject miniFrame;
    public GameObject bigMapMiniPanel;
    public GameObject bigMapMarker;
    public GameObject bigMapminiPanelDestinations;
    public GameObject max;
    public GameObject hide;
    public GameObject show;
    public GameObject forMap;
    public GameObject MenuminimapPanel;
    public GameObject[] BibliotekaTargets;
    public GameObject togglerpanel;
    public GameObject maindestinationtogglepanel;
    public GameObject Barriers;
    public GameObject buttonOpenMenu;
    public GameObject buttonOpenMinimap;
    public GameObject buttonHideMinimap;
    public GameObject buttonMaxMinimap;
    public GameObject joysticks;
    public GameObject closenavigationbutton;
    public GameObject Colliders;
    public int Toggler1 = 0;
    public int Toggler2 = 0;
    public GameObject rw2;
    public GameObject ChangeDevice;

    public GameObject Flags;
    public GameObject startFlags;
    public GameObject A_M_E_A_spawnpoint;
    public GameObject WASD_icon;
    public GameObject Middlefirst;
    public GameObject[] StairwayColliders;
    private int STC = 0;
    public bool languages = true;
    public GameObject ButtonAppearance;
    public GameObject L_M_Label;
    public GameObject specialobstacles;
    public Button ChangeDeviceButton;
    public GameObject Music_Icon;
    public GameObject Music_Button;

    private bool MbeingAbledToUse = true;
    private bool tempM;
    private bool HbeingAbledToUse = true;
    private bool tempH;
    private bool minimapActive = false;
    public bool Tog1switcher = false;
    public bool Tog2switcher = false;
    public bool mobileusage = false;
    private bool minimaponMobile = false;
    public bool CbeingAbledToUse = true;
    public bool LbeingAbledToUse = false;
    private Vector2 minimapos;
    private Vector2 miniFramepos;
    private bool A_M_E_A_ = false;
    public bool lfirstUse = false;
    private int fingerCount;


    public Light directionalLight;
    public Material skyMaterial;
    private Material runtimeSkybox;


    /*The 'Start()' function sets up some initial states of the program*/
    void Start()
    {
        //The agent of the naviagation path finding system is set for usage in the program
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;  // Enable once NavMesh is ready
        if (lineRenderer == null)
        {
            Debug.LogError("Please assign a Line Renderer component.");
        }
        MbeingAbledToUse = false;
        HbeingAbledToUse = false;
        EbeingAbledToUse = false;
        CbeingAbledToUse = false;
        // Game.GetComponent<ItemCollector>().enabled = false;
        player.GetComponent<Movement_Player>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        RectTransform mp = minimap.GetComponent<RectTransform>();
        Vector2 pos = mp.anchoredPosition;
        minimapos.y = pos.y;
        mp = miniFrame.GetComponent<RectTransform>();
        pos = mp.anchoredPosition;
        miniFramepos.y = pos.y;
    }

    private IEnumerator EnableAgentWhenNavMeshReady()
    {
        // Disable agent to avoid premature use
        agent.enabled = false;

        // Wait until there's a valid NavMesh near the agent’s position
        while (!NavMesh.SamplePosition(agent.transform.position, out _, 1.0f, NavMesh.AllAreas))
        {
            yield return null; // wait a frame and check again
        }
        agent.enabled = true;  // Enable once NavMesh is ready
    }
    /*The 'Update()' function checks for multiple conditions which determine what the next functionality to happen(if any) will be*/
    void Update()
    {
        if (mobileusage == false || (mobileusage == true && player.GetComponent<Movement_Player>().isdeactivated == false))
        {
            //First, it checks if there is any keyboard input from the user with 'HandleInput()'
            HandleInput();
        }
        if (mobileusage == true)
        {
            fingerCount = Input.touchCount;
            if (fingerCount == 5)
            {
                do_L();
            }
        }
        fingerCount = Input.touchCount;
        if (mobileusage == false && fingerCount == 1 && IsTapOverButton(Input.GetTouch(0).position, ChangeDeviceButton) == true && player.GetComponent<Movement_Player>().isdeactivated == false)
        {
            ChangeStart();
        }
        //Then it checks if a destination target has been set and acts accordingly
        if (target != null)
        {
            //If a destination target has been selected, a destination is set using the agent's function 'SetDestination()' 
            //and the target's position
            agent.SetDestination(target.position);

            //Then, 'DrawPath()' is called to draw the path from the user's position and the selected target
            DrawPath(agent.path);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    bool IsTapOverButton(Vector2 screenPos, Button button)
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        Canvas canvas = button.GetComponentInParent<Canvas>();

        // Convert screen space to local point in RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint
        );

        return rect.rect.Contains(localPoint);
    }

    private bool forceCursorUnlock = false;

    /*Actually starts the program, is called when a device is chosen from the Start Screen.*/
    public void ActualStart(bool mobile)
    {
        MbeingAbledToUse = false;
        HbeingAbledToUse = true;
        EbeingAbledToUse = true;
        CbeingAbledToUse = true;
        player.GetComponent<Movement_Player>().enabled = true;
        Game.GetComponent<ItemCollector>().enabled = true;
        if (mobile == true)
        {
            mobileusage = true;
            joysticks.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            buttonOpenMenu.SetActive(true);
            Game.GetComponent<ItemCollector>().mobileusage = true;
            RectTransform mp = minimap.GetComponent<RectTransform>();
            Vector2 pos = mp.anchoredPosition;
            pos.y = 449f;
            mp.anchoredPosition = pos;
            mp = miniFrame.GetComponent<RectTransform>();
            pos = mp.anchoredPosition;
            pos.y = 447f;
            mp.anchoredPosition = pos;
            buttonOpenMinimap.SetActive(true);
            show.SetActive(false);
        }
        else
        {
            Game.GetComponent<ItemCollector>().mobileusage = false;
            RectTransform mp = minimap.GetComponent<RectTransform>();
            Vector2 pos = mp.anchoredPosition;
            pos.y = minimapos.y;
            mp.anchoredPosition = pos;
            mp = miniFrame.GetComponent<RectTransform>();
            pos = mp.anchoredPosition;
            pos.y = miniFramepos.y;
            mp.anchoredPosition = pos;
            Cursor.lockState = CursorLockMode.Locked;
            //The label containing the message of how to open the menu with all the functionalities is turned on
            promptLabel.gameObject.SetActive(true);
            show.SetActive(true);

        }
        Cursor.visible = false;
        //The canvas containing most of the UI elements of the program is turned on
        uiCanvas.gameObject.SetActive(true);
        ChangeDevice.SetActive(true);

        latestMenu = MainMenuButtons;
        
        // Clear any selected UI element so game keys work from the start
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private bool once = true;

    /*Start done for the official Limited Mobility entrance.*/
    public void doStart(GameObject MainMenu)
    {
        if (A_M_E_A_ == true && once == true)
        {
            once = false;
            Game.GetComponent<ItemCollector>().enabled = false;
            forMap.GetComponent<PlayerTeleporter>().ameateleport = true;
            forMap.GetComponent<PlayerTeleporter>().TeleportTo(A_M_E_A_spawnpoint);
            forMap.GetComponent<MapController>().ChangeFloors(1);
            Game.GetComponent<ItemCollector>().enabled = true;
        }
    }

    /*Changes some start states when the user Changes Device.*/
    public void ChangeStart()
    {
        MbeingAbledToUse = false;
        HbeingAbledToUse = false;
        EbeingAbledToUse = false;
        XbeingAbledToUse = false;
        CbeingAbledToUse = false;
        Game.GetComponent<ItemCollector>().enabled = false;
        player.GetComponent<Movement_Player>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        minimapPanels.SetActive(true);
        minimap.SetActive(false);
        miniFrame.SetActive(false);
        max.SetActive(false);
        hide.SetActive(false);
        show.SetActive(true);
        if (mobileusage == true)
        {
            mobileusage = false;
            joysticks.SetActive(false);
            buttonOpenMenu.SetActive(false);

            if (!buttonOpenMinimap.activeSelf)
            {
                buttonMaxMinimap.SetActive(false);
                buttonHideMinimap.SetActive(false);
            }
            else
            {
                buttonOpenMinimap.SetActive(false);
            }

        }
        ChangeDevice.SetActive(false);
        rw2.SetActive(true);
    }

    public void A_M_E_A_access()
    {
        if (A_M_E_A_ == false)
        {
            A_M_E_A_ = true;
        }
        else
        {
            A_M_E_A_ = false;
        }
    }

    /*This function handles the keyboard inputs of a user*/
    private void HandleInput()
    {
        if (mobileusage == false)
        {
            // Skip game input handling if a UI element is focused (e.g., typing in chat input field)
            // This prevents E, H, M, etc. from triggering game actions while the user is typing
            if (UnityEngine.EventSystems.EventSystem.current != null && 
                UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }
            
            //If a user presses the letter 'E' on their keyboard
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Check for NPC interaction first (takes priority over menu)
                if (NPCInteractionEnabled && npcDetector != null && npcDetector.CanInteract())
                {
                    //NPC interaction is available, let NPCInteractionDetector handle it
                    npcDetector.OnInteract();
                    return; //Don't proceed to menu toggle
                }
                
                //If no NPC interaction, proceed with original menu toggle
                //and if they are allowed to do so [based on what they're doing at the moment(EbeingAbledToUse)], the following condition is met
                if (EbeingAbledToUse == true)
                {
                    //The script handling the MiniGame is turned off as it causes problems when the menu is open
                    Game.GetComponent<ItemCollector>().enabled = false;

                    //And the function 'ToggleNavigationMode()' handling the opening of the menu of functionalities is called
                    ToggleNavigationMode();
                }
            }

            if (Input.GetKeyDown(KeyCode.C) && (CbeingAbledToUse == true))
            {
                ChangeStart();
            }

            //If a user presses the letter 'X' on their keyboard, which is the button that cancels a navigation to a destination, 
            //and if they are allowed to do so [based on what they're doing at the moment(XbeingAbledToUse)], the fowllowing condition is met
            if (Input.GetKeyDown(KeyCode.X) && (XbeingAbledToUse == true) /*&& isNavigationMode*/)
            {
                //And the function 'CancelNavigation()' which handles the cancelling of a navigation to a destination is called
                CancelNavigation();
            }

            //If a user presses the letter 'M' on their keyboard, which is the button that either maximizes or minimizes the minimap, 
            //and if they are allowed to do so [based on what they're doing at the moment(MbeingAbledToUse)], the fowllowing condition is met
            if (Input.GetKeyDown(KeyCode.M) && (MbeingAbledToUse == true))
            {

                //The following condition checks whether the minimap is maximized or not. It checks that by the value of 'HbeingAbledToUse' which
                //is handles the hide of the minimap functionality. That functionality is only available when the minimap is minimized.
                //So if 'HbeingAbledToUse' is true, then the minimap is minimized, therefore the action to happen is to maximize it
                if (HbeingAbledToUse == true)
                {

                    //Maximizing the minimap works similar to when the menu is open therefore the MiniGame script is turned off
                    Game.GetComponent<ItemCollector>().enabled = false;

                    //When the minimap is maximized, hiding it is not allowed
                    //Opening the menu is also not allowed
                    HbeingAbledToUse = false;
                    EbeingAbledToUse = false;


                    minimapActive = true;

                    //The panels containing the maximized minimap items are turned on
                    bigMapMiniPanel.SetActive(true);
                    bigMapMarker.SetActive(true);
                    bigMapminiPanelDestinations.SetActive(true);

                    //The panels containing the minimized minimap items are turned off
                    minimap.SetActive(false);
                    miniFrame.SetActive(false);
                    max.SetActive(false);
                    hide.SetActive(false);

                    //The player's movement is turned off and the cursor is now visible and unlocked, same as when the menu is open
                    player.GetComponent<Movement_Player>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                //Opposite to the previous condition, if the minimap is maximized, pressing the letter 'M' minimizes it
                else
                {
                    //The maximized minimaps is now minimized, therefore gameplay continues and the minigame script can start again
                    Game.GetComponent<ItemCollector>().enabled = true;

                    //When the minimap is minimized, hiding it is allowed
                    //Opening the menu is also allowed
                    HbeingAbledToUse = true;
                    EbeingAbledToUse = true;

                    //The player's movement is turned on and the cursor is now invisible and locked, for gameplay purposes
                    player.GetComponent<Movement_Player>().enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;


                    minimapActive = false;


                    //The panels containing the maximized minimap items are turned off
                    bigMapMiniPanel.SetActive(false);
                    bigMapMarker.SetActive(false);
                    bigMapminiPanelDestinations.SetActive(false);

                    //The panels containing the minimized minimap items are turned on
                    minimap.SetActive(true);
                    miniFrame.SetActive(true);
                    max.SetActive(true);
                    hide.SetActive(true);
                }
            }

            //If a user presses the letter 'H' on their keyboard, which is the button that either hides or shows the minimap, 
            //and if they are allowed to do so [based on what they're doing at the moment(HbeingAbledToUse)], the fowllowing condition is met
            if (Input.GetKeyDown(KeyCode.H) && (HbeingAbledToUse == true))
            {
                //If 'MbeingAbledToUse' is true means that the minimap is shown, therefore hiding is also possible.
                //So what will happen is the hiding of the minimap
                if (MbeingAbledToUse == true)
                {
                    //The panels containing the minimap items are turned off
                    minimap.SetActive(false);
                    miniFrame.SetActive(false);
                    max.SetActive(false);

                    //The ability to maximize the minimap is turned off as it's hidden
                    MbeingAbledToUse = false;

                    hide.SetActive(false);

                    //The panel containing the message of how to turn the minimap back on is turned on
                    show.SetActive(true);
                }

                //The code below handles the showing of the minimap
                else
                {
                    //The panels containing the minimap items are turned on
                    minimap.SetActive(true);
                    miniFrame.SetActive(true);
                    max.SetActive(true);

                    //The ability to maximizing is turned on as the minimap is also back on
                    MbeingAbledToUse = true;
                    hide.SetActive(true);

                    //The panel containing the message of how to turn on the minimap is turned off as it's now necessary if it's already open
                    show.SetActive(false);
                }
            }
            if (((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.L)) || ((Input.GetKeyDown(KeyCode.L)) && (LbeingAbledToUse == true)))
            {
                if (lfirstUse == true)
                {
                    lfirstUse = false;
                    CloseMainMenu(Game.GetComponent<ItemCollector>().Llabel);
                }
                ChangeLighting();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C) && (CbeingAbledToUse == true))
            {
                ChangeStart();
            }
        }
    }

    /*Changes the light colors when the user unlocks the feature.*/
    private int mode = 0;
    public void ChangeLighting()
    {
        mode++;
        if (mode == 5)
        {
            mode = 0;
        }
        if (mode == 0)
        {
            if (directionalLight != null)
            {
                // Example: change light color via hex (#RRGGBB or #RRGGBBAA)
                string hexColor = "#E4E4E4"; // orange
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    directionalLight.color = newColor;
                }
            }
            if (skyMaterial != null)
            {
                // Clone the material so we don't overwrite the original asset
                runtimeSkybox = new Material(skyMaterial);

                // Assign this clone as the active skybox
                RenderSettings.skybox = runtimeSkybox;

                // Example: set sky tint from hex
                string hexColor = "#808080";
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    runtimeSkybox.SetColor("_SkyTint", newColor);
                }
            }
        }
        if (mode == 1)
        {
            if (directionalLight != null)
            {
                // Example: change light color via hex (#RRGGBB or #RRGGBBAA)
                string hexColor = "#EB2828"; // orange
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    directionalLight.color = newColor;
                }
            }
            if (skyMaterial != null)
            {
                // Clone the material so we don't overwrite the original asset
                runtimeSkybox = new Material(skyMaterial);

                // Assign this clone as the active skybox
                RenderSettings.skybox = runtimeSkybox;

                // Example: set sky tint from hex
                string hexColor = "#FF0000";
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    runtimeSkybox.SetColor("_SkyTint", newColor);
                    runtimeSkybox.SetFloat("_AtmosphereThickness", 0.39f);
                }
            }
        }
        if (mode == 2)
        {
            if (directionalLight != null)
            {
                // Example: change light color via hex (#RRGGBB or #RRGGBBAA)
                string hexColor = "#FF00C1"; // orange
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    directionalLight.color = newColor;
                }
            }
            if (skyMaterial != null)
            {
                // Clone the material so we don't overwrite the original asset
                runtimeSkybox = new Material(skyMaterial);

                // Assign this clone as the active skybox
                RenderSettings.skybox = runtimeSkybox;

                // Example: set sky tint from hex
                string hexColor = "#F800FF";
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    runtimeSkybox.SetColor("_SkyTint", newColor);
                }
            }
        }
        if (mode == 3)
        {
            if (directionalLight != null)
            {
                // Example: change light color via hex (#RRGGBB or #RRGGBBAA)
                string hexColor = "#E4FF00"; // orange
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    directionalLight.color = newColor;
                }
            }
            if (skyMaterial != null)
            {
                // Clone the material so we don't overwrite the original asset
                runtimeSkybox = new Material(skyMaterial);

                // Assign this clone as the active skybox
                RenderSettings.skybox = runtimeSkybox;

                // Example: set sky tint from hex
                string hexColor = "#0016FF";
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    runtimeSkybox.SetColor("_SkyTint", newColor);
                }
            }
        }
        if (mode == 4)
        {
            if (directionalLight != null)
            {
                // Example: change light color via hex (#RRGGBB or #RRGGBBAA)
                string hexColor = "#04FF00"; // orange
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    directionalLight.color = newColor;
                }
            }
            if (skyMaterial != null)
            {
                // Clone the material so we don't overwrite the original asset
                runtimeSkybox = new Material(skyMaterial);

                // Assign this clone as the active skybox
                RenderSettings.skybox = runtimeSkybox;

                // Example: set sky tint from hex
                string hexColor = "#FF8100";
                Color newColor;
                if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
                {
                    runtimeSkybox.SetColor("_SkyTint", newColor);
                    // Example: set atmosphere thickness to 2.0
                    runtimeSkybox.SetFloat("_AtmosphereThickness", 3.27f);
                }
            }
        }
    }

    /*Function that saves which menu was before the help button was clicked(Start Screen or Main Menu).*/
    private GameObject rp;
    private string helpState = "Not_Started";

    public void Set_Help(GameObject rightpanel)
    {
        rp = rightpanel;
        if (startFlags.activeSelf)
        {
            maindestinationtogglepanel.SetActive(false);
            startFlags.SetActive(false);
            ButtonAppearance.SetActive(false);
            L_M_Label.SetActive(false);
            Music_Icon.SetActive(false);
            Music_Button.SetActive(false);
            helpState = "Start";
        }
        else
        {
            helpState = "Not_Started";
        }
    }

    /*Takes the remembered panel before the HELP Panel was clicked and opens it again.*/
    public void Close_Help()
    {
        rp.SetActive(true);
        if (helpState == "Start")
        {
            maindestinationtogglepanel.SetActive(true);
            startFlags.SetActive(true);
            ButtonAppearance.SetActive(true);
            L_M_Label.SetActive(true);
            Music_Icon.SetActive(true);
            Music_Button.SetActive(true);
        }
        else
        {
            Flags.SetActive(true);
        }
    }

    /*Pressing 'E' done for Mobile.*/
    public void do_E()
    {
        //The script handling the MiniGame is turned off as it causes problems when the menu is open
        Game.GetComponent<ItemCollector>().enabled = false;

        //And the function 'ToggleNavigationMode()' handling the opening of the menu of functionalities is called
        ToggleNavigationMode();
    }

    /*Pressing 'X' done for Mobile.*/
    public void do_X()
    {
        //And the function 'CancelNavigation()' which handles the cancelling of a navigation to a destination is called
        CancelNavigation();
    }

    /*Pressing 'L' done for Mobile.*/
    public void do_L()
    {
        if (lfirstUse == true)
        {
            lfirstUse = false;
            CloseMainMenu(Game.GetComponent<ItemCollector>().Llabel);
        }
        ChangeLighting();
    }

    /*Pressing 'H' done for Mobile.*/
    public void do_H()
    {
        if (!minimap.activeSelf)
        {
            minimap.SetActive(true);
            miniFrame.SetActive(true);
        }
        else
        {
            minimap.SetActive(false);
            miniFrame.SetActive(false);
        }
    }

    /*The function below is called when all the collectibles are collected and a "secret" navigation path is started*/
    public void CallToggelNavigation()
    {
        Game.GetComponent<ItemCollector>().enabled = false;
        EbeingAbledToUse = false;
        tempM = MbeingAbledToUse;
        MbeingAbledToUse = false;
        tempH = HbeingAbledToUse;
        HbeingAbledToUse = false;
        minimapPanels.SetActive(false);
        if (mobileusage == true)
        {
            buttonOpenMenu.SetActive(false);
        }
        else
        {
            promptLabel.gameObject.SetActive(false);
        }
    }



    /*The function below handels the opening of the main menu with all the functionalities*/
    private void ToggleNavigationMode()
    {
        //Since the menu doesn't also close with the letter 'E' pressed, we turn of the ability of it if it's clicked anyway
        //Clicking it therefore, does nothing
        EbeingAbledToUse = false;


        tempM = MbeingAbledToUse;

        //Since we're on the main menu, maximizing or minimizing the minimap is uncessay since the minimap is disabled, so we 
        //turn off the ability to use it
        MbeingAbledToUse = false;
        tempH = HbeingAbledToUse;


        //Since we're on the main menu, hiding or showing the minimap is uncessay since the minimap is disabled, so we 
        //turn off the ability to use it
        HbeingAbledToUse = false;


        //Since the main menu is now open, the minimap gets disabled, so we disable the panels containing the minimap information
        minimapPanels.SetActive(false);


        //The main menu panel containing the first buttons gets turned on
        MainMenuButtons.gameObject.SetActive(true);

        if (CbeingAbledToUse == true)
        {
            ChangeDevice.SetActive(false);
            CbeingAbledToUse = false;
        }


        if (buttonHideMinimap.activeInHierarchy)
        {
            minimaponMobile = true;
        }
        else
        {
            minimaponMobile = false;
        }

        if (mobileusage == true)
        {
            buttonOpenMenu.SetActive(false);
            buttonOpenMinimap.SetActive(false);
            buttonMaxMinimap.SetActive(false);
            buttonHideMinimap.SetActive(false);
        }
        else
        {
            //The panel saying how to open the main menu is turned off as it's now unnecessary
            promptLabel.gameObject.SetActive(false);
        }

        //Since the main menu has now opened, the character's movement is stopped,
        //the cursor is now visible and locked again for the user to see and click buttons
        player.GetComponent<Movement_Player>().enabled = false;
        if (mobileusage == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            joysticks.SetActive(false);
        }
        Flags.SetActive(true);
    }

    void OnDestroy()
    {
        // Restore original if needed
        if (skyMaterial != null)
        {
            RenderSettings.skybox = skyMaterial;
        }
    }

    public void CancelToggelNavigation()
    {

        EbeingAbledToUse = true;


        MbeingAbledToUse = tempM;


        HbeingAbledToUse = tempH;


        minimapPanels.SetActive(true);


        MainMenuButtons.gameObject.SetActive(false);

        if (CbeingAbledToUse == true)
        {
            ChangeDevice.SetActive(false);
            CbeingAbledToUse = false;
        }

        if (buttonHideMinimap.activeInHierarchy)
        {
            minimaponMobile = true;
        }
        else
        {
            minimaponMobile = false;
        }

        if (mobileusage == true)
        {
            buttonOpenMenu.SetActive(false);
            buttonOpenMinimap.SetActive(false);
            buttonMaxMinimap.SetActive(false);
            buttonHideMinimap.SetActive(false);
        }
        else
        {
            promptLabel.gameObject.SetActive(false);
        }

        player.GetComponent<Movement_Player>().enabled = true;
        if (mobileusage == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            joysticks.SetActive(false);
        }
        Flags.SetActive(false);

    }

    /*The function below is used whenever a 'Close All' or 'Close' button is clicked in the menus
    which closes all menus and get back to gameplay*/
    public void CloseMainMenu(GameObject closed)
    {
        closed.gameObject.SetActive(false);
        if (Tog1switcher == true)
        {
            destinationModeToggle.gameObject.SetActive(true);
            Tog1switcher = false;
        }
        if (Tog2switcher == true)
        {
            togglerpanel.GetComponent<Toggle>().gameObject.SetActive(true);
            Tog2switcher = false;
        }

        if (mobileusage == true)
        {
            buttonOpenMenu.SetActive(true);
            joysticks.SetActive(true);
        }
        else
        {
            //The label saying how to open the main menu is turned back on as the menu is now closed
            promptLabel.gameObject.SetActive(true);
        }


        //The panels containing the minimap items are turned on
        if (mobileusage == false)
        {
            minimapPanels.SetActive(true);
        }
        //tempM and tempH contain the last state of the minimap before the menu was opened.
        //If the minimap was hidden, now that the menu is closing and the minimap would technically appear,
        //it should stay hidden since that was its last state. Similarly, if the minimap was on.
        MbeingAbledToUse = tempM;
        HbeingAbledToUse = tempH;

        if (mobileusage == true)
        {
            if (minimaponMobile == true)
            {
                buttonMaxMinimap.SetActive(true);
                buttonHideMinimap.SetActive(true);
                minimapPanels.SetActive(true);
            }
            else
            {
                buttonOpenMinimap.SetActive(true);
            }
        }

        //The player's movement and the cursor's status are switched to gameplay mode
        player.GetComponent<Movement_Player>().enabled = true;
        if (mobileusage == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // Clear UI selection so game keys work again (prevents buttons staying selected)
            if (UnityEngine.EventSystems.EventSystem.current != null)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }

        //The button 'E', which turnes on the main menu, is available to use again
        EbeingAbledToUse = true;

        //The script containing the MiniGame is turned on since we're now again on gameplay mode
        Game.GetComponent<ItemCollector>().enabled = true;
        Flags.SetActive(false);
    }


    /*The function and private variables below handle the elevator check-boxes and keep them synchronized.
    The program has 2 check-boxes but work as one, so when on is turned on/off, the other should do as well*/
    private int swi1 = 1;
    private int swi2 = 1;

    public void ChangeToggles(int toggle)
    {
        Toggle secondtoggle = togglerpanel.GetComponent<Toggle>();
        TMP_Text t = togglerpanel.transform.Find("Text").GetComponent<TMP_Text>();
        TMP_Text tt = destinationModeToggle.transform.Find("Text").GetComponent<TMP_Text>();

        if (toggle == 1)
        {
            if (swi1 == 1)
            {
                swi2 = 0;
                if (destinationModeToggle.isOn)
                {
                    secondtoggle.isOn = true;
                    t.gameObject.SetActive(false);
                    tt.gameObject.SetActive(false);
                }
                else
                {
                    secondtoggle.isOn = false;
                    t.gameObject.SetActive(true);
                    tt.gameObject.SetActive(true);
                }
            }
            else
            {
                swi1 = 1;
            }
        }
        else
        {
            if (swi2 == 1)
            {
                swi1 = 0;
                if (secondtoggle.isOn)
                {
                    destinationModeToggle.isOn = true;
                    t.gameObject.SetActive(false);
                    tt.gameObject.SetActive(false);
                }
                else
                {
                    destinationModeToggle.isOn = false;
                    t.gameObject.SetActive(true);
                    tt.gameObject.SetActive(true);
                }
            }
            else
            {
                swi2 = 1;
            }
        }
    }


    /*The function below handles one of the 2 elevator check-boxes, the horizontal one. Depending on which menu is open, 
    its position can be different, therefore, the function checks which menu is open and sets the check-box's position at 
    the correct spot*/
    public void ChangeTogglePosition(string type)
    {
        RectTransform rt = destinationModeToggle.GetComponent<RectTransform>();
        Vector2 pos = rt.anchoredPosition;
        if (type == "Normal Menu")
        {
            pos.x = 471;
            pos.y = 283;
            rt.anchoredPosition = pos;
        }
        else if (type == "Level 1 - Main")
        {
            pos.x = 1049.9f;
            pos.y = 317;
            rt.anchoredPosition = pos;
        }
        else if (type == "Main Area main")
        {
            pos.x = 1042;
            pos.y = 17;
            rt.anchoredPosition = pos;
        }
    }

    /*Maximized the mini-map.*/
    public void Maximizing()
    {
        //Maximizing the minimap works similar to when the menu is open therefore the MiniGame script is turned off
        Game.GetComponent<ItemCollector>().enabled = false;

        //When the minimap is maximized, hiding it is not allowed
        //Opening the menu is also not allowed
        HbeingAbledToUse = false;
        EbeingAbledToUse = false;


        minimapActive = true;

        //The panels containing the maximized minimap items are turned on
        bigMapMiniPanel.SetActive(true);
        bigMapMarker.SetActive(true);
        bigMapminiPanelDestinations.SetActive(true);

        //The panels containing the minimized minimap items are turned off
        minimap.SetActive(false);
        miniFrame.SetActive(false);
        if (mobileusage == false)
        {
            max.SetActive(false);
            hide.SetActive(false);
        }

        //The player's movement is turned off and the cursor is now visible and unlocked, same as when the menu is open
        player.GetComponent<Movement_Player>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        if (mobileusage == true)
        {
            joysticks.SetActive(false);
            buttonOpenMenu.SetActive(false);
        }
        else
        {
            Cursor.visible = true;
        }
    }

    /*The function below handles the minimizing of the maximized minimap*/
    public void Minimizing()
    {
        //Since the minimap is not maximized anymore, the abilities of hiding the minimized minimap
        //and opening the main menu are turned back on
        HbeingAbledToUse = true;
        EbeingAbledToUse = true;


        //The movement of the player and the status of the cursor are back on gameplay mode
        player.GetComponent<Movement_Player>().enabled = true;
        if (mobileusage == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = false;


        minimapActive = false;


        //The panels containing the maximized minimap items are turned off
        bigMapMiniPanel.SetActive(false);
        bigMapMarker.SetActive(false);
        bigMapminiPanelDestinations.SetActive(false);


        //The panels containing the minimized minimap items are turned on
        minimap.SetActive(true);
        miniFrame.SetActive(true);
        if (mobileusage == true)
        {
            buttonOpenMenu.SetActive(true);
            buttonHideMinimap.SetActive(true);
            buttonMaxMinimap.SetActive(true);
            joysticks.SetActive(true);
        }
        else
        {
            max.SetActive(true);
            hide.SetActive(true);
        }
    }

    /*The function below handles the setting of a destination target*/
    public void SetTarget(Transform newTarget)
    {
        //The panel containing the menu which will open when the navigation stops is turned off(and already set from another function)
        latestMenu.gameObject.SetActive(false);
        languages = Flags.activeSelf;
        Flags.SetActive(false);
        specialobstacles.SetActive(true);

        if (destinationModeToggle.gameObject.activeSelf)
        {
            Tog1switcher = true;
        }
        destinationModeToggle.gameObject.SetActive(false);
        if (togglerpanel.GetComponent<Toggle>().gameObject.activeSelf)
        {
            Tog2switcher = true;
        }
        togglerpanel.GetComponent<Toggle>().gameObject.SetActive(false);


        MenuminimapPanel.SetActive(false);

        //Since a navigation is started, i.e. , movement of the player, the player's movement and cursor's status are back on 
        //gameplay mode
        player.GetComponent<Movement_Player>().enabled = true;
        if (mobileusage == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            WASD_icon.SetActive(true);
        }
        else
        {
            joysticks.SetActive(true);
        }

        if (minimapActive == true)
        {
            bigMapMiniPanel.SetActive(false);
            bigMapMarker.SetActive(false);
            MbeingAbledToUse = false;
        }


        //The condition below checks whether or not the elevator check-box has been clicked, and whether the path will follow normal
        //route or elevator route
        if (destinationModeToggle.isOn)
        {
            //If the user has checked using elevators, the function 'HandleNewDestinationBehavior()' is called to handle the 
            //elevator navigation
            HandleNewDestinationBehavior(newTarget);
        }

        //Else, if elevators aren't checked, the path calculates normal shortest route, including stairs if necessary
        else
        {
            //This condition checks if we're on the MiniGame navigation after collecting all pieces
            if (newTarget.name == "Easter Egg")
            {
                target = newTarget;
                if (mobileusage == true)
                {
                    closenavigationbutton.SetActive(true);
                }
                else
                {
                    cancelLabel.gameObject.SetActive(true);
                }
                XbeingAbledToUse = true;
            }
            //This condition checks the player's coordinates to see in which of the 2 possible routes of going to the library 
            //the players should go as it's a distinct case
            else if (newTarget.parent.gameObject.name == "Bibliothika")
            {
                if (player.transform.position.y >= 3)
                {
                    if (player.transform.position.z < 134 && player.transform.position.x > 141)
                    {
                        target = BibliotekaTargets[0].transform;
                    }
                    else
                    {
                        target = BibliotekaTargets[1].transform;
                    }
                }
                else
                {
                    target = BibliotekaTargets[1].transform;
                }
            }

            //Else, the global varibale target is set to the target given with the function,(instead of the value null), 
            //and now the 'Update()' function will see that the target isn't null and start the navigation the way it's
            //described there
            else
            {
                target = newTarget;
            }
        }

        //The label saying how to cancel/stop the navigation is turned on, and the ability to do it with the letter 'X' is allowed
        if (mobileusage == true)
        {
            closenavigationbutton.SetActive(true);
        }
        else
        {
            cancelLabel.gameObject.SetActive(true);
        }

        XbeingAbledToUse = true;
    }


    /*The function below handles the destination paths with the usage of elevator/s based on the player's and target's positions*/
    private void HandleNewDestinationBehavior(Transform newTarget)
    {
        Barriers.SetActive(true);

        // Retrieve player and target positions
        Vector3 playerPosition = player.transform.position;
        Vector3 targetPosition = newTarget.position;
        TextMeshProUGUI buttonText = newTarget.GetComponentInChildren<TextMeshProUGUI>();
        MapController image = forMap.GetComponent<MapController>();
        Colliders.SetActive(true);
        if (playerPosition.y < 3.2)
        {
            StairwayColliders[3].SetActive(true);
            StairwayColliders[4].SetActive(true);
            StairwayColliders[5].SetActive(true);
            StairwayColliders[6].SetActive(true);
            StairwayColliders[7].SetActive(true);
            StairwayColliders[8].SetActive(true);
            StairwayColliders[9].SetActive(true);
            STC = 1;
        }
        else if (playerPosition.y <= 6.2)
        {
            StairwayColliders[0].SetActive(true);
            StairwayColliders[1].SetActive(true);
            StairwayColliders[2].SetActive(true);
            StairwayColliders[7].SetActive(true);
            StairwayColliders[8].SetActive(true);
            StairwayColliders[9].SetActive(true);
            STC = 2;
        }
        else
        {
            StairwayColliders[0].SetActive(true);
            StairwayColliders[1].SetActive(true);
            StairwayColliders[2].SetActive(true);
            StairwayColliders[3].SetActive(true);
            StairwayColliders[4].SetActive(true);
            StairwayColliders[5].SetActive(true);
            StairwayColliders[6].SetActive(true);
            STC = 3;
        }


        /*Basic Rooms*/

        /*/*Floor 1*/


        /*1*/   //Going from FLOOR 1 to E Rooms Floor 2 - Front Half of building -  using Elevator E1 Floor 1
        if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y < 3) && (playerPosition.y < 2.3) && (playerPosition.z < 134))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*2*/   //Going from FLOOR 1 to E Rooms Floor 2 - Baack Half of building -  using Elevator E2 Floor 1
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y < 3) && (playerPosition.z >= 134))
        {
            target = potentialDestinations[5];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*3*/   //Going from FLOOR 1 to B Rooms Floor 2 using Elevator E2 Floor 1
        else if ((newTarget.parent.gameObject.name[0] == 'B') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y < 3) && (playerPosition.y < 2.3))
        {
            target = potentialDestinations[5];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*4*/   //Going from FLOOR 1 to H Rooms Floor 2 using Elevator E1 Floor 1
        else if ((newTarget.parent.gameObject.name[0] == 'H') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y < 3) && (playerPosition.y < 2.3))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*5*/   //Going from FLOOR 1 to K Rooms Floor 2 using Elevator E1 Floor 1
        else if ((newTarget.parent.gameObject.name[0] == 'K') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y < 3) && (playerPosition.y < 2.3))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*6*/   //Going from FLOOR 1 to E Rooms Floor 3 - Front Half of building -  using Elevator E1 Floor 1
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y < 3) && (playerPosition.z < 134))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 2 floors...\n\nΠάμε πάνω 2 ορόφους...";
            floorchangerelevator = 3;
        }

        /*7*/   //Going from FLOOR 1 to E Rooms Floor 3 - Back Half of building -  using Elevator E2 Floor 1
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y < 3) && (playerPosition.z >= 134))
        {
            target = potentialDestinations[5];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E2";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 2 floors...\n\nΠάμε πάνω 2 ορόφους...";
            floorchangerelevator = 3;
        }

        /*8*/   //Going from FLOOR 1 to B Rooms Floor 3 using Elevator E2 Floor 1
        else if ((newTarget.parent.gameObject.name[0] == 'B') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y < 3))
        {
            target = potentialDestinations[5];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E2";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 2 floors...\n\nΠάμε πάνω 2 ορόφους...";
            floorchangerelevator = 3;
        }

        /*9*/   //Going from FLOOR 1 to H Rooms Floor 3 using Elevator E1 Floor 1
        else if ((newTarget.parent.gameObject.name[0] == 'H') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y < 3))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 2 floors...\n\nΠάμε πάνω 2 ορόφους...";
            floorchangerelevator = 3;
        }

        /*10*/   //Going from FLOOR 1 to K Rooms Floor 3 using Elevator E1 Floor 1
        else if ((newTarget.parent.gameObject.name[0] == 'K') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y < 3))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 2 floors...\n\nΠάμε πάνω 2 ορόφους...";
            floorchangerelevator = 3;
        }


        /*/*Floor 2*/

        /*11*/   //Going from FLOOR 2 to E Rooms Floor 1 - Front Half of building -  using Elevator E1 Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '1') && ((playerPosition.x < 190 && playerPosition.z < 134) || (playerPosition.x >= 190 && playerPosition.z < 127)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[3];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*12*/   //Going from FLOOR 2 to E Rooms Floor 1 - Back Half of building -  using Elevator E2 Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '1') && (playerPosition.x < 190) && (playerPosition.z >= 134) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*13*/   //Going from FLOOR 2 to B Rooms Floor 1 - Left Half of building -  using Elevator E2 Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'B') && (newTarget.parent.gameObject.name[1] == '1') && (playerPosition.y >= 3) && (playerPosition.y < 7.2) && (playerPosition.x < 152))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*14*/   //Going from FLOOR 2 to B Rooms Floor 1 - Right Half of building -  using Elevator ANAGNOSTIRIO Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'B') && (newTarget.parent.gameObject.name[1] == '1') && ((playerPosition.x < 190 && playerPosition.x >= 152) || (playerPosition.x >= 190 && playerPosition.z < 127)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2)) //&& (playerPosition.z < 103)
        {
            target = potentialDestinations[1];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 Anagnostiria";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*15*/   //Going from FLOOR 2 to H Rooms Floor 1 using Elevator E1 Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'H') && (newTarget.parent.gameObject.name[1] == '1') && ((playerPosition.x < 190) || (playerPosition.x >= 190 && playerPosition.z < 127)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[3];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*16*/   //Going from FLOOR 2 to K Rooms Floor 1 using Elevator E1 Floor 2
        else if ((newTarget.parent.gameObject.name == "K1") && ((playerPosition.x < 190) || (playerPosition.x >= 190 && playerPosition.z < 127)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[3];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*17*/   //Going from FLOOR 2 to A Rooms Floor 1 using Elevator ANAGNOSTIRIO Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'A') && (newTarget.parent.gameObject.name[1] == '1') && ((playerPosition.x < 190) || (playerPosition.x >= 190 && playerPosition.z < 132)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[1];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 Anagnostiria";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*18*/   //Going from FLOOR 2 to E Rooms Floor 3 - Front Half of building -  using Elevator E1 Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y >= 3) && (playerPosition.y < 7.2) && (playerPosition.z < 134))
        {
            target = potentialDestinations[3];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 3;
        }

        /*19*/   //Going from FLOOR 2 to E Rooms Floor 3 - Back Half of building -  using Elevator E2 Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y >= 3) && (playerPosition.y < 7.2) && (playerPosition.z >= 134))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 3;
        }

        /*20*/   //Going from FLOOR 2 to B Rooms Floor 3 using Elevator E2 Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'B') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 3;
        }

        /*21*/   //Going from FLOOR 2 to H Rooms Floor 3 using Elevator E1 Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'H') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[3];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 3;
        }

        /*22*/   //Going from FLOOR 2 to K Rooms Floor 3 using Elevator E1 Floor 2
        else if ((newTarget.parent.gameObject.name[0] == 'K') && (newTarget.parent.gameObject.name[1] == '3') && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[3];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 3;
        }

        /*/*Floor 3*/

        /*23*/   //Going from FLOOR 3 to E Rooms Floor 1 - Front Half of building -  using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '1') && (playerPosition.y >= 7.2) && (playerPosition.z < 134))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }

        /*24*/   //Going from FLOOR 3 to E Rooms Floor 1 - Back Half of building -  using Elevator E2 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '1') && (playerPosition.y >= 7.2) && (playerPosition.z >= 134))
        {
            target = potentialDestinations[7];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }

        /*25*/   //Going from FLOOR 3 to B Rooms Floor 1 using Elevator E2 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'B') && (newTarget.parent.gameObject.name[1] == '1') && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[7];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }

        /*26*/   //Going from FLOOR 3 to H Rooms Floor 1 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'H') && (newTarget.parent.gameObject.name[1] == '1') && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }

        /*27*/   //Going from FLOOR 3 to K Rooms Floor 1 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name == "K1") && /*(playerPosition.y >= 3) &&*/ (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }

        /*28*/   //Going from FLOOR 3 to A Rooms Floor 1 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'A') && (newTarget.parent.gameObject.name[1] == '1') && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }

        /*29*/   //Going from FLOOR 3 to E Rooms Floor 2 - Front Half of building -  using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y >= 7.2) && (playerPosition.z < 134))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*30*/   //Going from FLOOR 3 to E Rooms Floor 2 - Back Half of building -  using Elevator E2 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'E') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y >= 7.2) && (playerPosition.z >= 134))
        {
            target = potentialDestinations[7];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E2";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*31*/   //Going from FLOOR 3 to B Rooms Floor 2 using Elevator E2 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'B') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[7];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E2";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*32*/   //Going from FLOOR 3 to H Rooms Floor 2 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'H') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*33*/   //Going from FLOOR 3 to K Rooms Floor 2 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name[0] == 'K') && (newTarget.parent.gameObject.name[1] == '2') && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }


        /*Auditoriums & Study Rooms*/

        /*/*Floor 1*/


        /*34*/   //Going from FLOOR 1 to Amfitheatro SO Top Enterance Floor 2 - Right Half of building -  using Elevator A Floor 1
        else if ((newTarget.parent.gameObject.name == "AMFSO") && (playerPosition.y < 3) && (playerPosition.x >= 166) && (playerPosition.x <= 190))
        {
            target = potentialDestinations[0];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 A";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*35*/   //Going from FLOOR 1 to Amfitheatro SO Top Enterance Floor 2 - Left Half of building -  using Elevator E1 Floor 1
        else if ((newTarget.parent.gameObject.name == "AMFSO") && (playerPosition.y < 3) && (playerPosition.y < 2.3) && (playerPosition.x < 166))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }


        /*36*/   //Going from FLOOR 1 to Amfitheatro A Top Enterance Floor 2 - Right Half of building -  using Elevator A Floor 1
        else if ((newTarget.parent.gameObject.name == "AMFA") && (playerPosition.y < 3) && (playerPosition.x >= 166) && (playerPosition.x <= 190))
        {
            target = potentialDestinations[0];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 A";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*37*/   //Going from FLOOR 1 to Amfitheatro A Top Enterance Floor 2 - Left Half of building -  using Elevator E1 Floor 1
        else if ((newTarget.parent.gameObject.name == "AMFA") && (playerPosition.y < 3) && (playerPosition.y < 2.3) && (playerPosition.x < 166))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }


        /*38*/   //Going from FLOOR 1 to Study Rooms Floor 2 - Right Half of building -  using Elevator A Floor 1
        else if ((newTarget.parent.gameObject.name == "Anagnostiria") && (playerPosition.y < 3) && (playerPosition.x >= 166) && (playerPosition.x <= 190))
        {
            target = potentialDestinations[0];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 A";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*39*/   //Going from FLOOR 1 to Study Rooms Floor 2 - Left Half of building -  using Elevator E1 Floor 1
        else if ((newTarget.parent.gameObject.name == "Anagnostiria") && (playerPosition.y < 3) && (playerPosition.y < 2.3) && (playerPosition.x < 166))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }


        /*/*Floor 2*/


        /*40*/   //Going from FLOOR 2 to Amfitheatro SO Bottom Enterance Floor 1 - Right Half of building -  using Elevator ANAGNOSTIRIO Floor 2
        else if ((newTarget.parent.gameObject.name == "AMFSO_DOWN") && (((playerPosition.x >= 152) && (playerPosition.x < 190)) || (playerPosition.x >= 190 && playerPosition.z < 127)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[1];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 Anagnostiria";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*41*/   //Going from FLOOR 2 to Amfitheatro SO Bottom Enterance Floor 1 - Left Half of building -  using Elevator E1 Floor 2
        else if ((newTarget.parent.gameObject.name == "AMFSO_DOWN") && (playerPosition.y >= 3) && (playerPosition.y < 7.2) && (playerPosition.x < 152))
        {
            target = potentialDestinations[3];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }


        /*42*/   //Going from FLOOR 2 to Amfitheatro A Bottom Enterance Floor 1 - Right Half of building -  using Elevator ANAGNOSTIRIO Floor 2
        else if ((newTarget.parent.gameObject.name == "AMFA_DOWN") && (((playerPosition.x >= 152) && (playerPosition.x < 190)) || (playerPosition.x >= 190 && playerPosition.z < 127)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[1];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 Anagnostiria";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }


        /*43*/   //Going from FLOOR 2 to Amfitheatro A Bottom Enterance Floor 1 - Left Half of building -  using Elevator E1 Floor 2
        else if ((newTarget.parent.gameObject.name == "AMFA_DOWN") && (playerPosition.y >= 3) && (playerPosition.y < 7.2) && (playerPosition.x < 152))
        {
            target = potentialDestinations[3];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*/*Floor 3*/

        /*44*/   //Going from FLOOR 3 to Amfitheatro SO Top Enterance Floor 2 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name == "AMFSO") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }


        /*45*/   //Going from FLOOR 3 to Amfitheatro SO Bottom Enterance Floor 1 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name == "AMFSO_DOWN") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }



        /*46*/   //Going from FLOOR 3 to Amfitheatro A Top Enterance Floor 2 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name == "AMFA") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }


        /*47*/   //Going from FLOOR 3 to Amfitheatro A Bottom Enterance Floor 1 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name == "AMFA_DOWN") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }


        /*48*/   //Going from FLOOR 3 to Study Rooms Floor 2 using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name == "Anagnostiria") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }


        /*(Leshi) Student Center & Restaurant*/

        /*/*Floor 1*/

        /*49*/   //Going from FLOOR 1 to Leshi - Right Half of building -  using Elevator A Floor 1
        else if ((newTarget.parent.gameObject.name == "Leshi") && (playerPosition.y < 3) && (playerPosition.x >= 166) && (playerPosition.x < 190.5))
        {
            target = potentialDestinations[0];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 A";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }

        /*50*/   //Going from FLOOR 1 to Leshi - Middle Part of building -  using Elevator E1 Floor 1
        else if ((newTarget.parent.gameObject.name == "Leshi") && (playerPosition.y < 3) && (playerPosition.x >= 107) && (playerPosition.x < 166) && (playerPosition.z >= 119))
        {
            target = potentialDestinations[2];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor1 E1";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going up 1 floor...\n\nΠάμε πάνω 1 όροφο...";
            floorchangerelevator = 2;
        }


        /*/*Floor 3*/

        /*51*/   //Going from FLOOR 3 to Leshi using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name == "Leshi") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }


        /*(Prytaneia)Deanery*/

        /*/*Floor 2*/

        /*52*/   //Going from FLOOR 2 to Prytaneia - Left Half && Front-Right Half of building -  using Elevator E2 Floor 2
        else if ((newTarget.parent.gameObject.name == "Prytaneia") && ((playerPosition.x < 166) || (playerPosition.x >= 166 && playerPosition.z > 91.2 && playerPosition.z < 152.7)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*/*Floor 3*/

        /*53*/   //Going from FLOOR 3 to Prytaneia using Elevator E2 Floor 3
        else if ((newTarget.parent.gameObject.name == "Prytaneia") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[7];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }


        /*Bus Stop*/

        /*/*Floor 2*/

        /*54*/   //Going from FLOOR 2 to Bus Stop - Left Half && Front-Right Half of building -  using Elevator E2 Floor 2
        else if ((newTarget.parent.gameObject.name == "Bus") && ((playerPosition.x < 166) || (playerPosition.x >= 166 && playerPosition.z > 91.2 && playerPosition.z < 152.7)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }


        /*/*Floor 3*/


        /*55*/   //Going from FLOOR 3 to Bus Stop using Elevator E2 Floor 3
        else if ((newTarget.parent.gameObject.name == "Bus") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[7];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floors...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 1;
        }


        /*(Biblioteka)Library*/

        /*/*Floor 1*/


        /*56*/   //Going from FLOOR 1 to Bibliothiki walking to Library position 2
        else if ((newTarget.parent.gameObject.name == "Bibliothika") && (playerPosition.y < 3))// && (playerPosition.z >= 134) && (playerPosition.x < 189))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            target = BibliotekaTargets[1].transform;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }


        /*/*Floor 2*/

        /*57*/   //Going from FLOOR 2 to Bibliothiki - Back Left Half of building -  walking to Library position 2
        else if ((newTarget.parent.gameObject.name == "Bibliothika") && (playerPosition.x >= 190) && (playerPosition.z > 127) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))// && (playerPosition.z >= 134) && (playerPosition.x < 189))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            target = BibliotekaTargets[1].transform;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*58*/   //Going from FLOOR 2 to Bibliothiki - Front Right Half of building -  walking to Library position 1
        else if ((newTarget.parent.gameObject.name == "Bibliothika") && (playerPosition.x >= 190) && (playerPosition.z <= 127) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))// && (playerPosition.z >= 134) && (playerPosition.x < 189))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            target = BibliotekaTargets[0].transform;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*59*/   //Going from FLOOR 2 to Bibliothiki - Front Half of building -  walking to Library poaition 1
        else if ((newTarget.parent.gameObject.name == "Bibliothika") && (playerPosition.x < 190) && (playerPosition.z < 122) && (playerPosition.z < 152.73) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))// && (playerPosition.z >= 134) && (playerPosition.x < 189))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            target = BibliotekaTargets[0].transform;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*60*/   //Going from FLOOR 2 to Bibliothiki - Back Half of building -  using Elevator E2 Floor 2
        else if ((newTarget.parent.gameObject.name == "Bibliothika") && (playerPosition.x < 190) && (playerPosition.z >= 122) && (playerPosition.z < 152.73) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))// && (playerPosition.z >= 134) && (playerPosition.x < 189))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            elevatorFinalTarget = BibliotekaTargets[1].transform;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }

        /*/*Floor 3*/

        /*61*/   //Going from FLOOR 3 to Bibliothiki - Back Half of building -  using Elevator E2 Floor 3
        else if ((newTarget.parent.gameObject.name == "Bibliothika") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[7];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E2";
            level = 0;
            elevatorFinalTarget = BibliotekaTargets[1].transform;
            axis = "Going down 2 floor...\n\nΠάμε κάτω 2 όροφο...";
            floorchangerelevator = 2;
        }

        /*No Floor*/

        /*62*/   //Going from FLOOR 2 to Bibliothiki - Back Half of building -  walking to Library poaition 1
        else if ((newTarget.parent.gameObject.name == "Bibliothika") && (playerPosition.x >= 216))// && (playerPosition.z >= 134) && (playerPosition.x < 189))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            target = BibliotekaTargets[1].transform;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }


        /*(Front)Main Parking*/


        /*/*Floor3*/


        /*63*/   //Going from FLOOR 3 to Front Parking using Elevator E1 Floor 3
        else if ((newTarget.parent.gameObject.name == "Front Parking") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[4];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E1";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 ορόφους...";
            floorchangerelevator = 2;
        }


        /*Side Parking*/


        /*/*Floor 2*/


        /*64*/   //Going from FLOOR 2 to Side Parking using Elevator E2 Floor 2
        else if ((newTarget.parent.gameObject.name == "Side Parking") && ((playerPosition.x < 166) || (playerPosition.x >= 166 && playerPosition.z > 91.2 && playerPosition.z < 152.7)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }


        /*/*Floor 3*/


        /*65*/   //Going from FLOOR 3 to Side Parking using Elevator E2 Floor 3
        else if ((newTarget.parent.gameObject.name == "Side Parking") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[7];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 2 floor...\n\nΠάμε κάτω 2 ορόφους...";
            floorchangerelevator = 2;
        }


        /*Cafeteria Parking*/


        /*/*Floor 2*/


        /*66*/   //Going from FLOOR 2 to Cafeteria Parking using Elevator E2 Floor 2
        else if ((newTarget.parent.gameObject.name == "Cafeteria Parking") && ((playerPosition.x < 190) || (playerPosition.x >= 190 && playerPosition.z < 127)) && (playerPosition.y >= 3) && (playerPosition.y < 7.2))
        {
            target = potentialDestinations[6];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor2 E2";
            level = 0;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 1;
        }


        /*/*Floor 3*/


        /*67*/   //Going from FLOOR 3 to Cafeteria Parking using Elevator E2 Floor 3
        else if ((newTarget.parent.gameObject.name == "Cafeteria Parking") && (playerPosition.y >= 7.2))
        {
            target = potentialDestinations[7];
            if (mobileusage == true)
            {
                closenavigationbutton.SetActive(true);
            }
            else
            {
                cancelLabel.gameObject.SetActive(true);
            }
            elevatorUse = true;
            elevatorToBeUsed = "Elevator_Floor3 E2";
            level = 1;
            elevatorFinalTarget = newTarget;
            axis = "Going down 1 floor...\n\nΠάμε κάτω 1 όροφο...";
            floorchangerelevator = 2;
        }
        //If any different case, normal navigation without elevator(most likely elevator isn't needed)
        else
        {
            // Default: just set the new target normally
            target = newTarget;
        }
        if (mobileusage == true)
        {
            closenavigationbutton.SetActive(true);
        }
        else
        {
            cancelLabel.gameObject.SetActive(true);
        }
        XbeingAbledToUse = true;
    }

    private void SetNewTargetFromArray()
    {
        // Example logic for setting a new target from the potentialDestinations array
        if (potentialDestinations.Length > 0)
        {
            // Choose a random destination (or apply your own selection logic)
            int randomIndex = UnityEngine.Random.Range(0, potentialDestinations.Length);
            target = potentialDestinations[randomIndex];
            // UI changes after selecting destination
            // destinationButtonsPanel.gameObject.SetActive(false);
            cancelLabel.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No potential destinations available.");
        }
    }

    /*The 2 functions below help the program to remember which panel was last opened so there is a correct and smooth menu navigation*/
    public void setWinterSummer(GameObject iss)
    {
        forwintersummer = iss;
    }

    public void getWinterSummer()
    {
        forwintersummer.SetActive(true);
    }

    /*This function sets the last opened menu panel when the user selects a destination to navigate to and it need to be remembered
    to be opened once the navigation gets cancelled*/
    public void SetLatestMenu(GameObject Menu)
    {
        latestMenu = Menu;
    }

    /*This function handles the cancelation of a destination navigation*/
    public void CancelNavigation()
    {
        Barriers.SetActive(false);
        Colliders.SetActive(false);
        specialobstacles.SetActive(false);
        //Since the navigation is canceled, the ability to cancel one is disabled
        XbeingAbledToUse = false;

        //After the cancelation, the user returns back to the menu, so their movement is turned disabled and the navmesh agent is reset
        player.GetComponent<Movement_Player>().enabled = false;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        //The elevators are turned off until a new destination is picked and the target is turned to null so the update doesnt one
        //and start a new navigation
        elevatorUse = false;
        target = null;

        //The latest menu and items which were open before the navigation started is opened again
        latestMenu.gameObject.SetActive(true);
        MenuminimapPanel.SetActive(true);
        if (STC != 0)
        {
            if (STC == 1)
            {
                StairwayColliders[3].SetActive(false);
                StairwayColliders[4].SetActive(false);
                StairwayColliders[5].SetActive(false);
                StairwayColliders[6].SetActive(false);
                StairwayColliders[7].SetActive(false);
                StairwayColliders[8].SetActive(false);
                StairwayColliders[9].SetActive(false);
            }
            else if (STC == 2)
            {
                StairwayColliders[0].SetActive(false);
                StairwayColliders[1].SetActive(false);
                StairwayColliders[2].SetActive(false);
                StairwayColliders[7].SetActive(false);
                StairwayColliders[8].SetActive(false);
                StairwayColliders[9].SetActive(false);
            }
            else
            {
                StairwayColliders[0].SetActive(false);
                StairwayColliders[1].SetActive(false);
                StairwayColliders[2].SetActive(false);
                StairwayColliders[3].SetActive(false);
                StairwayColliders[4].SetActive(false);
                StairwayColliders[5].SetActive(false);
                StairwayColliders[6].SetActive(false);
            }
            STC = 0;
        }
        if (Tog1switcher == true)
        {
            destinationModeToggle.gameObject.SetActive(true);
            Tog1switcher = false;
        }
        if (Tog2switcher == true)
        {
            togglerpanel.GetComponent<Toggle>().gameObject.SetActive(true);
            Tog2switcher = false;
        }
        WASD_icon.SetActive(false);

        if (mobileusage == false)
        {
            cancelLabel.gameObject.SetActive(false);
        }
        else
        {
            closenavigationbutton.SetActive(false);
        }

        if (mobileusage == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            joysticks.SetActive(false);
        }

        if (minimapActive == true)
        {
            bigMapMiniPanel.SetActive(true);
            bigMapMarker.SetActive(true);
            MbeingAbledToUse = true;
        }
        // Go back to destination selection mode
        Flags.SetActive(languages);
    }

    /*This function handles the drawing of the path to the destination selected*/
    private void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) return;

        lineRenderer.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]);
        }
    }
}
