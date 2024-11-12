using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip swordAttackClip;
    [SerializeField] private AudioClip fireballClip;
    [SerializeField] private AudioClip movementClip;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip landingClip;
    [SerializeField] private AudioClip fallingClip;
    [SerializeField] private AudioClip flyingClip;

    private List<AudioSource> audioSources = new List<AudioSource>(); // List to store multiple AudioSources

    private bool isPlayingMovementSound = false;
    private bool isPlayingFallingSound = false;
    private bool isPlayingFlyingSound = false;

    public bool IsPlayingFallingSound => isPlayingFallingSound;
    public bool IsPlayingFlyingSound => isPlayingFlyingSound;

    private void Awake()
    {
        // Create an AudioSource for each sound effect
        CreateAudioSource(jumpClip);
        CreateAudioSource(swordAttackClip);
        CreateAudioSource(fireballClip);
        CreateAudioSource(movementClip);
        CreateAudioSource(takeDamageClip);
        CreateAudioSource(landingClip);
        CreateAudioSource(fallingClip);
        CreateAudioSource(flyingClip);
    }

    private void CreateAudioSource(AudioClip clip)
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = clip;
        audioSources.Add(newSource);
    }

    public void PlayJumpSound()
    {
        PlaySound(jumpClip);
    }

    public void PlaySwordAttackSound()
    {
        PlaySound(swordAttackClip);
    }

    public void PlayFireballSound()
    {
        PlaySound(fireballClip);
    }

    public void PlayMovementSound()
    {
        if (!isPlayingMovementSound && movementClip != null)
        {
            PlaySound(movementClip, true);  // Loop movement sound
            isPlayingMovementSound = true;
        }
    }

    public void StopMovementSound()
    {
        if (isPlayingMovementSound)
        {
            StopSound(movementClip);
            isPlayingMovementSound = false;
        }
    }

    public void PlayTakeDamageSound()
    {
        PlaySound(takeDamageClip);
    }

    public void PlayLandingSound()
    {
        PlaySound(landingClip);
    }

    public void PlayFallingSound()
    {
        if (!isPlayingFallingSound && fallingClip != null)
        {
            PlaySound(fallingClip);
            isPlayingFallingSound = true;
            Debug.Log("Falling sound played");
        }
    }

    public void StopFallingSound()
    {
        if (isPlayingFallingSound)
        {
            StopSound(fallingClip);
            isPlayingFallingSound = false;
        }
    }

    public void PlayFlyingSound()
    {
        if (!isPlayingFlyingSound && flyingClip != null)
        {
            PlaySound(flyingClip);
            isPlayingFlyingSound = true;
        }
    }

    public void StopFlyingSound()
    {
        if (isPlayingFlyingSound)
        {
            StopSound(flyingClip);
            isPlayingFlyingSound = false;
        }
    }

    private void PlaySound(AudioClip clip, bool loop = false)
    {
        foreach (var source in audioSources)
        {
            if (source.clip == clip && !source.isPlaying)
            {
                source.loop = loop;
                source.Play();
                break;
            }
        }
    }

    private void StopSound(AudioClip clip)
    {
        foreach (var source in audioSources)
        {
            if (source.clip == clip && source.isPlaying)
            {
                source.Stop();
                break;
            }
        }
    }
}
