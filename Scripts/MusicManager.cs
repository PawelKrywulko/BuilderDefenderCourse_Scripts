﻿using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _volume = 0.5f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        _audioSource.volume = _volume;
    }

    public void IncreaseVolume()
    {
        _volume += 0.1f;
        _volume = Mathf.Clamp01(_volume);
        _audioSource.volume = _volume;
        PlayerPrefs.SetFloat("musicVolume", _volume);
    }

    public void DecreaseVolume()
    {
        _volume -= 0.1f;
        _volume = Mathf.Clamp01(_volume);
        _audioSource.volume = _volume;
        PlayerPrefs.SetFloat("musicVolume", _volume);
    }

    public float GetVolume()
    {
        return _volume;
    }
}
