
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DoorInteractionHandler : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _door;
    [SerializeField] private float _colliderDisableDelay = 0.5f;

    private Animator _doorAnimator;
    private Collider _doorCollider;
    private NavMeshObstacle _navMeshObstacle;
    private bool _isOpen;

    private void Awake()
    {
        if (_door == null) _door = gameObject;

        _doorAnimator = _door.GetComponent<Animator>();
        _doorCollider = _door.GetComponent<Collider>();
        _navMeshObstacle = _door.GetComponent<NavMeshObstacle>();

        // Add NavMeshObstacle if missing
        if (_navMeshObstacle == null)
        {
            _navMeshObstacle = _door.AddComponent<NavMeshObstacle>();
            _navMeshObstacle.carveOnlyStationary = false;
            _navMeshObstacle.carving = true;
        }
    }

    public string GetHint() => "Press E to open/close the door.";

    public void React()
    {
        _isOpen = !_isOpen;
        _doorAnimator.SetBool("IsOpened", _isOpen);

        // Handle collision and navigation
        if (_isOpen)
        {
            StartCoroutine(DisableCollisionAfterDelay(_colliderDisableDelay));
        }
        else
        {
            EnableCollisionImmediately();
        }
    }

    private System.Collections.IEnumerator DisableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        //if (_doorCollider != null)
        //    _doorCollider.enabled = false;

        if (_navMeshObstacle != null)
            _navMeshObstacle.enabled = false;
    }

    private void EnableCollisionImmediately()
    {
        if (_doorCollider != null)
            _doorCollider.enabled = true;

        if (_navMeshObstacle != null)
            _navMeshObstacle.enabled = true;
    }

    public bool IsDoorOpen()
    {
        return _isOpen;
    }

}
