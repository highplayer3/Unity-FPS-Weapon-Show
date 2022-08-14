using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpactEffect
{
    public Material material;
    public GameObject Effect;
}
public class Bullet : MonoBehaviour
{
    public AudioClip[] ImpactAudios;//子弹击中音效

    public ImpactEffect[] EffectList;
    [HideInInspector]
    public Vector3 m_StartPos;//子弹的起始位置，用于计算射程
    
    private float m_MaxDistance;//子弹射程
    [HideInInspector]
    public GameObject m_Owner;//子弹由谁发出来的

    public void Init(GameObject owner,float maxdistance)
    {
        m_Owner = owner;
        m_MaxDistance = maxdistance;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Explode ep = GetComponent<Explode>();
        if (ep != null)
        {
            ep.Explosion(); 
        }
        else
        {
            if (ImpactAudios.Length > 0)
            {
                AudioSource.PlayClipAtPoint(ImpactAudios[Random.Range(0, ImpactAudios.Length)], transform.position);
            }
            if (collision.gameObject.GetComponent<MeshRenderer>() == null)
            {
                return;
            }
            Material ImpactMaterial = collision.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            for (int i = 0; i < EffectList.Length; i++)
            {
                if (EffectList[i].material == ImpactMaterial)
                {
                    GameObject effect = GameObject.Instantiate(EffectList[i].Effect, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
                    Destroy(effect, 5.0f);
                }
            }
        }
        gameObject.SetActive(false);

    }
    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - m_StartPos).magnitude > m_MaxDistance)
        {
            gameObject.SetActive(false);
            Debug.Log(m_Owner);
        }
    }
}
