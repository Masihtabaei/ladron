using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ProfessorMovement : MonoBehaviour
{
    public Transform doorPosition;
    public Transform roomTargetPosition;
    public Animator professorAnimator;
    public Action GameOverReached;
    public Noctula noctula;
    public DoorInteractionHandler doorHandler;
    private NavMeshAgent agent;

    [SerializeField]
    private PlayerEngine _player;

    [SerializeField]
    private GameObject _inbox;

    [SerializeField]
    private TextMeshProUGUI _inboxText;

    private enum State
    {
        Idle,
        GoingToDoor,
        WaitingAtDoor,
        EnteringRoom,
        Deciding,
        Returning,
        ReachedDestination
    }

    private State currentState = State.Idle;

    private Vector3 initialPosition;

    public bool playerIsHidden = false;
    public bool canPlayerStillHide = false;

    //to make professor looks toward the player
    public Transform playerTransform;


    //red overlay
    public Material overlayMaterial;  // assign in Inspector

    public float maxOverlayDistance = 10f;  // tune this for your level

   
    void Awake()
    {
        initialPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();

        // Reset overlay here:
        ResetOverlayEffect();


        //SearchForPlayer();
    }



    void Update()
    {
        Debug.Log($"Current State: {currentState}, Path Status: {agent.pathStatus}, Remaining Distance: {agent.remainingDistance}");
        bool isAtInitialPosition = Vector3.Distance(transform.position, initialPosition) < 0.1f;

        switch (currentState)
        {
            case State.Idle:
                
                professorAnimator.SetBool("shouldWalk", false);

                ResetOverlayEffect();

                break;

            case State.GoingToDoor:
                UpdateOverlayEffect();
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentState = State.WaitingAtDoor;
                    professorAnimator.SetBool("shouldWalk", false);
                    OpenDoorAndEnterRoom();
                }
                else
                {
                    float speed = agent.velocity.magnitude;
                    professorAnimator.SetBool("shouldWalk", speed > 0.2f);
                }
                break;

            case State.EnteringRoom:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentState = State.Deciding;
                    professorAnimator.SetBool("shouldWalk", false);
                    StartCoroutine(DecideWhatToDo());
                }
                else if (agent.pathStatus == NavMeshPathStatus.PathPartial)
                {
                    Debug.LogWarning("Path is blocked!");
                }
                else
                {
                    professorAnimator.SetBool("shouldWalk", agent.velocity.magnitude > 0.2f);
                }
                break;

            case State.Deciding:
                // Nothing here — handled by coroutine
                break;

            case State.Returning:
                UpdateOverlayEffect();
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentState = State.ReachedDestination;
                    professorAnimator.SetBool("shouldWalk", false);
                }
                else
                {
                    professorAnimator.SetBool("shouldWalk", agent.velocity.magnitude > 0.2f);
                }
                break;

            case State.ReachedDestination:
                UpdateOverlayEffect();
                // Check if player is hidden in any spot
                playerIsHidden = IsPlayerHidden();

                if (!playerIsHidden)
                {
                    // Look at player
                    Vector3 lookDir = playerTransform.position - transform.position;
                    lookDir.y = 0f; // don't tilt up/down
                    if (lookDir.magnitude > 0.1f)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
                    }
                    
                }
                break;
        }
        if(isAtInitialPosition)
            ResetOverlayEffect();

        
    }

    private void UpdateOverlayEffect()
    {
        float distanceToRoom = Vector3.Distance(transform.position, roomTargetPosition.position);
        float t = Mathf.Clamp01(1f - (distanceToRoom / maxOverlayDistance));

        overlayMaterial.SetFloat("_VignetteStrength", Mathf.Lerp(0.5f, 2.0f, t));
        overlayMaterial.SetFloat("_PulseAmount", Mathf.Lerp(0.1f, 0.6f, t));
    }

    private void ResetOverlayEffect()
    {
        // Set overlay parameters to zero or base neutral values
        overlayMaterial.SetFloat("_VignetteStrength", 0f);
        overlayMaterial.SetFloat("_PulseAmount", 0f);
    }

    private void OpenDoorAndEnterRoom()
    {
        if (doorHandler != null && !doorHandler.IsDoorOpen())
        {
            doorHandler.React();
        }

        StartCoroutine(EnterRoomAfterDelay(1.5f));
    }

    private System.Collections.IEnumerator EnterRoomAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);


   

        // Verify path before setting destination
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, roomTargetPosition.position, NavMesh.AllAreas, path))
        {
            currentState = State.EnteringRoom;
            //door is open -> player cannot hide anymore
            canPlayerStillHide = false;
            agent.SetDestination(roomTargetPosition.position);
        }
        else
        {         
            Debug.LogError("Failed to calculate path to room target!");  
            
        }



    }


   


    private System.Collections.IEnumerator DecideWhatToDo()
    {

        // Check if player is hidden in any spot
        playerIsHidden = IsPlayerHidden();

        if (playerIsHidden)
        {
            yield return new WaitForSeconds(2.0f); // Professor looks around
            //Debug.LogError("Player hidden → returning to start.");
            currentState = State.Returning;
            agent.SetDestination(initialPosition);
            professorAnimator.SetBool("shouldWalk", true);
            _inboxText.text = "Dear Ladron Nordal,\r\nThis is your official notice that your prompt engineering exam starts at 08:00 AM the next day of when you will be receiving this letter.\r\nProfessor Morning also wanted us to inform you that failing this exam will result in an instant removal from the student registry with no possibility to ever study computer science at any other university in Germany ever again.\r\nAny attempts to cheat, for example through strategic LLM prompting, will be prosecuted by Professor Morning himself. Additionally, the popular LLM Noctula was instruced to report any suspicous user behaviour as well as their respective IP adress to Professor Morning.\r\nWe, the University of Applied Sciences in Coburg, and all its faculty members including Prof. Morning cannot be held accountable or sued for whatever the prosecution by Professor Morning entails.\r\nGood luck on your exam!\r\nKind regards,\r\nThe examination committee.";
        }
        else
        {
            //Debug.LogError("Player found → angry!");
            //GameOverReached?.Invoke();
            //noctula._trustScore = -40.0f;
            noctula.GameOverReached?.Invoke();
            currentState = State.ReachedDestination; // No more moving
            
            professorAnimator.SetBool("shouldGetAngry", true);
        }
    }

    private bool IsPlayerHidden()
    {
        return _player.isHidden;
    }

    public void SearchForPlayer()
    {

        playerIsHidden = false;
        canPlayerStillHide = true;
        agent.isStopped = false;
        currentState = State.GoingToDoor;
        agent.SetDestination(doorPosition.position);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Draw current path
        if (agent.hasPath)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Gizmos.DrawSphere(agent.path.corners[i], 0.1f);
                Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
            }
        }

        // Draw NavMesh edges at doorway
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        Gizmos.color = Color.cyan;
        for (int i = 0; i < navMeshData.indices.Length; i += 3)
        {
            Vector3 a = navMeshData.vertices[navMeshData.indices[i]];
            Vector3 b = navMeshData.vertices[navMeshData.indices[i + 1]];
            Vector3 c = navMeshData.vertices[navMeshData.indices[i + 2]];

            if (Vector3.Distance(a, doorPosition.position) < 2f ||
                Vector3.Distance(b, doorPosition.position) < 2f ||
                Vector3.Distance(c, doorPosition.position) < 2f)
            {
                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(b, c);
                Gizmos.DrawLine(c, a);
            }
        }
    }

  

}
