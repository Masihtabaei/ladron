using UnityEngine;

public class Noctula : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Awake!");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start!");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update!");

    }

    void OnDestroy()
    {
        Debug.Log("Destroy!");
    }
}
