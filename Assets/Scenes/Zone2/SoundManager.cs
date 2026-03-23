using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    float currentVolume;
    List<AudioSource> listAudio = new List<AudioSource>();

    void Start()
    {
        foreach (var audio in GetComponents<AudioSource>())
        {
            listAudio.Add(audio);
        }
    }

    public void ChangeVolume(float _volume)
    {
        currentVolume = _volume;
        foreach (var audio in listAudio)
        {
            audio.volume = _volume;
        }
    }

    public void PlayAudio(string _nameAudio)
    {
        AudioSource audio = null;

        foreach (var item in listAudio)
        {
            if (!item.isPlaying)
            {
                audio = item;
                break;
            }
        }

        if (audio == null)
        {
            audio = gameObject.AddComponent<AudioSource>();
            listAudio.Add(audio);
        }

        audio.volume = currentVolume;
        audio.clip = Resources.Load<AudioClip>("Sounds/" + _nameAudio);
        audio.Play();
    }
}
