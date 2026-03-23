using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("------------ Audio Source ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] public AudioSource footStepSource;
    [SerializeField] public AudioSource earthquakeSource;

    [Header("------------ Audio Clip ------------")]
    public AudioClip backGround;
    public AudioClip moveStep;
    public AudioClip eatHP;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip jump;
    public AudioClip landed;
    public AudioClip takeCoin;
    public AudioClip earthquake;
    public AudioClip enemyDie;
    public AudioClip enemyHurt;
    public AudioClip playerHurt;
    public AudioClip teleportClip;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        footStepSource.clip = moveStep;
        earthquakeSource.clip = earthquake;
        musicSource.clip = backGround;
        musicSource.Play();
    }

    public void PlayFootStep()
    {
        footStepSource.Play();
    }

    public void PlayEarthquake()
    {
        earthquakeSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlaySFXForSeconds(AudioClip clip, float duration)
    {
        StartCoroutine(PlayClipCoroutine(clip, duration));
    }

    private IEnumerator PlayClipCoroutine(AudioClip clip, float duration)
    {
        SFXSource.clip = clip;
        SFXSource.Play();
        yield return new WaitForSeconds(duration);
        SFXSource.Stop();
    }

}
