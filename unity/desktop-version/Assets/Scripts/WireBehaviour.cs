using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WireBehaviour : MonoBehaviour
{
    bool mouseDown = false;
    public WireStats wireStats;
    public LineRenderer lineRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wireStats = gameObject.GetComponent<WireStats>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveWire();
        lineRenderer.SetPosition(1, new Vector3(gameObject.transform.position.x-0.1f, gameObject.transform.position.y - 0.1f,gameObject.transform.position.z));
    }

    void OnMouseDown()
    {
        mouseDown = true;
    }

    void OnMouseOver()
    {
        wireStats.movable = true;
    }

    void OnMouseExit()
    {
        if (!wireStats.moving) { wireStats.movable = false; }

    }
    void OnMouseUp()
    {
        mouseDown = false;
        if(!wireStats.connected)
        gameObject.transform.position = wireStats.startPosition;
        if(wireStats.connected)
            gameObject.transform.position = wireStats.connectedPosition;
    }
    void MoveWire()
    {
        if (mouseDown && wireStats.movable)
        {


            wireStats.moving = true;
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 1));
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, transform.parent.transform.position.z);
        }
   
    else{ wireStats.moving =false;} }
}
