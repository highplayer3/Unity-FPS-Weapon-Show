using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter :Singleton<EventCenter>
{
    //参数可以根据自己的需求改变
    public delegate void ProcessEventHandler(Object obj, int Param1, int Param2);

    private Dictionary<string, ProcessEventHandler> EventMap = new Dictionary<string, ProcessEventHandler>();

    //注册事件给EventCenter
    public void Regist(string name,ProcessEventHandler func)
    {
        if (EventMap.ContainsKey(name))
            EventMap[name] += func;
        else
            EventMap[name] = func;
    }
    //移除事件的注册
    public void UnRegist(string name,ProcessEventHandler func)
    {
        if (EventMap.ContainsKey(name))
        {
            EventMap[name] -= func;
        }
        else
        {
            return;
        }
    }
    //触发事件
    public void Trigger(string name,Object obj,int Param1,int Param2)
    {
        if (EventMap.ContainsKey(name))
        {
            EventMap[name].Invoke(obj, Param1, Param2);
        }
    }
}
