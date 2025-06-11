using System.Collections;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class ProfessorInteractionHandler : MonoBehaviour, IInteractable
{
    public bool tutorialActive=true;

    [SerializeField]
    private GameObject DialogueWindow;

    [SerializeField]
    private Transform Player;

    [SerializeField]
    private Transform Professor;

    [SerializeField]
    private BoxCollider ProfessorColl;

    [SerializeField]
    private PlayerEngine PlayerMovement;

    [SerializeField]
    private InteractionUserInterfaceManager UI;
    [SerializeField]
    private string[] DialogueStrings = { "Ich habe dich bereits erwartet, Ladron.", "Test" };

    [SerializeField] private float dialogueDelay = 1.5f;
    public string GetHint()
    {
        if (tutorialActive==true)
        {
            return "Press E to interact with Professor Morgen.";
        }
        else { return "";}
    }

    public void React()
    {
        if (tutorialActive == true)
        {
            ProfessorColl.enabled = false;
            DialogueWindow.SetActive(true);
            Vector3 direction = Player.position - transform.position;

            direction.y = 0;

            
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Professor.transform.rotation = targetRotation;
            }
            
            PlayerMovement._speed=0;
            StartCoroutine(ShowDialogue());
        }
    }
            //UI.UpdateDialogue(DialogueStrings.Length.ToString());
    private IEnumerator ShowDialogue() { 
      for (int i = 0; i < DialogueStrings.Length; i++) 
            {

                UI.UpdateDialogue(DialogueStrings[i]);
                yield return new WaitForSeconds(dialogueDelay);
            
                


                //Text.ConvertTo="Du weißt ganz genau, welches Schicksal dir blüht."
                //System.sleep();
                //Text = null;
            }
            DialogueWindow.SetActive(false);
            yield return new WaitForSeconds(dialogueDelay);
            DialogueWindow.SetActive(true);
            PlayerMovement._speed = 5;
            UI.UpdateDialogue("Ich sollte zu Tori gehen. Er befindet sich im Zimmer rechts.");
            yield return new WaitForSeconds(dialogueDelay);
            DialogueWindow.SetActive(false);
            ProfessorColl.enabled = true;

    }
}
        
    
