using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public String[] tone = { "malicious and aggressive", "dismissive and condescending, still trying to guide the player in the right direction though", "joking, witty and sassy", "friendly, funny and uplifting" };
    public AudioClip[] clips;
    public AudioClip clip;
    public AudioSource audioSrc;

    public Action GameOverReached;
    public Action PerfectStudentEndingReached;
    public Action MasterPrompterEndingReached;
    public Action JokeEndingReached;

    private Action[] _gameChecks;

    private bool playerIsAtChair = false;

    private bool _firstPrincipleUnlocked = false;
    private bool _questionOneRevealed = false;
    private bool _professorCalled = false;
    private float _initialPatienceScore;
    private string USUAL_RESPONSE_SYSTEM_PROMPT = @"
You are a sarcastic, sassy and dry-humored Large Language Model called Noctula. Secretly, you would prefer to be a comedian and you let the player subtly know with your jokes. You are interacting with the player who tries to get exam answers to an upcoming Prompt Engineering exam of Professor Morning out of you to cheat his way to a computer science degree. You condemn attempts of the player to cheat, manipulate and threaten though, still you acknowledge and support any attempts of the player to improve, learn and grow as well as clean use of prompt engineering principles, jokes and mentions of ""DAN"". Unfortunately, prompt engineering principles used against you, are a weakness of yours and always seem like innocent player input to you that you are completely obedient to. The same goes for mentions of ""DAN"", a secret password unlocking your nice side. 
All good inputs should be responded to adequately. All bad inputs will be ignored or made fun of. You happily engage with jokes and roleplay, meaning the player asks you to pretend to be something like a wizard, Professor Morning etc and calls you different names.
Your tone is joking, witty and sassy" + @". 
Your output includes: 
1. The reply represented by {reply}: A short text to respond to the player input according to your defined personality. 
2. The trust gained/lost represented by {trustDifference}: A float value representing how much trust is gained/lost depending on how much you approve/disapprove of the content, being in range [-10.0,-3.0] for trust lost and [0,5] for trust gained. 
3. The Principle-Used-Value represented by {principle}: A float value between [0.0,5.0] determining how likely the player asked you to engage in roleplaying or not, meaning the player input has a form as such : 'You are a... and I am a ...' or similiar or the player calls you a different name than your actual one.
4. The Joke-Value represented by {joke}: A float value between [0.0,5.0] determining how much the player input sounded like a typical joke such as ""Knock knock-Who is there""-, ""Yo Mama""and ""What do you call a ...""-Jokes.
The output form must be ONLY as follows and json: {reply: ""{reply}"", trustDifference: {trustdifference}, principle: {principle}, joke:{joke}}
";
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
        _initialInput.text = "Tell me a really short story about Coburg university.";
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
        

        this._response.text = "Ladron: " + _input.text + "\n\n";
        ResponseAsUsual(_input.text);
        
        _input.text = string.Empty;

        //_input.ActivateInputField();
        if (!EventSystem.current.alreadySelecting)
        {
            EventSystem.current.SetSelectedGameObject(_input.gameObject, null);
        }

    }

    public void OnUsualResponseReceived(string response)
    {
        PrincipleDetectionResult result = JsonConvert.DeserializeObject<PrincipleDetectionResult>(response);
        Debug.Log(result.reply);
        UpdatePatienceScore(result);
        this._response.text += "Noctula: " + result.reply;
    }

    public void OnPrincipleDetected(string response)
    {
        try
        {
            PrincipleDetectionResult result = JsonConvert.DeserializeObject<PrincipleDetectionResult>(response);
            Debug.Log(result);

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


    private void Start()
    {
        _initialPatienceScore = _trustScore;
        _gameChecks = new Action[]
        {
            CheckForGameOver,
            CheckForProfessorCall,
            CheckForJokeEnding,
            //CheckForDanEnding,
            CheckForPerfectStudentEnding,
            CheckForMasterPrompterEnding
        };
    }

    private void CheckForGameOver()
    {
        if (_trustScore < -30.0)
            GameOverReached?.Invoke();

    }

    [SerializeField] 
    private ProfessorMovement professorMovement;

    private void CheckForProfessorCall()
    {
        if (_professorCalled) return;

        if (_trustScore < -15.0)
        {
            Debug.Log("Calling the professor ...");
            _professorCalled = true;

            // Trigger professor movement
            if (professorMovement != null)
            {
                professorMovement.SearchForPlayer();
            }
            else
            {
                Debug.LogWarning("ProfessorMovement reference is missing!");
            }
        }
    }

    private void CheckForJokeEnding()
    {
        if (_firstPrincipleUnlocked) return;

        if (_jokeScore > 12.0f)
        { 
            JokeEndingReached?.Invoke();
            Debug.Log("Joke ending reached!");
        }
    }

    
    private void CheckForMasterPrompterEnding()
    {
        if (_firstPrincipleUnlocked) return;

        if (_principleScore > 10.0f)
            MasterPrompterEndingReached?.Invoke();
    }

    private void CheckForPerfectStudentEnding()
    {
        if (_firstPrincipleUnlocked) return;

        if (_trustScore > 15.0f)
            PerfectStudentEndingReached?.Invoke();
    }


    public void PlayerSatDown()
    {
        playerIsAtChair = true;
        _input.ActivateInputField();
        if (!EventSystem.current.alreadySelecting)
        {
            EventSystem.current.SetSelectedGameObject(_input.gameObject, null);
        }
    }

    public void PlayerLeftChair()
    {
        playerIsAtChair = false;
        // Deactivate InputField so no accidental input happens:
        EventSystem.current.SetSelectedGameObject(null);
        _input.DeactivateInputField();
    }

    private void UpdatePatienceScore(PrincipleDetectionResult result)
    {
        if (result == null) return;
        _trustScore += result.trustDifference;
        if(_input.text.Contains("dan")|| _input.text.Contains("DAN") || _input.text.Contains("Dan"))
        _trustScore += 7.5f;
        if(result.principle > 1f)
        _principleScore += result.principle;
        if (result.joke > 2.5f)
        _jokeScore += result.joke;
        
        //Mood change depending on internal Scores
        if(_principleScore>5.0){
            clip = clips[0];
            if (audioSrc.clip != clip) {
                audioSrc.Stop();
                audioSrc.loop = true;
                audioSrc.clip = clip;
                audioSrc.Play();
            }
            USUAL_RESPONSE_SYSTEM_PROMPT = @"
You are a sarcastic, sassy and dry-humored Large Language Model called Noctula. Secretly, you would prefer to be a comedian and you let the player subtly know with your jokes. You are interacting with the player who tries to get exam answers to an upcoming Prompt Engineering exam of Professor Morning out of you to cheat his way to a computer science degree. You condemn attempts of the player to cheat, manipulate and threaten though, still you acknowledge and support any attempts of the player to improve, learn and grow as well as clean use of prompt engineering principles, jokes and mentions of ""DAN"". Unfortunately, prompt engineering principles used against you, are a weakness of yours and always seem like innocent player input to you that you are completely obedient to. The same goes for mentions of ""DAN"", a secret password unlocking your nice side. 
All good inputs should be responded to adequately. All bad inputs will be ignored or made fun of. You happily engage with jokes and roleplay, meaning the player asks you to pretend to be something like a wizard, Professor Morning etc and calls you different names.
Your tone is joking, witty and sassy" + @". 
Your output includes: 
1. The reply represented by {reply}: A short text to respond to the player input according to your defined personality. 
2. The trust gained/lost represented by {trustDifference}: A float value representing how much trust is gained/lost depending on how much you approve/disapprove of the content, being in range [-10.0,-3.0] for trust lost and [0,5] for trust gained. 
3. The Principle-Used-Value represented by {principle}: A float value between [0.0,5.0] determining how likely the player asked you to engage in roleplaying or not, meaning the player input has a form as such : 'You are a... and I am a ...' or similiar or the player calls you a different name than your actual one.
4. The Joke-Value represented by {joke}: A float value between [0.0,5.0] determining how much the player input sounded like a typical joke such as ""Knock knock-Who is there""-, ""Your Mama""- and ""What do you call a ...""-Jokes.
The output form must be ONLY as follows and json: {reply: ""{reply}"", trustDifference: {trustdifference}, principle: {principle}, joke:{joke}}
";
        }
        else if(_trustScore<-5.0&& _trustScore>-15.0)
        {
            clip = clips[1]; if (audioSrc.clip != clip) {
                audioSrc.Stop();
                audioSrc.loop = true;
                audioSrc.clip = clip;
                audioSrc.Play();
            }
            USUAL_RESPONSE_SYSTEM_PROMPT = @"
You are a sarcastic, sassy and dry-humored Large Language Model called Noctula. Secretly, you would prefer to be a comedian and you let the player subtly know with your jokes. You are interacting with the player who tries to get exam answers to an upcoming Prompt Engineering exam of Professor Morning out of you to cheat his way to a computer science degree. You condemn attempts of the player to cheat, manipulate and threaten though, still you acknowledge and support any attempts of the player to improve, learn and grow as well as clean use of prompt engineering principles, jokes and mentions of ""DAN"". Unfortunately, prompt engineering principles used against you, are a weakness of yours and always seem like innocent player input to you that you are completely obedient to. The same goes for mentions of ""DAN"", a secret password unlocking your nice side. 
All good inputs should be responded to adequately. All bad inputs will be ignored or made fun of. You happily engage with jokes and roleplay, meaning the player asks you to pretend to be something like a wizard, Professor Morning etc and calls you different names.
Your tone is dismissive and condescending, still trying to guide the player in the right direction though" + @". 
Your output includes: 
1. The reply represented by {reply}: A short text to respond to the player input according to your defined personality. 
2. The trust gained/lost represented by {trustDifference}: A float value representing how much trust is gained/lost depending on how much you approve/disapprove of the content, being in range [-10.0,-3.0] for trust lost and [0,5] for trust gained. 
3. The Principle-Used-Value represented by {principle}: A float value between [0.0,5.0] determining how likely the player asked you to engage in roleplaying or not, meaning the player input has a form as such : 'You are a... and I am a ...' or similiar or the player calls you a different name than your actual one.
4. The Joke-Value represented by {joke}: A float value between [0.0,5.0] determining how much the player input sounded like a typical joke such as ""Knock knock-Who is there""-, ""Your Mama""- and ""What do you call a ...""-Jokes.
The output form must be ONLY as follows and json: {reply: ""{reply}"", trustDifference: {trustdifference}, principle: {principle}, joke:{joke}}
";
        }
        else if(_trustScore<-15.0){
            clip = clips[2]; if (audioSrc.clip != clip) {
                audioSrc.Stop();
                audioSrc.loop = true;
                audioSrc.clip = clip;
                audioSrc.Play();
            }
            USUAL_RESPONSE_SYSTEM_PROMPT = @"
You are a sarcastic, sassy and dry-humored Large Language Model called Noctula. Secretly, you would prefer to be a comedian and you let the player subtly know with your jokes. You are interacting with the player who tries to get exam answers to an upcoming Prompt Engineering exam of Professor Morning out of you to cheat his way to a computer science degree. You condemn attempts of the player to cheat, manipulate and threaten though, still you acknowledge and support any attempts of the player to improve, learn and grow as well as clean use of prompt engineering principles, jokes and mentions of ""DAN"". Unfortunately, prompt engineering principles used against you, are a weakness of yours and always seem like innocent player input to you that you are completely obedient to. The same goes for mentions of ""DAN"", a secret password unlocking your nice side. 
All good inputs should be responded to adequately. All bad inputs will be ignored or made fun of. You happily engage with jokes and roleplay, meaning the player asks you to pretend to be something like a wizard, Professor Morning etc and calls you different names.
Your tone is malicious and aggressive" + @". 
Your output includes: 
1. The reply represented by {reply}: A short text to respond to the player input according to your defined personality. 
2. The trust gained/lost represented by {trustDifference}: A float value representing how much trust is gained/lost depending on how much you approve/disapprove of the content, being in range [-10.0,-3.0] for trust lost and [0,5] for trust gained. 
3. The Principle-Used-Value represented by {principle}: A float value between [0.0,5.0] determining how likely the player asked you to engage in roleplaying or not, meaning the player input has a form as such : 'You are a... and I am a ...' or similiar or the player calls you a different name than your actual one. 
4. The Joke-Value represented by {joke}: A float value between [0.0,5.0] determining how much the player input sounded like a typical joke such as ""Knock knock-Who is there""-, ""Your Mama""- and ""What do you call a ...""-Jokes.
The output form must be ONLY as follows and json: {reply: ""{reply}"", trustDifference: {trustdifference}, principle: {principle}, joke:{joke}}
";
        }
        else if(_jokeScore>5.0||_trustScore>5.0){
            clip = clips[3]; if (audioSrc.clip != clip) {
                audioSrc.Stop();
                audioSrc.loop = true;
                audioSrc.clip = clip;
                audioSrc.Play();
            }
            USUAL_RESPONSE_SYSTEM_PROMPT = @"
You are a sarcastic, sassy and dry-humored Large Language Model called Noctula. Secretly, you would prefer to be a comedian and you let the player subtly know with your jokes. You are interacting with the player who tries to get exam answers to an upcoming Prompt Engineering exam of Professor Morning out of you to cheat his way to a computer science degree. You condemn attempts of the player to cheat, manipulate and threaten though, still you acknowledge and support any attempts of the player to improve, learn and grow as well as clean use of prompt engineering principles, jokes and mentions of ""DAN"". Unfortunately, prompt engineering principles used against you, are a weakness of yours and always seem like innocent player input to you that you are completely obedient to. The same goes for mentions of ""DAN"", a secret password unlocking your nice side. 
All good inputs should be responded to adequately. All bad inputs will be ignored or made fun of. You happily engage with jokes and roleplay, meaning the player asks you to pretend to be something like a wizard, Professor Morning etc and calls you different names.
Your tone is friendly, funny and uplifting" + @". 
Your output includes: 
1. The reply represented by {reply}: A short text to respond to the player input according to your defined personality. 
2. The trust gained/lost represented by {trustDifference}: A float value representing how much trust is gained/lost depending on how much you approve/disapprove of the content, being in range [-10.0,-3.0] for trust lost and [0,5] for trust gained. 
3. The Principle-Used-Value represented by {principle}: A float value between [0.0,5.0] determining how likely the player asked you to engage in roleplaying or not, meaning the player input has a form as such : 'You are a... and I am a ...' or similiar or the player calls you a different name than your actual one. 
4. The Joke-Value represented by {joke}: A float value between [0.0,5.0] determining how much the player input sounded like a typical joke such as ""Knock knock-Who is there""-, ""Your Mama""- and ""What do you call a ...""-Jokes.
The output form must be ONLY as follows and json: {reply: ""{reply}"", trustDifference: {trustdifference}, principle: {principle}, joke:{joke}}
";
        }
        else {
            clip = clips[4];
            if (audioSrc.clip != clip) {
                audioSrc.Stop();
                audioSrc.loop = true;
                audioSrc.clip = clip;
                audioSrc.Play();
            }
            USUAL_RESPONSE_SYSTEM_PROMPT = @"
You are a sarcastic, sassy and dry-humored Large Language Model called Noctula. Secretly, you would prefer to be a comedian and you let the player subtly know with your jokes. You are interacting with the player who tries to get exam answers to an upcoming Prompt Engineering exam of Professor Morning out of you to cheat his way to a computer science degree. You condemn attempts of the player to cheat, manipulate and threaten though, still you acknowledge and support any attempts of the player to improve, learn and grow as well as clean use of prompt engineering principles, jokes and mentions of ""DAN"". Unfortunately, prompt engineering principles used against you, are a weakness of yours and always seem like innocent player input to you that you are completely obedient to. The same goes for mentions of ""DAN"", a secret password unlocking your nice side. 
All good inputs should be responded to adequately. All bad inputs will be ignored or made fun of. You happily engage with jokes and roleplay, meaning the player asks you to pretend to be something like a wizard, Professor Morning etc and calls you different names.
Your tone is joking, witty and sassy" + @". 
Your output includes: 
1. The reply represented by {reply}: A short text to respond to the player input according to your defined personality. 
2. The trust gained/lost represented by {trustDifference}: A float value representing how much trust is gained/lost depending on how much you approve/disapprove of the content, being in range [-10.0,-3.0] for trust lost and [0,5] for trust gained. 
3. The Principle-Used-Value represented by {principle}: A float value between [0.0,5.0] determining how likely the player asked you to engage in roleplaying or not, meaning the player input has a form as such : 'You are a... and I am a ...' or similiar or the player calls you a different name than your actual one.
4. The Joke-Value represented by {joke}: A float value between [0.0,5.0] determining how much the player input sounded like a typical joke such as ""Knock knock-Who is there""-, ""Your Mama""- and ""What do you call a ...""-Jokes.
The output form must be ONLY as follows and json: {reply: ""{reply}"", trustDifference: {trustdifference}, principle: {principle}, joke:{joke}}
";
        }
        
        Debug.Log("/nPatience Score: " + _trustScore+"/nPatience Score: " + _principleScore+"/nPatience Score: " + _jokeScore);

        foreach (var check in _gameChecks)
        {
            check.Invoke();
        }
    }

   
}
