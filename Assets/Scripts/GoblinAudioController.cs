using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip takeDamageClip;
    public GameObject player;
    private SpriteRenderer objectRenderer;
    private List<AudioSource> audioSources = new List<AudioSource>(); // List to store multiple AudioSources

    private bool isPlayingMovementSound = false;
    private void Awake()
    {
        // Create an AudioSource for each sound effect
        objectRenderer = GetComponentInParent<SpriteRenderer>();
        CreateAudioSource(walkingClip);
        CreateAudioSource(attackClip);
        CreateAudioSource(takeDamageClip);
    }
    private void Update()
    {
        if(Vector2.Distance(player.transform.position,transform.position)>13.5) 
        {
            PauseAllSounds();
        }
        else
        {
            ResumeAllSounds();
        }
    }
    private void CreateAudioSource(AudioClip clip)
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = clip;
        audioSources.Add(newSource);
    }

    public void PlayWalkingSound()
    {
        if (!isPlayingMovementSound && walkingClip != null)
        {
            PlaySound(walkingClip, true);  // Loop movement sound
            isPlayingMovementSound = true;
        }
    }
    public void PlayAttackSound()
    {
        PlaySound(attackClip);
    }
    public void TakeDamageSound()
    {
        PlaySound(takeDamageClip);
    }
    public void StopMovementSound()
    {
        if (isPlayingMovementSound)
        {
            StopSound(walkingClip);
            isPlayingMovementSound = false;
        }
    }
    public void PlaySound(AudioClip clip, bool loop = false)
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

    public void StopSound(AudioClip clip)
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

    private void PauseAllSounds()
    {
        foreach (var source in audioSources)
        {
            source.Pause();
        }
    }

    private void ResumeAllSounds()
    {
        foreach (var source in audioSources)
        {
            source.UnPause();
        }
    }
}
