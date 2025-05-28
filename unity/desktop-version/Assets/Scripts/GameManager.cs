using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GatewayComponent _gatewayComponent;

    [SerializeField]
    private CountdownClock _countdownClock;

    public void OnPrincipleDetected(PrincipleDetectionResult result)
    {
        Debug.Log(result);
    }

    public void OnTimeOut() 
    {
        Debug.Log("Ran out of time!");
    }
    private void Awake()
    {
        if (_gatewayComponent != null)
        {
            _gatewayComponent.PrincipleDetected += OnPrincipleDetected;
            _countdownClock.TimeOut += OnTimeOut;
        }
    }
    private void OnDestroy()
    {
        if (_gatewayComponent != null)
        {
            _gatewayComponent.PrincipleDetected -= OnPrincipleDetected; 
            _countdownClock.TimeOut -= OnTimeOut;
        }

    }
}
