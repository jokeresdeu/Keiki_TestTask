using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager 
{
    private SoundMap _soundMap;
    private AudioSource _currentAudioSourse;

    public AudioManager(SoundMap soundMap)
    {
        _soundMap = soundMap;
    }

    public void CreateAudiosourseForLvl(Transform transform)
    {
        _currentAudioSourse = transform.gameObject.AddComponent<AudioSource>();
    }

    public void PlayRandomSound(SoundId soundId, bool loop = false)
    {
        AudioClip[] clips = _soundMap.VariableSounds.First(sound => sound.SoundId == soundId).Clips;
        AudioClip clip = clips[Random.Range(0, clips.Length - 1)];
        PlaySound(clip, _currentAudioSourse, loop);
    }

    public void PlayAnimalSound(AudioSource audioSourse, AnimalId animalId, SoundId soundId, bool loop = false)
    {
        AudioClip clip = _soundMap.SingularAnimalSounds.First(sound => sound.SoundId == soundId).Sounds.First(sound => sound.AnimalId == animalId).AudioClip;
        PlaySound(clip, audioSourse, loop);
    }

    private void PlaySound(AudioClip audioClip, AudioSource sourse, bool loop)
    {
        if (sourse.isPlaying)
        {
            return;
        }
            
        sourse.clip = audioClip;
        sourse.loop = loop;
        sourse.Play();
    }
}




[System.Serializable]
public class Sound
{
    public string name;

    [HideInInspector]
    public AudioSource sourse;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;

}