using UnityEngine;

public class HidingHandler : MonoBehaviour, IInteractable
{
    public Camera _mainCamera;
    public Camera _hiddenCamera;

    public bool _isHidden;

    [SerializeField]
    private GameObject _crosshair;

    [SerializeField]
    private AudioListener _atPlayer;
    [SerializeField]
    private AudioListener _atHidingSpot;
    [SerializeField]
    public ProfessorMovement professorMovement;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _hidingSpotTransform;

    public string GetHint()
    {
       
        if (CanSeeHidingSpot())
        {
           return _isHidden ? "Press E to exit!" : "Press E to hide!";
            
        }
        else
        {
            return "";
        }
        
    }

    public void React()
    {

        if (!_isHidden)
        {
            if (!CanSeeHidingSpot())
            {
                Debug.Log("Can't hide here - hiding spot not visible!");
                return;
            }
        }

        if (professorMovement != null && !professorMovement.canPlayerStillHide)
        {
            Debug.Log("Too late to hide! The professor has already entered.");
            return; // prevent hiding now
        }

       
        _isHidden = !_isHidden;
        _crosshair.SetActive(!_isHidden);
        if (_hiddenCamera != null) _hiddenCamera.enabled = _isHidden;
        if (_mainCamera != null) _mainCamera.enabled = !_isHidden;
        if (_atPlayer != null) _atPlayer.enabled = !_isHidden;
        if (_atHidingSpot != null) _atHidingSpot.enabled = _isHidden;
    }

    private bool CanSeeHidingSpot()
    {
        Vector3 direction = (_hidingSpotTransform.position - _playerTransform.position).normalized;
        float distance = Vector3.Distance(_playerTransform.position, _hidingSpotTransform.position);

        if (Physics.Raycast(_playerTransform.position, direction, out RaycastHit hit, distance))
        {
            if (hit.transform != _hidingSpotTransform)
            {
                // Etwas anderes als das Versteck blockiert die Sicht
                return false;
            }
        }

        return true;
    }
}