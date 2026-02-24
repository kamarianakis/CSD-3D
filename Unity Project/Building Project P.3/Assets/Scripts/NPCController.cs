/*This script handles NPC behavior, animations, and interaction state.
 * It manages the NPC's response to player interaction and coordinates with the chat system.*/
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("NPC Identity")]
    [Tooltip("Name of the NPC (displayed in chat)")]
    public string npcName = "University Assistant";
    
    [Header("References")]
    [Tooltip("Animator component for NPC animations (optional)")]
    public Animator npcAnimator;
    
    [Tooltip("Reference to the player GameObject")]
    public GameObject player;
    
    [Tooltip("Chat UI component (will be created in next step)")]
    public NPCChatUI chatUI;
    
    [Header("Interaction Settings")]
    [Tooltip("Distance at which player can interact with NPC")]
    public float interactionRange = 3f;
    
    [Header("Debug")]
    [Tooltip("Show interaction range in Scene view")]
    public bool showInteractionRange = false;
    
    // State management
    private bool isInteracting = false;
    private bool playerInRange = false;
    
    // Reference to interaction detector (optional, for coordination)
    private NPCInteractionDetector interactionDetector;
    
    void Start()
    {
        // Try to find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj;
            }
        }
        
        // Try to find interaction detector
        if (player != null)
        {
            interactionDetector = player.GetComponent<NPCInteractionDetector>();
        }
        
        // Initialize animator state
        if (npcAnimator != null)
        {
            npcAnimator.SetBool("IsTalking", false);
            npcAnimator.SetTrigger("Idle");
        }
    }
    
    void Update()
    {
        // Check player distance (optional - can be handled by interaction detector)
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            bool wasInRange = playerInRange;
            playerInRange = distance <= interactionRange;
            
            // Handle range state changes
            if (playerInRange && !wasInRange && !isInteracting)
            {
                OnPlayerEnterRange();
            }
            else if (!playerInRange && wasInRange && !isInteracting)
            {
                OnPlayerExitRange();
            }
        }
    }
    
    /// <summary>
    /// Called when player enters interaction range
    /// </summary>
    void OnPlayerEnterRange()
    {
        // Play greeting animation (if available)
        if (npcAnimator != null)
        {
            npcAnimator.SetTrigger("Greet");
        }
        
        // Optional: Play greeting sound, show name tag, etc.
    }
    
    /// <summary>
    /// Called when player exits interaction range
    /// </summary>
    void OnPlayerExitRange()
    {
        if (!isInteracting)
        {
            // Return to idle animation
            if (npcAnimator != null)
            {
                npcAnimator.SetTrigger("Idle");
            }
        }
    }
    
    /// <summary>
    /// Called when player starts interaction (presses E)
    /// </summary>
    public void StartInteraction()
    {
        Debug.Log("NPCController: StartInteraction called for " + npcName);
        
        if (isInteracting)
        {
            Debug.Log("NPCController: Already interacting, returning");
            return; // Already interacting
        }
        
        isInteracting = true;
        
        // Play talking animation (if available)
        if (npcAnimator != null)
        {
            npcAnimator.SetBool("IsTalking", true);
        }
        
        // Open chat UI
        if (chatUI != null)
        {
            Debug.Log("NPCController: Opening chat UI");
            chatUI.OpenChat(this);
        }
        else
        {
            Debug.LogError($"NPCController: Chat UI is NULL! Cannot open chat for {npcName}. Make sure Chat UI is assigned in Inspector!");
            
            // Temporary: End interaction immediately if no chat UI
            Invoke(nameof(EndInteraction), 0.1f);
        }
    }
    
    /// <summary>
    /// Called when interaction ends (chat closes)
    /// </summary>
    public void EndInteraction()
    {
        Debug.Log("NPCController: EndInteraction called for " + npcName);
        
        if (!isInteracting)
        {
            Debug.Log("NPCController: Not currently interacting, returning");
            return; // Not interacting
        }
        
        isInteracting = false;
        
        // Stop talking animation
        if (npcAnimator != null)
        {
            npcAnimator.SetBool("IsTalking", false);
        }
        
        // DON'T call chatUI.CloseChat() here - it creates a loop!
        // The chat UI is already closed by the button or ESC key
        
        // Notify interaction detector (this resets the detection state)
        if (interactionDetector != null)
        {
            interactionDetector.OnInteractionEnd();
        }
        else
        {
            // Try to find it if not assigned
            if (player != null)
            {
                interactionDetector = player.GetComponent<NPCInteractionDetector>();
                if (interactionDetector != null)
                {
                    interactionDetector.OnInteractionEnd();
                }
            }
        }
        
        // Return to idle
        if (npcAnimator != null)
        {
            npcAnimator.SetTrigger("Idle");
        }
        
        Debug.Log("NPCController: Interaction ended, ready for new interaction");
    }
    
    /// <summary>
    /// Check if currently interacting with player
    /// </summary>
    public bool IsInteracting()
    {
        return isInteracting;
    }
    
    /// <summary>
    /// Get NPC name
    /// </summary>
    public string GetNPCName()
    {
        return npcName;
    }
    
    // Visual debug in Scene view
    void OnDrawGizmosSelected()
    {
        if (showInteractionRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}

