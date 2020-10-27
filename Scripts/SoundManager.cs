using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private readonly Dictionary<Sound, AudioClip> _soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
    private float _volume = 0.5f;
    
    public enum Sound
    {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyWaveStarting,
        EnemyHit,
        EnemyDie,
        GameOver,
    }
    
    private AudioSource _audioSource;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);

        foreach (Sound sound in Enum.GetValues(typeof(Sound)))
        {        
            _soundAudioClipDictionary.Add(sound, Resources.Load<AudioClip>(sound.ToString()));
        }
    }

    public void PlaySound(Sound sound)
    {
        _audioSource.PlayOneShot(_soundAudioClipDictionary[sound], _volume);
    }

    public void IncreaseVolume()
    {
        _volume += 0.1f;
        _volume = Mathf.Clamp01(_volume);
        PlayerPrefs.SetFloat("soundVolume", _volume);
    }

    public void DecreaseVolume()
    {
        _volume -= 0.1f;
        _volume = Mathf.Clamp01(_volume);
        PlayerPrefs.SetFloat("soundVolume", _volume);
    }

    public float GetVolume()
    {
        return _volume;
    }
}
