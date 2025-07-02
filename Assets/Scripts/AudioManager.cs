using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public bool isPlaying;

    public Slider progressSlider;
    public TMP_Text currentTimeText;
    public TMP_Text durationText;
    private bool isDragging = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleAudio();
        }

        UpdateProgressBar();
    }

    public void ToggleAudio()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            // If at the end, restart from beginning
            if (source.time >= source.clip.length - 0.1f)
            {
                source.time = 0;
                progressSlider.value = 0;
            }
            source.Play();
        }
        else
        {
            source.Pause();
        }
    }

    private void UpdateProgressBar()
    {
        if (source.clip == null || isDragging) return;

        // Calculate progress (clamped to prevent overshooting)
        float progress = Mathf.Clamp01(source.time / source.clip.length);
        progressSlider.value = progress;

        UpdateTimeDisplays();
    }

    private void UpdateTimeDisplays()
    {
        if (currentTimeText != null)
        {
            currentTimeText.text = FormatTime(source.time);
        }

        if (durationText != null && source.clip != null)
        {
            durationText.text = FormatTime(source.clip.length);
        }
    }

    public void OnSliderDragStart()
    {
        isDragging = true;
    }

    public void OnSliderDragEnd()
    {
        if (source.clip == null) return;

        isDragging = false;

        // Handle seeking to end specially
        if (progressSlider.value >= 0.999f)
        {
            source.time = source.clip.length - 0.1f;
            progressSlider.value = 1f;
            if (isPlaying) source.Play(); // Keep playing if we were playing
        }
        else
        {
            source.time = progressSlider.value * source.clip.length;
            if (!source.isPlaying && isPlaying) source.Play();
        }
    }

    public void OnSliderValueChanged()
    {
        if (isDragging && source.clip != null)
        {
            // Update current time display while dragging
            if (currentTimeText != null)
            {
                float displayTime = progressSlider.value * source.clip.length;
                currentTimeText.text = FormatTime(displayTime);
            }
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
    }

    public void OnNewAudioLoaded(AudioClip newClip)
    {
        source.clip = newClip;
        progressSlider.value = 0;
        UpdateTimeDisplays();
    }
}