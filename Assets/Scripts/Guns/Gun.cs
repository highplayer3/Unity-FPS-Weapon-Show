using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有枪械都有的功能实现
/// </summary>
public class Gun : MonoBehaviour
{
    protected GameObject m_Owner = null;//拿着枪的角色
    public Transform FirePos;
    public Transform CasingPos;
    public Transform CameraPos;
    public ParticleSystem FireParticle;//开火特效
    [HideInInspector]
    public GunData GunAttr;//获取枪械所属的属性
    protected Animator anim;
    protected float FireTime = 0f;
    protected int m_BulletCount = 0;
    protected bool m_isReloading = false;
    protected bool m_isAiming = false;
    protected int m_aimPos = 0;
    protected Camera playerCamera;
    protected float m_playerCameraFOV;
    protected bool isThrowing = false;
    public GameObject grenadeMatchBody;
    #region 音效
    [SerializeField]
    protected AudioClip DryFire;//子弹已满或已空的音效
    public AudioSource m_audioSource1;
    public AudioSource m_audioSource2;
    public AudioClip ThrowFire;

    private LineCtrl m_ctrl;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        m_ctrl = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<LineCtrl>();
        
    }
    private void Start()
    {
        //EventCenter.Instance.Regist("TellThePlayer", OnAction);
        EventCenter.Instance.Regist("ThrowFinish", OnThrowFinish);
    }

    private void OnThrowFinish(UnityEngine.Object obj, int Param1, int Param2)
    {
        isThrowing = false;
    }


    protected void Play(AudioClip _clip,int channel=1)//默认用通道一来播放
    {
        if (channel == 1)
        {
            m_audioSource1.clip = _clip;
            m_audioSource1.Play();
        }
        else
        {
            m_audioSource2.clip = _clip;
            m_audioSource2.Play();
        }
        
    }
    public void Init(GunData gunData,Camera ca,Camera sca,GameObject owner)
    {
        this.GunAttr = gunData;
        if (GunAttr != null)
        {
            SetBullet(GunAttr.CartridgeClip);
            Play(GunAttr.DrawSound);
            m_playerCameraFOV = GunAttr.FOVForNormal;

            playerCamera = ca;
            if (CameraPos != null && sca != null)
            {
                sca.transform.SetParent(CameraPos);
            }
            m_Owner = owner;
        }
    }
    public void SetBullet(int n)
    {
        
        if (GunAttr != null)
        {
            if (n > GunAttr.CartridgeClip)
            {
                n = GunAttr.CartridgeClip;
            }
        }
        m_BulletCount = n;
        EventCenter.Instance.Trigger("SetBullet", null, m_BulletCount, GunAttr.CartridgeClip);
    }
    // Update is called once per frame
    void Update()
    {
        FireTime += Time.deltaTime;
        //如果可以连续射击
        if (GunAttr != null)
        {
            if (GunAttr.FireAway)
            {
                if (Input.GetButton("Fire1"))
                {
                    //1/RateOfFire即为时间
                    if (FireTime >= 1.0f/GunAttr.RateOfFire)
                    {
                        Fire();
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Fire();
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                ThrowGrenade();
            }
            Aim_Update();
        }
    }
    protected void Aim_Update()
    {
        if (isThrowing||m_isReloading) return;
        if (Input.GetButtonDown("Fire2"))
        {
            m_isAiming = true;
            anim.SetFloat("AimPos", m_aimPos);
            anim.SetBool("IsAiming", m_isAiming);
            m_playerCameraFOV = GunAttr.FOVForAim;
            EventCenter.Instance.Trigger("Aim", null, 1, 0);
            Play(GunAttr.AimSound,2);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            m_isAiming = false;
            anim.SetBool("IsAiming", m_isAiming);
            m_playerCameraFOV = GunAttr.FOVForNormal;
            EventCenter.Instance.Trigger("Aim", null, 0, 0);
        }
        if(playerCamera!=null)
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, m_playerCameraFOV, Time.deltaTime * 6);
    }
    /// <summary>
    /// 开火
    /// </summary>
    protected void Fire()
    {
        if (m_isReloading||isThrowing)
        {
            return;
        }
        //每次发射都会把射击间隔清零
        FireTime = 0f;
        if (GunAttr!=null)
        {
            //有子弹
            if (m_BulletCount > 0)
            {
                anim.SetTrigger("Fire");
                if (FireParticle != null)
                {
                    FireParticle.Play();
                }
                Play(GunAttr.FireSound);
                CreateBullet(FirePos.forward);
                CreateBulletCasing();
                --m_BulletCount;
                EventCenter.Instance.Trigger("Fire", GunAttr, m_BulletCount, GunAttr.CartridgeClip);
            }
            else
            {
                Play(GunAttr.NoBulletSound);
            }
            
            //EventCenter.Instance.Trigger("SetBullet", null, m_BulletCount, GunAttr.CartridgeClip);
        }
    }
    protected bool AnimatorIsPlaying(string name)
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        return info.IsName(name)&& info.normalizedTime < 1.0f;
    }
    protected virtual void Reload()
    {
        if (m_isReloading||isThrowing)
        {
            return;
        }
        if (GunAttr != null)
        {
            if (m_BulletCount == GunAttr.CartridgeClip)
            {
                AudioSource.PlayClipAtPoint(DryFire, transform.position);
                Debug.Log("弹夹已满无需换弹！");
                return;
            }
            if (m_BulletCount > 0)
            {
                Play(GunAttr.ReloadSoundLeft,2);
                anim.SetTrigger("ReloadLeft");
            }
            else
            {
                Play(GunAttr.ReloadSoundOut,2);
                anim.SetTrigger("ReloadOut");
            }
        }
        m_isReloading = true;
        StartCoroutine(Reloading());
    }
    protected IEnumerator Reloading()
    {
        //让动画执行1秒，然后判断
        yield return new WaitForSeconds(1.0f);
        while (m_isReloading)
        {
            //判断换弹夹的动画是否播放完
            m_isReloading = AnimatorIsPlaying("ReloadLeft") || AnimatorIsPlaying("ReloadOut");
            if (!m_isReloading)
            {
                SetBullet(GunAttr.CartridgeClip);
            }
            yield return null;//防止游戏卡顿，卡在循环里，把它分到很多帧里。
        }
    }
    protected virtual void CreateBullet(Vector3 dir)
    {
        if (GunAttr != null&&GunAttr.BulletPrefab != null)
        {
            //创建子弹
            GameObject obj = GameObject.Instantiate(GunAttr.BulletPrefab,FirePos);
            obj.transform.SetParent(null);
            //子弹发射
            obj.GetComponent<Rigidbody>().AddForce(dir * GunAttr.BulletDamage);
            obj.GetComponent<Bullet>().Init(m_Owner, GunAttr.EffectiveRange);
            
            Destroy(obj, 5f);
        }
    }
    protected void CreateBulletCasing()
    {
        if (GunAttr != null && GunAttr.CasingPrefab != null)
        {
            //创建子弹
            GameObject obj = GameObject.Instantiate(GunAttr.CasingPrefab, CasingPos);
            obj.transform.SetParent(null);

            Rigidbody rig = obj.GetComponent<Rigidbody>();
            obj.GetComponent<Rigidbody>().AddForce((CasingPos.right+CasingPos.up) * 100);
            if (m_Owner != null)
            {
                //速度加上角色移动的速度
                rig.velocity+=m_Owner.GetComponent<CharacterController>().velocity;
            }
            Destroy(obj, 5f);
        }
    }

    private void ThrowGrenade()
    {
        if (isThrowing||m_ctrl.GrenadeCount<=0)
        {
            return;
        }
        isThrowing = true;
        Play(ThrowFire, 1);
        //m_Owner.GetComponent<ItemMgr>().grenadeNumber--;
        //EventCenter.Instance.Trigger("ThrowGrenade",null, m_Owner.GetComponent<ItemMgr>().grenadeNumber,0);//触发扔手雷事件

        anim.SetTrigger("ThrowGrenade");
        StartCoroutine(ThrowGrenadeProcess());
    }
    IEnumerator ThrowGrenadeProcess()
    {
        yield return new WaitForSeconds(0.3f);
        anim.speed = 0;
        //GameObject gre = GameObject.Instantiate(throwObj, grenadeMatchBody.transform);
        //gre.transform.SetParent(null);
        //gre.GetComponent<Rigidbody>().AddForce(throw_velocity * (m_Owner.transform.forward + m_Owner.transform.up).normalized);
        //yield return new WaitForSeconds(interval);
        //isThrowing = false;
    }
}
