using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GatewayComponent gatewayComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPrincipleDetected(PrincipleDetectionResult result)
    {
        Debug.Log(result);
    }

    private void Awake() 
    { 
        if (gatewayComponent != null) { gatewayComponent.PrincipleDetected += OnPrincipleDetected; } 
    }
    private void OnDestroy() 
    { 
        if (gatewayComponent != null) { gatewayComponent.PrincipleDetected -= OnPrincipleDetected; } 

    }
}
