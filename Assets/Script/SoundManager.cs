using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    //Queue of audios added and play in order
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] int currentClipIndex = 0;
    [SerializeField] Slider soundSlider;
    //List of audio available, 1 for music, 2 for sfx sound
    [SerializeField]
    public List<SoundAudio> soundAudios;
    //Transistion speed
    float transistionSecondSpeedClose = 1f;
    float transistionSecondSpeedOpen = 1f;
    bool isTransistionProcess = false;
    //Transistion counter
    float transistionSecondCounter = 0;
    //Save the previous sound volume set when start transistion the volume
    float prevSoundVolume = 0;
    [SerializeField] AudioSource audioSource; //Music audio source
    [SerializeField] AudioSource audioSource2; //SFX audio source
    // Start is called before the first frame update
    void Start()
    {

    }
    //Play the next song (transistioning), if flag is false then no next song will be played
    public void PlayNextSong(bool flag)
    {
        if (!flag)
        {
            currentClipIndex = -1;
        }
        else currentClipIndex++;
        if (currentClipIndex >= audioClips.Count)
        {
            currentClipIndex = 0;
        }
        StartCoroutine(TransistionSound(currentClipIndex == -1 ? null : audioClips[currentClipIndex]));
    }
    public void PlaySFX(AudioClip audioClip)
    {
        audioSource2.PlayOneShot(audioClip);
    }

    void Update()
    {
        audioSource.volume = (float)soundAudios[0].volume / 100f;
        audioSource2.volume = (float)soundAudios[1].volume / 100f;
        //Keep playing when sound is stopped
        if (!audioSource.isPlaying && isTransistionProcess == false)
        {
            PlayNextSong(true);
        }
    }


    IEnumerator TransistionSound(AudioClip nextAudioClip)
    {
        isTransistionProcess = true; ;
        transistionSecondCounter = audioSource.volume;
        prevSoundVolume = audioSource.volume;
        while (transistionSecondCounter != 0)
        {
            transistionSecondCounter = Mathf.MoveTowards(transistionSecondCounter, 0, Time.deltaTime * transistionSecondSpeedClose);
            audioSource.volume = transistionSecondCounter;
            yield return null;
        }
        audioSource.Stop();
        if (nextAudioClip != null)
            audioSource.PlayOneShot(nextAudioClip);
        while (transistionSecondCounter != prevSoundVolume)
        {
            transistionSecondCounter = Mathf.MoveTowards(transistionSecondCounter, prevSoundVolume, Time.deltaTime * transistionSecondSpeedOpen);
            audioSource.volume = transistionSecondCounter;
            yield return null;
        }
        isTransistionProcess = false;
    }

}
[Serializable]
public class SoundAudio
{
    public string name;
    public int volume;
}
