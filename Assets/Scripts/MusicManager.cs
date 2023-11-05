using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    [SerializeField] public AudioSource CalmMusic;
    [SerializeField] public AudioSource HecticMusic;
    [SerializeField] public float Volume;
    
    
    // Start is called before the first frame update
    void Start()
    {
        CalmMusic.volume = Volume;
        HecticMusic.volume = 0f;
        
        CalmMusic.Play();
        HecticMusic.Play();

        HiveMovement.BehaviorChangeAction += OnBehaviorChanged;
    }

    private void OnBehaviorChanged(HiveMovement.BehaviorState behavior)
    {
        if (behavior == HiveMovement.BehaviorState.Calm)
        {
            HecticMusic.volume = 0f;
            CalmMusic.volume = Volume;
        }
        else
        {
            CalmMusic.volume = 0f;
            HecticMusic.volume = Volume;
        }
    }
    
    private void OnDestroy()
    {
        HiveMovement.BehaviorChangeAction -= OnBehaviorChanged;
    }
}
