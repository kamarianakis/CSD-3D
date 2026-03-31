using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RobopetScript : MonoBehaviour
{
    public GameObject player;
    public Image eyes;
    public Animator animator;
    public AudioSource audioSource;

    public float tetherDistance;
    public float tetherSpeedMultiplier;
    public float followDistance = 2f;
    public float blinkIntervalMin = 0.5f;
    public float blinkIntervalMax = 2.5f;
    public float blinkDuration = 0.1f;
    public float noiseIntervalMin = 2f;
    public float noiseIntervalMax = 10f;

    private NavMeshAgent _navAgent;
    private bool _eyesClosed = false;
    private float _navSpeed;

    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();

        // Capture starting nav agent speed on start.
        if(_navAgent != null )
            _navSpeed = _navAgent.speed;

        StartCoroutine(AnimateEyes());
        StartCoroutine(AnimateNoises());
    }

    // Follows the player using the nav agent,
    // but stops at a distance to avoid being annoying.
    void MoveToPlayer()
    {
        if (_navAgent != null)
        {
            _navAgent.isStopped = false;
            _navAgent.destination = player.transform.position;

            // Start the moving animation
            animator.SetBool("moving", true);
        }
    }

    void Idle()
    {
        _navAgent.isStopped = true;

        animator.SetBool("moving", false);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceFromPlayer > tetherDistance)
        {
            // Move faster when too far away
            _navAgent.speed = _navSpeed * tetherSpeedMultiplier;
        } else
        {
            // Back to the original speed
            _navAgent.speed = _navSpeed;
        }
            
        if (distanceFromPlayer < followDistance)
        {
            // Reached player, idle for now
            Idle();
        }
        else
        {
            MoveToPlayer();
        }

        // Animate eye rect
        eyes.rectTransform.localScale = new Vector2(
            eyes.rectTransform.localScale.x, _eyesClosed 
            ? 0.02f 
            : 1f
        );
    }

    // Indefinitely oscillates between closed and open eye state, with randomness.
    IEnumerator AnimateEyes()
    {
        while (true)
        {
            _eyesClosed = false;
            yield return new WaitForSeconds(Random.Range(blinkIntervalMin, blinkIntervalMax));
            _eyesClosed = true;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    // Triggers noises in random intervals.
    IEnumerator AnimateNoises()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(noiseIntervalMin, noiseIntervalMax));
            audioSource.Play();
        }
    }
}
