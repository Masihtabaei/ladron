using NUnit.Framework;
using UnityEngine;

public class ConnectionScenario : MonoBehaviour
{
    public bool success=false;
    public WireStats one;
    public WireStats two;
    public WireStats three;
    public WireStats four;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        success = one.connected&& two.connected&& three.connected && four.connected;
    }
}
