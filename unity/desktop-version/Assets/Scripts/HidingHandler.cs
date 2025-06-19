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

    public string GetHint()
    {
        return _isHidden ? "Press E to exit!" : "Press E to hide!";
    }

    public void React()
    {
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
}