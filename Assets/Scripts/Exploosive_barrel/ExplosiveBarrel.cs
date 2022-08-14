using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public float ExplodeRadius = 8f;
    private Bullet m_Bullet;
    private Gun gun;
    private Rigidbody rig;
    private int health = 50;
    private AudioSource audioSource;
    public GameObject effect;//爆炸后的特效
    public AudioClip boomSound;//爆炸后逇音效
    public GameObject destoryObj;//油桶被引爆后的物体
    public GameObject[] remains;//爆炸后的残余物
    private bool hasExploded = false;//是否已经爆炸了

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        EventCenter.Instance.Regist("Explosion", OnExplodeJudge);
    }
    //对爆炸事件的处理，参数obj即产生爆炸的物体
    private void OnExplodeJudge(UnityEngine.Object obj, int Param1, int Param2)
    {
        //如果是自身爆炸就不用响应
        if (((GameObject)obj) == this.gameObject)
        {
            return;
        }
        else if(((GameObject)obj).CompareTag("ExplodeBarrel"))
        {
            //如果油桶在爆炸范围之外，就不需要响应这个爆炸事件
            if (Vector3.Distance(((GameObject)obj).transform.position, transform.position)> ((GameObject)obj).GetComponent<ExplosiveBarrel>().ExplodeRadius)
            {
                return;
            }
            else
            {
                //否则油桶会受到伤害，伤害值与距离爆炸中心范围成正比
                health -= (int)Vector3.Distance(((GameObject)obj).transform.position, transform.position) * 20;
            }    
        }
        //如果是子弹爆炸(火箭筒等)
        else if (((GameObject)obj).CompareTag("Bullet"))
        {
            Explode ep = ((GameObject)obj).GetComponent<Explode>();
            if (ep == null)
            {
                return;
            }
            if ((this.gameObject.transform.position - ((GameObject)obj).transform.position).magnitude > ep.ExplodeAreaRadius)
            {
                //Debug.Log(this.gameObject.name + "在爆炸范围之外");
            }
            else
            {
                health -= ep.center_Damage-(int)(Vector3.Distance(((GameObject)obj).transform.position, transform.position) * ep.decreaseCofficient);
            }
        }else if (((GameObject)obj).CompareTag("Grenade"))
        {
            if (Vector3.Distance(((GameObject)obj).transform.position, transform.position) > ((GameObject)obj).GetComponent<Grenade>().grenadeData.EffectRadius)
            {
                return;
            }
            else
            {
                health -= (int)(Vector3.Distance(((GameObject)obj).transform.position, transform.position) * ((GameObject)obj).GetComponent<Grenade>().grenadeData.AttenuationCofficient);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.gameObject.GetComponent<Bullet>() != null)
            {
                m_Bullet = collision.gameObject.GetComponent<Bullet>();
                gun = m_Bullet.m_Owner.GetComponentInChildren<Gun>();
            }
            if (collision.gameObject.GetComponent<Explode>() == null)
            {
                //子弹如果不能爆炸的话，排除火箭筒的子弹
                var direction = gun.FirePos.forward;
                health -= (int)(gun.GunAttr.BulletDamage / 10);
                if(health>0)
                    rig.AddForce(direction * gun.GunAttr.BulletDamage);
            }
            else
            {
                
            }
        }
        
    }
    void HasDestory()
    {
        var pos = this.gameObject.transform.position;
        gameObject.SetActive(false);
        GameObject eb = GameObject.Instantiate(destoryObj, pos, Quaternion.identity);
        for(int i = 0; i < remains.Length; i++)
        {
            GameObject rm = GameObject.Instantiate(remains[i],pos,Quaternion.identity);
            rm.GetComponent<Rigidbody>().AddExplosionForce(600, pos, ExplodeRadius);
        }
    }
    private void Update()
    {
        if (health <= 0&&!hasExploded)
        {
            audioSource.clip = boomSound;
            GameObject effects = GameObject.Instantiate(effect, this.transform.position, Quaternion.identity);
            audioSource.Play();
            //油桶爆炸也会触发爆炸事件
            EventCenter.Instance.Trigger("Explosion", this.gameObject, 0, 0);
            Destroy(effects, 5f);
            hasExploded = true;
            HasDestory();
        }
        
    }
}
