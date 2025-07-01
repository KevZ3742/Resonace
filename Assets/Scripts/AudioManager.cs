using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public bool isPlaying;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleAudio();
        }
    }

    public void ToggleAudio()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            source.Play();
        }
        else
        {
            source.Pause();
        }
    }
}
