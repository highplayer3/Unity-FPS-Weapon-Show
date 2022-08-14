using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    public AudioClip[] ExplodeAudios;
    public GameObject ExplodeEffect;

    public int center_Damage = 150;//爆炸中心伤害
    public float decreaseCofficient = 25f;//每1米衰减的伤害
    public float ExplodeAreaRadius = 5.0f;//爆炸范围
    public float ExplodeForce = 500f;
    public void Explosion()
    {
        Vector3 pos = transform.position;
        if (ExplodeAudios.Length > 0)
        {
            AudioSource.PlayClipAtPoint(ExplodeAudios[Random.Range(0, ExplodeAudios.Length)], pos);

        }
        if (ExplodeEffect != null)
        {
            GameObject obj = GameObject.Instantiate(ExplodeEffect, pos, transform.localRotation);
            Destroy(obj, 5f);
        }
        //爆炸效果
        if (ExplodeAreaRadius > 0)
        {
            EventCenter.Instance.Trigger("Explosion", this.gameObject, 0, 0);
            Collider[] colliders=Physics.OverlapSphere(pos, ExplodeAreaRadius);
            for(int i = 0; i < colliders.Length; i++)
            {
                float dis = (colliders[i].transform.position - transform.position).magnitude;
                
                Rigidbody rig = colliders[i].GetComponent<Rigidbody>();
                if (rig != null)
                {
                    //炸飞的效果
                    rig.AddExplosionForce(ExplodeForce, pos, ExplodeAreaRadius);
                }
            }
        }
    }
}
