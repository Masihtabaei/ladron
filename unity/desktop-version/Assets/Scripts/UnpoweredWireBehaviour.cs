using UnityEngine;

public class UnpoweredWireBehaviour : MonoBehaviour
{
    public UnpoweredWireStats unpoweredWire;
    void Start()
    {
        unpoweredWire = GetComponent<UnpoweredWireStats>();

    }

    // Update is called once per frame
    void Update()
    {
        if (unpoweredWire.connected)
        {

        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WireStats>()) {
            WireStats wire = collision.GetComponent<WireStats>();
            if (wire.color == unpoweredWire.color)
            {
                wire.connected = true;
                unpoweredWire.connected = true;
                wire.connectedPosition= gameObject.transform.position;
            }
            }
        }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WireStats>())
        {
            WireStats wire = collision.GetComponent<WireStats>();
             wire.connected = false;
                unpoweredWire.connected = false;
            
        }
    }
}
