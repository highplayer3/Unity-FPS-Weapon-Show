using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Animator m_TargetAnimator;
    private int health = 100;//靶子的生命值
    [SerializeField]
    private GameObject hitEffect;//击中靶子的特效
    [SerializeField]
    private AudioClip hitAudio;//击中靶子播放的声音

    private AudioSource audioSource;
    public AudioClip m_TargetDown;//靶子倒下的声音
    public AudioClip m_TargetUp;//靶子立起来的声音
    private bool isPlayingAudio = false;

    private HUDTextInfo m_hUDTextInfo;//击中靶子显示的文字

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_TargetAnimator = GetComponent<Animator>();
        //注册事件Hit,并使用OnHit函数处理Hit事件
        //EventCenter.Instance.Regist("Hit", OnHit);
        EventCenter.Instance.Regist("Explosion", OnExplodeJudge);
    }

    private void OnExplodeJudge(UnityEngine.Object obj, int Param1, int Param2)
    {
        GameObject m_obj = (GameObject)obj;
        if (m_obj.CompareTag("Bullet"))
        {
            Explode ep = m_obj.GetComponent<Explode>();
            if (ep == null)
            {
                return;
            }
            if ((this.gameObject.transform.position - m_obj.transform.position).magnitude > ep.ExplodeAreaRadius)
            {
                //Debug.Log(this.gameObject.name + "在爆炸范围之外");
            }
            else
            {
                float dis = (this.gameObject.transform.position - m_obj.transform.position).magnitude;
                OnHit(m_obj, ep.center_Damage - (int)(ep.decreaseCofficient * dis), 0);
            }
        }
        else if(m_obj.CompareTag("ExplodeBarrel"))
        {
            ExplosiveBarrel eb = m_obj.GetComponent<ExplosiveBarrel>();
            if ((this.transform.position - m_obj.transform.position).magnitude > eb.ExplodeRadius)
            {
                return;
            }
            else
            {
                float dis = (this.gameObject.transform.position - m_obj.transform.position).magnitude;
                OnHit(m_obj,(int)(30*dis), 0);
            }
        }
    
        
    }

    private void OnHit(UnityEngine.Object obj, int Param1, int Param2)
    {
        //0:头部，1:身体，2:腿部
        switch (Param2)
        {
            case 0:
                health = (health - Param1) >= 0 ? health - Param1 : 0;
                break;
            case 1:
                health = (health - Param1) >= 0 ? health - Param1 : 0;
                break;
            case 2:
                health = (health - Param1) >= 0 ? health - Param1 : 0;
                break;
        }
        #region HUD文字
        m_hUDTextInfo = new HUDTextInfo(((GameObject)obj).transform, string.Format("{1}{0}", Param1, "-"));
        m_hUDTextInfo.Color = Color.red;
        m_hUDTextInfo.Size = 50;
        m_hUDTextInfo.Speed = 0.6f;
        m_hUDTextInfo.VerticalAceleration = UnityEngine.Random.Range(-1, 1f);
        m_hUDTextInfo.VerticalPositionOffset = 1f;
        m_hUDTextInfo.VerticalFactorScale = UnityEngine.Random.Range(1.5f, 10);
        m_hUDTextInfo.Side = (UnityEngine.Random.Range(0, 2) == 1) ? bl_Guidance.LeftDown : bl_Guidance.RightDown;
        m_hUDTextInfo.ExtraDelayTime = -1;
        m_hUDTextInfo.AnimationType = bl_HUDText.TextAnimationType.PingPong;
        m_hUDTextInfo.FadeSpeed = 100;
        bl_UHTUtils.GetHUDText.NewText(m_hUDTextInfo);
        #endregion
        //特效声音部分
        GameObject effect = GameObject.Instantiate(hitEffect, ((GameObject)obj).transform.position,
            Quaternion.LookRotation(-this.transform.forward));
        effect.transform.SetParent(this.gameObject.transform);
        AudioSource.PlayClipAtPoint(hitAudio, ((GameObject)obj).transform.position);
        Destroy(effect, 1f);
        EventCenter.Instance.UnRegist("Hit", OnHit);
    }
    //靶子被击倒后重置
    private void Reset()
    {
        isPlayingAudio = false;
        audioSource.clip = m_TargetUp;
        audioSource.Play();
        this.health = 100;
        m_TargetAnimator.SetBool("IsFalled", false);
        m_TargetAnimator.SetTrigger("Reset");
    }
    private void Update()
    {
        if (health == 0)
        {
            m_TargetAnimator.SetBool("IsFalled",true);
            if (!isPlayingAudio)
            {
                audioSource.clip = m_TargetDown;
                audioSource.Play();
                isPlayingAudio = true;
            }
            
        }
    }
    //注册事件
    void ActivateEvent()
    {
        EventCenter.Instance.Regist("Hit", OnHit);
    }
    
}
