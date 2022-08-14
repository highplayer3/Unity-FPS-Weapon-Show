using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShow : MonoBehaviour
{
    public float RotateSpeed = 30f;
    private bool isLookAtWeapon=false;
    // Start is called before the first frame update
    void Start()
    {
        //当LookWeapon事件发生，将会调用OnLookWeapon去处理事件
        EventCenter.Instance.Regist("LookWeapon", OnLookWeapon);
        EventCenter.Instance.Regist("DontLookWeapon", OnDontLookWeapon);
    }
    void OnDontLookWeapon(Object obj,int param1,int param2)
    {
        if ((GameObject)obj == gameObject)
        {
            isLookAtWeapon=false;
        }
    }
    void OnLookWeapon(System.Object obj,int Para1,int Para2)
    {
        isLookAtWeapon = (GameObject)obj == gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (isLookAtWeapon)
        {
            return;
        }
        else
        {
            transform.Rotate(Vector3.up, Time.deltaTime * RotateSpeed); 
        }
        
    }
}
