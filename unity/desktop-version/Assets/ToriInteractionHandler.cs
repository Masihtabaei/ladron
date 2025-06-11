using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class ToriInteractionHandler : MonoBehaviour, IInteractable
{
    [SerializeField]
    private ProfessorInteractionHandler PIH;


    [SerializeField]
    private GameObject DialogueWindow;

    [SerializeField]
    private Transform Player;

    [SerializeField]
    private Transform Tori;

    [SerializeField]
    private BoxCollider ToriColl;

    [SerializeField]
    private PlayerEngine PlayerMovement;

    [SerializeField]
    private InteractionUserInterfaceManager UI;
    [SerializeField]
    private string[] DialogueStrings = { "Hi, Ladron.", "Ja, haha, du siehst richtig." };

    [SerializeField] private float dialogueDelay = 2f;
    public string GetHint()
    {
        bool tutorialActive = PIH.tutorialActive;
        
            return "Press E to interact with Tori.";
        
    }

    public void React()
    {
        bool tutorialActive = PIH.tutorialActive;
        if (tutorialActive == true)
        {
            ToriColl.enabled = false;
            DialogueWindow.SetActive(true);
            Vector3 direction = Player.position - transform.position;

            direction.y = 0;


            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Tori.transform.rotation = targetRotation;
            }

            PlayerMovement._speed = 0;
            StartCoroutine(ShowDialogue());
        }
        else { StartCoroutine(Warning()); }
    }
    //UI.UpdateDialogue(DialogueStrings.Length.ToString());

    private IEnumerator Warning() {
        ToriColl.enabled = false;
        DialogueWindow.SetActive(true);
        Vector3 direction = Player.position - transform.position;

        direction.y = 0;


        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Tori.transform.rotation = targetRotation;
        }

        PlayerMovement._speed = 0;
        UI.UpdateDialogue("Was machst du noch hier?");
        yield return new WaitForSeconds(dialogueDelay);
        UI.UpdateDialogue("Alles was du von mir brauchst, ist im kleinen Schrank.");
        yield return new WaitForSeconds(dialogueDelay);
        UI.UpdateDialogue("...");
        yield return new WaitForSeconds(dialogueDelay);
        UI.UpdateDialogue("Dein Zimmer ist am anderen Ende des Korridors.");
        yield return new WaitForSeconds(dialogueDelay);
        UI.UpdateDialogue("Viel Erfolg beim Prompten.");
        yield return new WaitForSeconds(dialogueDelay);
        UI.UpdateDialogue("Dir bleiben nur wenige Stunden!");
        yield return new WaitForSeconds(dialogueDelay);
        DialogueWindow.SetActive(false);
        PlayerMovement._speed = 5;
    }
    private IEnumerator ShowDialogue()
    {
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
        UI.UpdateDialogue("Hm, Tori hat etwas für mich in seinem kleinen Schrank.");
        yield return new WaitForSeconds(dialogueDelay);
        UI.UpdateDialogue("Das sollte ich mir angucken.");
        yield return new WaitForSeconds(dialogueDelay);
        UI.UpdateDialogue("Es hilft mir bestimmt die Prüfungsergebnisse aus Noctula zu kitzeln.");
        yield return new WaitForSeconds(dialogueDelay);
        DialogueWindow.SetActive(false);
        PIH.tutorialActive = false;
        ToriColl.enabled = true;
    }
}
