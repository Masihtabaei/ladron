using UnityEngine;
using System.Collections;

public class AudioSwitcher : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource oneShot;
    public AudioSource bg;
    public AudioClip oneShotClip;           
    public AudioClip elevatorBackground;
    public AudioClip loopingClip;           
    public AudioClip bing;
    public BoxCollider buttons;

    void Start()
    {
        buttons.enabled = false;
        StartCoroutine(PlayOneShotThenLoop());
        
    }

    IEnumerator PlayOneShotThenLoop()
    {

        oneShot.loop = false;
        oneShot.clip = oneShotClip;
        oneShot.Play();

        bg.loop = true;
        bg.clip = elevatorBackground;
        bg.Play();

        yield return new WaitForSeconds(60.0f);
        bg.Stop();
        bg.loop = false;
        bg.clip = bing;
        bg.Play();
        yield return new WaitForSeconds(bing.length);
        buttons.enabled = true;

         
        
        audioSource.loop = true;
        audioSource.clip = loopingClip;
        audioSource.Play();
        

    }
}
