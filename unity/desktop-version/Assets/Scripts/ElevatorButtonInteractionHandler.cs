using System.Collections;
using GLTF.Schema;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class ElevatorButtonInteractionHandler : MonoBehaviour, IInteractable
{
    public float delay=1f;

    public int i = 0;

    [SerializeField]
    private new Animator animation;

    [SerializeField]
    private GameObject TextBg;

    [SerializeField]
    private InteractionUserInterfaceManager UI;

    public string GetHint()
    {
        return "Press E to open Elevator";
    }

    public void React()
    {   
        StartCoroutine(OpenElevator());
        StartCoroutine(CloseElevator());

    }

    private IEnumerator CloseElevator() {
        animation.Play("Close"); 
        //
        yield return new WaitForSeconds(delay);
       TextBg.SetActive(false);
        
        


    }
    private IEnumerator OpenElevator()
    {
        animation.Play("Scene");
        if (i == 0)
        {
            TextBg.SetActive(true);
            UI.UpdateDialogue("Ladron - with original music by Paul Müller");
            i++;
        }
        yield return new WaitForSeconds(delay); 

        

    }
}
