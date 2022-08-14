using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "MyFPS Data/Grenade Data")]
public class GrenadeData : ItemData
{
    public enum GrenadeType
    {
        GT_Hand_grenade,
        GT_FlashBang,
        GT_Smoke_Grenade,
    }
    [Header("手榴弹类型")]
    public GrenadeType SunType;
    [Header("爆炸中心伤害")]
    public float CenterHurt;
    [Header("爆炸范围")]
    public float EffectRadius;
    [Header("爆炸伤害衰减(每米)")]
    public float AttenuationCofficient;
    [Header("投掷速度")]
    public float ThrowSpeed;
    [Header("手雷爆炸音效")]
    public AudioClip explodeAudio;
    [Header("手雷爆炸特效")]
    public GameObject explodeEffect;
    [Header("手雷最大拾取数量")]
    public int maxNumber;
    [Header("爆炸触发时间")]
    public float explodeTime;
}
