﻿using UnityEngine;
using UnityEngine.Rendering;

public class ChromaticAberrationEffect : MonoBehaviour
{
    public static ChromaticAberrationEffect Instance { get; private set; }
    
    [SerializeField] private float decreaseSpeed = 1f;
    
    private Volume _volume;

    private void Awake()
    {
        Instance = this;
        _volume = GetComponent<Volume>();
    }

    private void Update()
    {
        if (_volume.weight > 0)
        {
            _volume.weight -= Time.deltaTime * decreaseSpeed;
        }
    }

    public void SetWeight(float weight)
    {
        _volume.weight = weight;
    }
}
