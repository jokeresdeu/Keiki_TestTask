using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoundMap", menuName = "SoundMap/SoundMap", order = 1)]
public class SoundMap : ScriptableObject
{
    [SerializeField] private VariableSounds[] _variableSounds;
    [SerializeField] private SingularAnimalSounds[] _singularAnimalSounds;

    public VariableSounds[] VariableSounds => _variableSounds;
    public SingularAnimalSounds[] SingularAnimalSounds => _singularAnimalSounds;
}


public abstract class SoundsBase
{
    [SerializeField] private SoundId _soundId;
    public SoundId SoundId => _soundId;
   
}

[Serializable]
public class SingularAnimalSounds: SoundsBase
{
    [SerializeField] private AnimalSound[] _sounds;
    public AnimalSound[] Sounds => _sounds;
}

[Serializable]
public class VariableSounds: SoundsBase
{
    [SerializeField] protected AudioClip[] _clips;
    public AudioClip[] Clips => _clips;
}

[Serializable]
public class AnimalSound
{
    [SerializeField] private AnimalId _animalId;
    [SerializeField] private AudioClip _adioClip;

    public AnimalId AnimalId => _animalId;
    public AudioClip AudioClip => _adioClip;
}
