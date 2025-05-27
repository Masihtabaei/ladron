using UnityEngine;

public class MailBoxController : MonoBehaviour, IInteractable
{
    public string GetHint()
    {
        return "I am a mailbox";
    }

    public void React()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
