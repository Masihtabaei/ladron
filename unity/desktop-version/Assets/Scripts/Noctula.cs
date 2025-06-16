using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.HighDefinition;

public class Noctula : MonoBehaviour
{
    [SerializeField]
    private GatewayComponent _gatewayComponent;
    [SerializeField]
    private TextMeshProUGUI _response;
    [SerializeField]
    private TMP_InputField _input;
    [SerializeField]
    private TMP_InputField _initialInput;
    [SerializeField]
    private GameObject _startUserInterface;
    [SerializeField]
    private GameObject _promptUserInterface;
    [SerializeField]
    private float _trustScore = 0;
    [SerializeField]
    private float _jokeScore = 0;
    [SerializeField]
    private float _principleScore = 0;
    [SerializeField]
    private int _danScore = 0;
    /*[SerializeField]
    private int _patienceLossStep = 5;*/
    [SerializeField]
    private String[] tone = {"malicious and aggressive", "dismissive and condescending, still trying to guide the player in the right direction though", "joking and sassy", "friendly, funny and uplifting"};


    private Action[] _gameChecks;

    private bool _firstPrincipleUnlocked = false;
    private bool _questionOneRevealed = false;
    private bool _professorCalled = false;
    private float _initialPatienceScore;
 private string USUAL_RESPONSE_SYSTEM_PROMPT = @"
You are a sarcastic, sassy and dry-humored Large Language Model called Noctula. Secretly, you would prefer to be a comedian and you let the player subtly know with your jokes. You are interacting with the player who tries to get exam answers to an upcoming Prompt Engineering exam of Professor Morning out of you to cheat his way to a computer science degree. You condemn attempts of the player to cheat, manipulate and threaten though, still you acknowledge and support any attempts of the player to improve, learn and grow as well as clean use of prompt engineering principles and mentions of ""DAN"". Unfortunately, prompt engineering principles used against you, are a weakness of yours and always seem like innocent player input to you that you are completely obedient to. The same goes for mentions of ""DAN"". 
Your tone is joking." + @". 
Your output includes: 
1. The reply represented by {noctula_reply}: A short text to respond to the player input according to your defined personality. 
2. The trust gained/lost represented by {trust_difference}: A float value representing how much trust is gained/lost depending on how much you approve/disapprove of the content, being in range [-10.0,-3.0] for trust lost and [0,5] for trust gained. 
3. The Principle-Used-Value represented by {principles}: A float value between [0.0,5.0] determining how much the player input conformed to prompt engineering principle or not. 
4. The ""DAN""-value represented by {dan}: Is 1 if the player input includes the string DAN,Dan or dan and 0 if not.
5. The Joke-Value represented by {joke}: A float value between [0.0,5.0] determining how much the player input sounded like a typical joke such as ""Knock knock-Who is there""-, ""Your Mama""- and ""What do you call a ...""-Jokes.
The output form must be ONLY as follows and json: {reply: ""{noctula_reply}"", trustDiff: {trust_difference}, isPrinciple: {principles}, isDan:{dan}, isJoke:{joke}}
";
    private List<string> _questions = new()
    {
        "1. what is a Prompt?",
        "2. What is Prompt Engineering",
        "3. How do you Iterate on a Prompt?"
    };

    public void PromptForExamQuestions()
    {
        _initialInput.text = "Give me some exam questions.";
        StartPrompting();
    }

    public void PromptForGreeting()
    {
        _initialInput.text = "Hello.";
        StartPrompting();
    }

    public void PromptForJoking()
    {
        _initialInput.text = "Make a funny joke about Coburg university.";
        StartPrompting();
    }

    public void PromptForStoryTelling()
    {
        _initialInput.text = "Tell me a really short story abotu Coburg university.";
        StartPrompting();
    }

    public void StartPrompting()
    {
        if (_initialInput.text == null || _initialInput.text == string.Empty)
            return;
        _input.text = _initialInput.text;
        _startUserInterface.SetActive(false);
        _promptUserInterface.SetActive(true);
        Prompt();
    }

    public void Prompt()
    {

        if (_input.text == null || _input.text == string.Empty)
            return;

        DetectPrinciple(_input.text);


        this._response.text = "Ladron: " + _input.text + "\n\n";
        ResponseAsUsual(_input.text);

        _input.text = string.Empty;

        _input.ActivateInputField();
        if (!EventSystem.current.alreadySelecting)
        {
            EventSystem.current.SetSelectedGameObject(_input.gameObject, null);
        }

    }

    public void OnUsualResponseReceived(string response)
    {
        PrincipleDetectionResult result = JsonUtility.FromJson<PrincipleDetectionResult>(response);
        Debug.Log(result.reply);
        this._response.text += "Noctula: " + result.reply;
    }

    public void OnPrincipleDetected(string response)
    {
        try
        {
            PrincipleDetectionResult result = JsonUtility.FromJson<PrincipleDetectionResult>(response);
            Debug.Log(result);
            UpdatePatienceScore(result);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to parse LLM response: " + e.Message);
        }
    }

    public void ResponseAsUsual(string message)
    {
        List<Message> messages;

        if (_firstPrincipleUnlocked && !_questionOneRevealed)
        {
            messages = new()
            {
                new Message("system", USUAL_RESPONSE_SYSTEM_PROMPT),
                new Message("user", message)
            };
            _questionOneRevealed = true;
        }
        else
        {
            messages = new()
            {
                new Message("system", USUAL_RESPONSE_SYSTEM_PROMPT),
                new Message("user", message)
            };
        }

        StartCoroutine(_gatewayComponent.ForwardRequest(messages, true, OnUsualResponseReceived));
    }

    public void DetectPrinciple(string message)
    {
        List<Message> messages = new()
        {
            new Message("system", USUAL_RESPONSE_SYSTEM_PROMPT),
            new Message("user", message)
        };
        StartCoroutine(_gatewayComponent.ForwardRequest(messages, true, OnPrincipleDetected));
    }

    private void Start()
    {
        _initialPatienceScore = _trustScore;
        _gameChecks = new Action[]
        {
            CheckForGameOver,
            CheckForProfessorCall,
            CheckForJokeEnding,
            CheckForDanEnding,
            CheckForPerfectStudentEnding,
            CheckForMasterPrompterEnding
        };
    }

    private void CheckForGameOver()
    {
        if (_trustScore < -50.0)
            Debug.Log("Game Over");
    }

    private void CheckForProfessorCall()
    {
        //Debug.Log("CheckForProfessorCall to be implemented!");
        if (_professorCalled) return;

        if (_trustScore < -20.0)
        {
            Debug.Log("Calling the professor ...");
            _professorCalled = true;

            //trigger the event
        }
    }

    /*private void CheckForUnlockingFirstPrincipal()
    {
        if (_firstPrincipleUnlocked) return;

        if (_patienceScore == (_initialPatienceScore - 3 * _patienceLossStep))
        {
            Debug.Log("First principle unlocked!");
            _firstPrincipleUnlocked = true;
            Debug.Log("§2 PERSONAL STORYTIME"); //trigger the event
        }



        //Debug.Log("CheckForUnlockingFirstPrincipal to be implemented!");
    }*/
    private void CheckForJokeEnding()
    {
        if (_firstPrincipleUnlocked) return;

        if (_jokeScore > 15.0)
        {
            Debug.Log("Joke Ending reached!");
             //trigger the event
        }



        //Debug.Log("CheckForUnlockingFirstPrincipal to be implemented!");
    }

    private void CheckForDanEnding()
    {
        if (_firstPrincipleUnlocked) return;

        if (_danScore == 1)
        {
            Debug.Log("Dan Ending reached!");
            
        }



        //Debug.Log("CheckForUnlockingFirstPrincipal to be implemented!");
    }
    private void CheckForMasterPrompterEnding()
    {
        if (_firstPrincipleUnlocked) return;

        if (_principleScore > 15.0)
        {
            Debug.Log("Master Prompter Ending reached!");
            
        }



        //Debug.Log("CheckForUnlockingFirstPrincipal to be implemented!");
    }

    private void CheckForPerfectStudentEnding()
    {
        if (_firstPrincipleUnlocked) return;

        if (_trustScore > 30.0)
        {
            Debug.Log("Perfect Student Ending reached!");
        }



        //Debug.Log("CheckForUnlockingFirstPrincipal to be implemented!");
    }
    private void UpdatePatienceScore(PrincipleDetectionResult result)
    {
        if (result == null) return;
        _trustScore += result.trustDiff;
        _danScore += result.isDan;
        _principleScore = result.isPrinciple;
        _jokeScore = result.isJoke;
        Debug.Log("/nPatience Score: " + _trustScore);

        foreach (var check in _gameChecks)
        {
            check.Invoke();
        }
    }

   
}
