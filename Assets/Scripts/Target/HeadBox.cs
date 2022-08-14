﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBox : MonoBehaviour
{
    private GunData gunData;//获取发出子弹的枪的属性，用于计算伤害
    private Bullet m_bullet;//获取击中靶子的子弹
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            SendMessageUpwards("ActivateEvent");
            //子弹伤害计算
            m_bullet = collision.gameObject.GetComponent<Bullet>();
            gunData = m_bullet.m_Owner.GetComponentInChildren<Gun>().GunAttr;
            float distance = (m_bullet.transform.position - m_bullet.m_StartPos).magnitude;
            distance /= 20;
            int damage = (int)(gunData.BulletDamage * 0.08) + (int)distance * Random.Range(-5, 0);

            //触发事件，然后交给事件处理函数去处理
            EventCenter.Instance.Trigger("Hit", collision.gameObject, damage, 0);
        }
        else
        {
            return;
        }
        
    }
}
