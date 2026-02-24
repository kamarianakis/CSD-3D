/*This script handles NPC interaction detection, similar to ItemCollector pattern.
 * It detects when the player is looking at an NPC and shows an interaction prompt.
 * When the player presses Q, it triggers the NPC interaction.*/
using UnityEngine;
using TMPro;

public class NPCInteractionDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("Camera used for raycast detection (usually player's camera)")]
    public Camera playerCamera;
    
    [Tooltip("Layer mask for NPCs - create a new 'NPC' layer in Unity and assign NPCs to it")]
    public LayerMask npcLayer;
    
    [Tooltip("Maximum distance for NPC detection")]
    public float detectionRange = 5f;
    
    [Header("UI References")]
    [Tooltip("GameObject containing the 'Press Q to Talk' prompt UI")]
    public GameObject interactionPromptUI;
    
    [Tooltip("Text component for the prompt (optional, if you want to customize the text)")]
    public TMP_Text promptText;
    
    [Header("Debug")]
    [Tooltip("Show debug rays in Scene view")]
    public bool showDebugRay = false;
    
    // Current NPC being detected
    private NPCController currentNPC = null;
    
    // State management (similar to WaypointSystem's EbeingAbledToUse pattern)
    private bool isInteracting = false;
    private bool QbeingAbledToUse = true; // Flag to control if Q key can be pressed
    
    void Start()
    {
        // If camera not assigned, try to find main camera
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogWarning("NPCInteractionDetector: No camera assigned and no Main Camera found!");
            }
        }
        
        // Hide prompt UI at start
        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(false);
        }
    }
    
    void Update()
    {
        // Don't detect if already interacting (chat is open)
        if (isInteracting)
        {
            return;
        }
        
        // Don't detect if camera is not available
        if (playerCamera == null)
        {
            return;
        }
        
        // Raycast detection (same pattern as ItemCollector.cs)
        // Cast from center of screen (crosshair position)
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;
        
        // Visual debug ray (only in Scene view)
        if (showDebugRay)
        {
            Debug.DrawRay(ray.origin, ray.direction * detectionRange, Color.green);
        }
        
        // Check if raycast hits an NPC
        if (Physics.Raycast(ray, out hit, detectionRange, npcLayer))
        {
            // Try to get NPCController component from hit object
            NPCController npc = hit.collider.GetComponent<NPCController>();
            
            if (npc != null)
            {
                // Found a valid NPC
                if (currentNPC != npc)
                {
                    // New NPC detected
                    currentNPC = npc;
                    Debug.Log("NPCInteractionDetector: NPC detected - " + npc.GetNPCName());
                }
                
                // Show interaction prompt
                ShowInteractionPrompt(hit.point);
                
                // Check for Q key press to interact (only if allowed - same pattern as E key menu)
                if (Input.GetKeyDown(KeyCode.Q) && QbeingAbledToUse)
                {
                    Debug.Log("NPCInteractionDetector: Q key pressed, starting interaction");
                    OnInteract();
                }
            }
            else
            {
                // Hit something on NPC layer but no NPCController component
                // This might be a child object, try to find parent
                NPCController parentNPC = hit.collider.GetComponentInParent<NPCController>();
                if (parentNPC != null)
                {
                    currentNPC = parentNPC;
                    ShowInteractionPrompt(hit.point);
                    
                    if (Input.GetKeyDown(KeyCode.Q) && QbeingAbledToUse)
                    {
                        OnInteract();
                    }
                }
                else
                {
                    // No valid NPC found
                    HideInteractionPrompt();
                    currentNPC = null;
                }
            }
        }
        else
        {
            // No NPC in sight
            HideInteractionPrompt();
            currentNPC = null;
        }
    }
    
    /// <summary>
    /// Shows the interaction prompt UI at the specified world position
    /// </summary>
    void ShowInteractionPrompt(Vector3 worldPosition)
    {
        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(true);
            
            // Position the prompt above the hit point (optional - can be screen-space UI instead)
            // Uncomment if you want world-space positioning:
            // interactionPromptUI.transform.position = worldPosition + Vector3.up * 0.5f;
        }
    }
    
    /// <summary>
    /// Hides the interaction prompt UI
    /// </summary>
    public void HideInteractionPrompt()
    {
        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(false);
        }
    }
    
    /// <summary>
    /// Checks if player can interact with an NPC right now
    /// </summary>
    public bool CanInteract()
    {
        return currentNPC != null && !isInteracting;
    }
    
    /// <summary>
    /// Called when player presses Q to interact with NPC
    /// </summary>
    public void OnInteract()
    {
        if (currentNPC != null && !isInteracting && QbeingAbledToUse)
        {
            Debug.Log("NPCInteractionDetector: OnInteract called with NPC - " + currentNPC.GetNPCName());
            
            // Disable Q key (same pattern as E key menu)
            QbeingAbledToUse = false;
            isInteracting = true;
            HideInteractionPrompt();
            
            // Notify the NPC to start interaction
            currentNPC.StartInteraction();
        }
        else
        {
            Debug.LogWarning("NPCInteractionDetector: OnInteract called but conditions not met!");
            Debug.LogWarning($"currentNPC: {(currentNPC != null ? currentNPC.GetNPCName() : "null")}, isInteracting: {isInteracting}, QbeingAbledToUse: {QbeingAbledToUse}");
        }
    }
    
    /// <summary>
    /// Called when interaction ends (chat closes)
    /// </summary>
    public void OnInteractionEnd()
    {
        Debug.Log("NPCInteractionDetector: OnInteractionEnd called - resetting interaction state");
        
        // Re-enable Q key interaction (same pattern as E key menu - line 966 in WaypointSystem)
        QbeingAbledToUse = true;
        isInteracting = false;
        
        // Hide prompt initially - it will show again on next frame if player is still looking at NPC
        HideInteractionPrompt();
        
        Debug.Log("NPCInteractionDetector: Q key re-enabled, ready for new interaction");
    }
    
    /// <summary>
    /// Check if currently interacting with an NPC
    /// </summary>
    public bool IsInteracting()
    {
        return isInteracting;
    }
}

