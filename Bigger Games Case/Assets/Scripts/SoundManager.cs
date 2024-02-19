using System;
using System.Collections.Generic;
using System.Linq;
using Script;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SoundItem
{
    public string name;
    public AudioClip source;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]private AudioSource audioSource;
    [SerializeField] public List<SoundItem> sounds;
    
    public void PlaySound(string soundName)
    {
        if (!Settings.Instance.soundToggle.isOn)
        {
            return;
        }
        SoundItem currentSound = sounds.FirstOrDefault(item => item.name == soundName);
        audioSource.PlayOneShot(currentSound?.source);
    }

    
}