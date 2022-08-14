using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun
{
    public int MinShotBullet = 8;
    public int MaxShotBullet = 16;

    public float MaxShotRadius = 5f;
    /// <summary>
    /// 重写基类的虚函数
    /// </summary>
    protected override void Reload()
    {
        if (GunAttr != null && !m_isReloading)
        {
            if (m_BulletCount == GunAttr.CartridgeClip)
            {
                AudioSource.PlayClipAtPoint(DryFire, transform.position);
                Debug.Log("弹夹已满无需换弹！");
                return;
            }
            anim.SetTrigger("ReloadLeft");
            Play(GunAttr.ReloadSoundLeft);
            m_isReloading = true;
            StartCoroutine(PlayReload());
        }
    }
    IEnumerator PlayReload()
    {
        yield return new WaitForSeconds(0.3f);

        while (AnimatorIsPlaying("ReloadOpen"))
        {
            yield return null;
        }
        int nt = GunAttr.CartridgeClip - m_BulletCount;
        //容量有毒少，就播放多少次装弹
        for (int i = 0; i < nt; i++)
        {
            anim.SetTrigger("ReloadInsert");
            Play(GunAttr.ReloadSoundIn);
            yield return new WaitForSeconds(0.3f);

            while (AnimatorIsPlaying("ReloadInsert"))
            {
                yield return null;
            }
            m_BulletCount++;
            EventCenter.Instance.Trigger("SetBullet", null, m_BulletCount, GunAttr.CartridgeClip);//通知UI改变
        }
        anim.SetTrigger("ReloadOut");
        Play(GunAttr.ReloadSoundOut);
        yield return new WaitForSeconds(0.3f);
        while (AnimatorIsPlaying("ReloadClose"))
        {
            yield return null;
        }
        m_isReloading = false;
    }

    protected override void CreateBullet(Vector3 dir)
    {
        int count = UnityEngine.Random.Range(MinShotBullet, MaxShotBullet);
        for(int i = 0; i < count; i++)
        {
            Vector3 vPos = dir * GunAttr.EffectiveRange;
            vPos += MaxShotRadius * UnityEngine.Random.insideUnitSphere;
            base.CreateBullet(vPos.normalized);
        }
        
    }
}
