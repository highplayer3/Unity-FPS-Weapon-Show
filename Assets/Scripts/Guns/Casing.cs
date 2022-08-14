using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//弹壳的音效
public class Casing : MonoBehaviour
{
    public AudioClip[] CasingAudios;

    private void OnCollisionEnter(Collision collision)
    {
        if (CasingAudios.Length > 0)
        {
            AudioSource.PlayClipAtPoint(CasingAudios[Random.Range(0, CasingAudios.Length)], transform.position);
            //为什么Gun里面用的添加AudioSource，因为上面这个方法的原理就是在目标位置创建一个AudioSource
        }
        
    }
}
