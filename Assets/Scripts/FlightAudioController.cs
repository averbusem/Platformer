using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip flyingClip;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip fireballClip;
    private GameObject player;
    private List<AudioSource> audioSources = new List<AudioSource>(); // List to store multiple AudioSources
    private SpriteRenderer objectRenderer; // Для проверки видимости объекта
    private EnemyFlight ef;

    private bool isFlyingMovementSound = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        objectRenderer = GetComponentInParent<SpriteRenderer>();
        ef = GetComponentInParent<EnemyFlight>();
        // Create an AudioSource for each sound effect
        CreateAudioSource(flyingClip);
        CreateAudioSource(fireballClip);
        CreateAudioSource(takeDamageClip);
    }

    private void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > 13.5 || ef.IsDead())
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

    public void PlayFlyingSound()
    {
        if (!isFlyingMovementSound && flyingClip != null)
        {
            PlaySound(flyingClip, true);  // Loop movement sound
            isFlyingMovementSound = true;
        }
    }

    public void StopFlyingSound()
    {
        if (isFlyingMovementSound)
        {
            StopSound(flyingClip);
            isFlyingMovementSound = false;
        }
    }

    public void PlayFireballSound()
    {
        PlaySound(fireballClip);
    }

    public void PlayTakeDamageSound()
    {
        PlaySound(takeDamageClip);
    }

    private void PlaySound(AudioClip clip, bool loop = false)
    {
        foreach (var source in audioSources)
        {
            if (source.clip == clip && !source.isPlaying)
            {
                source.loop = loop;
                source.volume = 0.3f;
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
            if (!source.isPlaying)
            {
                source.UnPause();
            }
        }
    }
}

