using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="MyFPS Data/Gun Data")]
public class GunData : ItemData
{
    public enum GunType
    {
        GT_Rifle,//步枪
        GT_HandGun,//手枪
        GT_ShotGun,//散弹枪
        GT_Sniper,//狙击枪
        GT_Launcher//火箭筒
    }
    public GunType SubType;
    [Header("弹夹容量")]
    public int CartridgeClip;
    [Header("有效射程")]
    public float EffectiveRange;
    [Header("是否可以连续射击")]
    public bool FireAway;
    [Header("射速")]
    public float RateOfFire;
    
    [Header("子弹伤害程度")]
    public float BulletDamage;
    [Header("后坐力")]
    public float Recoil;

    public GameObject BulletPrefab;
    [Header("弹壳预制体")]
    public GameObject CasingPrefab;
    [Header("瞄准视角")]
    public float FOVForAim;
    [Header("正常视角")]
    public float FOVForNormal;
    [Header("开火音效")]
    public AudioClip FireSound;
    [Header("开火但是没子弹的音效")]
    public AudioClip NoBulletSound;
    [Header("瞄准音效")]
    public AudioClip AimSound;
    [Header("装弹音效")]
    public AudioClip ReloadSoundLeft;
    public AudioClip ReloadSoundOut;
    public AudioClip ReloadSoundIn;
    [Header("拿起武器音效")]
    public AudioClip DrawSound;
}
