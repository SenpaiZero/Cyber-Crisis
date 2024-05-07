using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    [SerializeField] AudioSource hurt;
    [SerializeField] AudioSource meunClick;
    [SerializeField] AudioSource pickup;
    [SerializeField] AudioSource talk;

    public void PlayMenuClick()
    {
        Play(meunClick);
    }

    public void PlayHurt()
    {
        Play(hurt);
    }

    public void PlayPickup()
    {
        Play(pickup);
    }

    public void PlayTalk()
    {
        Play(talk);
    }

    public void StopAllSounds()
    {
        hurt.Stop();
        meunClick.Stop();
        pickup.Stop();
        talk.Stop();
    }

    private void Play(AudioSource toPlay)
    {
        StopAllSounds();
        toPlay.Play();
    }
}
