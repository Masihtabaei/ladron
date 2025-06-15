using UnityEngine;
using System.Collections;

public class NarrationAudioController : MonoBehaviour
{
    [Header("References")]
    public GameObject player;            // assign in inspector: player GameObject
    private PlayerEngine playerEngine;

    [Header("References")]
    public GameObject elevator;          // assign in inspector: elevator GameObject
    private ElevatorButtonInteractionHandler elevatorEngine;

    public AudioSource musicSource;      // Background music
    public AudioSource narratorSource;   // Narrator voice

    public float fadeDuration = 1f;      // Time to fade in/out
    public float reducedVolume = 0.05f;  // How low the music gets

    private float originalVolume;

    void Start()
    {
        originalVolume = musicSource.volume;

        playerEngine = player.GetComponent<PlayerEngine>();
        elevatorEngine = elevator.GetComponent<ElevatorButtonInteractionHandler>();

        // Disable interaction during narration
        playerEngine.canInteract = false;

        narratorSource.Play();
        StartCoroutine(FadeMusicDuringNarration());
    }

    IEnumerator FadeMusicDuringNarration()
    {
        // Fade music down
        yield return StartCoroutine(FadeVolume(musicSource, reducedVolume, fadeDuration));

        // Wait for narration to finish
        yield return new WaitWhile(() => narratorSource.isPlaying);

        // Open elevator door
        elevatorEngine.OpenDoor();

        // Fade music back up
        yield return StartCoroutine(FadeVolume(musicSource, originalVolume, fadeDuration));

        // Re-enable interaction after narration and door opening start
        playerEngine.canInteract = true;
    }

    IEnumerator FadeVolume(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        source.volume = targetVolume;
    }
}
