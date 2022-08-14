using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SoundData
{
    public Material material;
    public AudioClip[] SoundList;
}
[CreateAssetMenu(menuName ="MyFPS Data/Foot Sound Data")]
public class FootSoundData : ScriptableObject
{
    public SoundData[] FootSoundList;

    public AudioClip GetSoundByMaterial(Material _material)
    {
        foreach(var sd in FootSoundList)
        {
            if (sd.material == _material)
            {
                return sd.SoundList[Random.Range(0, sd.SoundList.Length)];
            }
        }
        return null;
    }
}
    
