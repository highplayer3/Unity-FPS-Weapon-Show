using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GrenadeData grenadeData;
    public AudioSource audioSource;
    public AudioClip collisonAudio;
    
    private void OnCollisionEnter(Collision collision)
    {
        
        audioSource.clip = collisonAudio;
        audioSource.Play();
        float time = Time.time;
        StartCoroutine(DoExplode(time, grenadeData.explodeTime));
    }
    IEnumerator DoExplode(float startTime,float endTime)
    {
        while (Time.time - startTime < endTime)
        {
            yield return null;//防止阻塞线程
        }
        EventCenter.Instance.Trigger("Explosion", this.gameObject, 0, 0);
        GameObject obj = GameObject.Instantiate(grenadeData.explodeEffect, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(grenadeData.explodeAudio, this.transform.position);
        Destroy(this.gameObject);
        if (grenadeData.ItemName == "烟雾弹")
        {
            Destroy(obj, 7f);
        }
        else 
        { 
            Destroy(obj, 3f); 
        }
    }
}
