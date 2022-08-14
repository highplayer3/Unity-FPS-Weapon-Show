using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    #region 单例
    private static AudioMgr instance;
    public static AudioMgr Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.GetComponent<AudioMgr>();
            }
            return instance;
        }
    }
    #endregion
    private Dictionary<GameObject, AudioClip> audioInfoDic;
    private AudioSource audioSource;
    private void Start()
    {
        audioInfoDic = new Dictionary<GameObject, AudioClip>();
    }
    private void Update()
    {
       
    }
    /// <summary>
    /// 外部调用的接口
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_clip"></param>
    public void AddListen(GameObject _obj,AudioClip _clip)
    {
        if (audioInfoDic.ContainsKey(_obj))
        {
            return;
        }
        audioInfoDic.Add(_obj, _clip);
    }
    private IEnumerator PlayAudio()
    {
        foreach(var item in audioInfoDic)
        {
            if (item.Key.GetComponent<AudioSource>() == null)
            {
                audioSource=item.Key.AddComponent<AudioSource>();
                audioSource.clip = item.Value;
                audioSource.loop = true;
            }
            else
            {
                continue;
            }
            yield return null;
        }
    }
}
