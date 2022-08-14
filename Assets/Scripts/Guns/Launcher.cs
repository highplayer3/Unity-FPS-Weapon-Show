using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Gun
{
    public GameObject m_Bullet;
    public float ReloadWaitTime = 0.5f;
    public bool IsOpposite = false;
    protected override void CreateBullet(Vector3 dir)
    {
        base.CreateBullet(dir);
        if (m_Bullet != null)
        {
            m_Bullet.SetActive(IsOpposite);
        }
    }
    protected override void Reload()
    {
        if (GunAttr != null && !m_isReloading && m_BulletCount == 0) 
        {
            anim.SetTrigger("ReloadLeft");
            Play(GunAttr.ReloadSoundLeft, 2);

            m_isReloading = true;
            StartCoroutine(PlayReload());
        }
    }

    IEnumerator PlayReload()
    {
        yield return new WaitForSeconds(ReloadWaitTime);
        if (m_Bullet != null)
        {
            m_Bullet.SetActive(!IsOpposite);
        }
        while (m_isReloading)
        {
            m_isReloading = AnimatorIsPlaying("ReloadLeft");
            if (!m_isReloading)
            {
                SetBullet(GunAttr.CartridgeClip);
            }
            yield return null;
        }
    }
}
    
