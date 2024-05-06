using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    
    public AudioSource audioSource;

    private int audioIndex = 0;
    
    public void Init()
    {
        PlaySound(0);
    }

    public void PlaySound(int index)
    {
        audioIndex = index;
        audioSource.clip = audioClips[audioIndex];
        audioSource.time = 0;
        audioSource.Play();
        //audioSource.PlayOneShot(audioClips[audioIndex]);
    }

    public void Btn_click()
    {
        if (audioIndex.Equals(0))
        {
            PlaySound(1);
        }
        else
        {
            PlaySound(0);
        }
    }

    // private bool noAudio = false;
    public void Slider_Audio(float value)
    {
        audioSource.volume = value;
        if (value>0.01f)
        {
            audioSource.mute =false;
            muteImage.sprite = audioSource.mute ? muteSprite[0] : muteSprite[1];
        }
    }


    public Image muteImage;
    public Sprite[] muteSprite;
    public void Mute_Click()
    {
        audioSource.mute = !audioSource.mute;
        muteImage.sprite = audioSource.mute ? muteSprite[0] : muteSprite[1];
    }
}
