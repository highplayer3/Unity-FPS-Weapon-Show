using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    [HideInInspector]
    public CharacterController m_characterController;
    private float h;
    private float v;
    private AudioSource m_AudioSource;
    [SerializeField]
    private float walkSpeed = 3.0f;
    [SerializeField]
    private float runSpeed = 6.0f;
    private Animator anim;
    [SerializeField]
    private float JumpHeight = 1.2f;
    private float gravity = 20f;
    private Vector3 velocity = Vector3.zero;

    public float StandStepTime = 0.6f;
    private float m_StandStepTimer = 0f;
    private Material material;//角色脚下的材质
    public FootSoundData footSoundData;
    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_AudioSource = GetComponent<AudioSource>();
        EventCenter.Instance.Regist("EquipmentItem", OnEquipmentItem);
    }

    private void OnEquipmentItem(Object obj, int Param1, int Param2)
    {
        anim=((GameObject)obj).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform组件的移动不会做任何碰撞检测
        //transform.Translate(transform.forward * Time.deltaTime);
        Move_Update();
        Height_Update();
        m_characterController.Move(velocity * Time.deltaTime);
        Sound_Update();
    }
    //角色移动
    private void Move_Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * h + transform.forward * v;
        float fSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            fSpeed = runSpeed;
        }
        direction *= fSpeed;
        velocity.x = direction.x;
        velocity.z = direction.z;
        if (anim != null)
        {
            anim.SetFloat("Speed", direction.magnitude / runSpeed);
        }
    }
    //脚步声播放
    void Sound_Update()
    {
        if (m_characterController.isGrounded)
        {
            Vector3 vT = velocity;
            vT.y = 0f;
            float len = vT.magnitude;
            if (len > 0)
            {
                m_StandStepTimer += Time.deltaTime;
                if (m_StandStepTimer > StandStepTime)
                {
                    AudioClip ac = footSoundData.GetSoundByMaterial(material);
                    m_AudioSource.clip = ac;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        m_AudioSource.pitch = 2f;
                    }
                    else
                    {
                        m_AudioSource.pitch = 1f;
                    }
                    m_AudioSource.Play();

                    m_StandStepTimer = 0f;
                }
                
            }
        }
        
    }
    //重力实现
    void Height_Update()
    {
        if (m_characterController.isGrounded)
        {
            if (velocity.y < -1f)
            {
                velocity.y = -1f;
            }
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(2 * gravity * JumpHeight);
            }
        }
        velocity.y -= gravity * Time.deltaTime;
    }
    //角色控制器的碰撞回调函数
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Plane"))
        {
            //获取默认材质，若有多个材质球，则采用数组的形式获取对应材质球
            material = hit.collider.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
        }
    }
}
