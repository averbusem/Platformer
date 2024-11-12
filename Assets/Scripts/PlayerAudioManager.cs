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
    private AudioSource audioSource;
    private bool isPlayingMovementSound = false;
    private bool isPlayingFallingSound = false;
    private bool isPlayingFlyingSound = false;
    public bool IsPlayingFallingSound => isPlayingFallingSound;
    public bool IsPlayingFlyingSound => isPlayingFlyingSound;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
            audioSource.clip = movementClip;
            audioSource.loop = true;
            audioSource.Play();
            isPlayingMovementSound = true;
        }
    }

    public void StopMovementSound()
    {
        if (isPlayingMovementSound)
        {
            audioSource.Stop();
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
            audioSource.PlayOneShot(fallingClip);  // Используем PlayOneShot
            isPlayingFallingSound = true;
            Debug.Log("Falling sound played");
        }
    }

    public void StopFallingSound()
    {
        if (isPlayingFallingSound)
        {
            audioSource.Stop();
            audioSource.loop = false;
            isPlayingFallingSound = false;
        }
    }

    public void PlayFlyingSound()
    {
        if (!isPlayingFlyingSound && flyingClip != null)
        {
            audioSource.PlayOneShot(flyingClip);  
            isPlayingFlyingSound = true;
            //Debug.Log("Flying sound played");
        }
    }

    public void StopFlyingSound()
    {
        if (isPlayingFlyingSound)
        {
            audioSource.Stop();
            isPlayingFlyingSound = false;
        }
    }
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
