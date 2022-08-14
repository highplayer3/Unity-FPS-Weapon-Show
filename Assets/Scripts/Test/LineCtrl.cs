using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCtrl : MonoBehaviour
{
    public LineRenderer line;
    public Transform sphere;
    public float interval;//每隔多久计算一次坐标点
    public float Velocity;//斜抛的初速度
    private bool hasGetGun;
    private bool hasGetGrenade;
    private GameObject gre;
    private Animator GunPrefab;
    public int GrenadeCount = 0;
    // Start is called before the first frame update

    void Start()
    {
        EventCenter.Instance.Regist("EquipmentItem", OnEquipmentItem);
        EventCenter.Instance.Regist("PickUpGrenade", OnPickUpGrenade);
        //EventCenter.Instance.Regist("ThrowGrenade", OnThrowGrenade);
    }

    //private void OnThrowGrenade(UnityEngine.Object obj, int Param1, int Param2)
    //{
        
    //}

    private void OnPickUpGrenade(UnityEngine.Object obj, int Param1, int Param2)
    {
        
        if (Param1 > 0)
        {
            GrenadeCount = Param1;
            hasGetGrenade = true;
        }
        else
        {
            hasGetGrenade = false;
        }
        gre = ((GrenadeData)obj).ItemEqquipmentPrefab;
    }

    private void OnEquipmentItem(UnityEngine.Object obj, int Param1, int Param2)
    {
        hasGetGun = true;
        GunPrefab = ((GameObject)obj).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasGetGun && hasGetGrenade&&GrenadeCount>0)
        {
            if (Input.GetKey(KeyCode.T))
            {
                //GunPrefab.SetTrigger("ThrowGrenade");
                line.gameObject.SetActive(true);
                sphere.gameObject.SetActive(true);
                List<Vector3> list = GetPointPos();
                line.positionCount = list.Count;
                line.SetPositions(list.ToArray());
                sphere.position = list[list.Count - 1];
                //StartCoroutine(ThrowGrenadeProcess());
            }
            if (Input.GetKeyUp(KeyCode.T))
            {
                GrenadeCount--;
                GunPrefab.speed = 1;
                line.gameObject.SetActive(false);
                sphere.gameObject.SetActive(false);
                EventCenter.Instance.Trigger("ThrowGrenade", null, GrenadeCount, 0);
                Instantiate(gre, transform.position, transform.rotation).GetComponent<Rigidbody>().velocity = Velocity * transform.forward;
                EventCenter.Instance.Trigger("ThrowFinish", null, 0, 0);//告诉Gun投掷过程结束了
            }
        }
        else
        {
            if(GunPrefab!=null)
                GunPrefab.speed = 1;
            line.gameObject.SetActive(false);
            sphere.gameObject.SetActive(false);
        }
        
    }
    List<Vector3> GetPointPos()
    {
        List<Vector3> list = new List<Vector3>();
        //水平方向
        Vector3 horizontalDir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        //Debug.DrawLine(transform.position, transform.position + transform.forward*3);
        //Debug.DrawLine(transform.position, transform.position + horizontalDir * 3);
        float angle = Vector3.Angle(transform.forward, horizontalDir);//斜抛的角度
        float horizontalSpeed = Mathf.Cos(angle / 180 * Mathf.PI) * Velocity;//水平速度不变
        float verticalSpeed = Mathf.Sin(angle/180*Mathf.PI) * Velocity*(transform.forward.y>0?1:-1);

        //Debug.Log(horizontalSpeed + "-" + verticalSpeed+"-"+angle);
        for(int i = 0; i < 1000; i++)
        {
            Vector3 pos = transform.position + (horizontalSpeed * interval * i * horizontalDir) + ((verticalSpeed + (verticalSpeed + Physics.gravity.y* interval * i))/2 * interval * i * Vector3.up);
            //Debug.Log(pos);
            list.Add(pos);
            if (i > 0)
            {
                RaycastHit hit;
                Vector3 dir = list[list.Count - 1] - list[list.Count - 2];
                if(Physics.Raycast(list[list.Count-2],dir,out hit, dir.magnitude))
                {
                    list[list.Count - 1] = hit.point;
                    break;
                }
                 
            }
        }
        return list;
    }

    IEnumerator ThrowGrenadeProcess()
    {
        yield return new WaitForSeconds(0.2f);
        GunPrefab.speed = 0;
    }
}
