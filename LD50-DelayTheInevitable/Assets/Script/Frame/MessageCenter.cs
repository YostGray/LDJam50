using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum eMsgType
{
    LanguageChange = 1,
    PlayerDead = 2,
}

public class MessageCenter:Singleton<MessageCenter>
{
    Dictionary<eMsgType, List<Action<object[]>>> ListernerDic = new Dictionary<eMsgType, List<Action<object[]>>>();

    public void AddListerner(eMsgType msgType, Action<object[]> action)
    {
        if (ListernerDic.ContainsKey(msgType))
        {
            if (ListernerDic[msgType].Contains(action))
            {
                Debug.LogWarning("重复添加消息监听："+ msgType);
                return;
            }
            ListernerDic[msgType].Add(action);
        }
        else
        {
            ListernerDic.Add(msgType, new List<Action<object[]>>());
            ListernerDic[msgType].Add(action);
        }
    }

    public void RemoveListerner(eMsgType msgType, Action<object[]> action)
    {
        if (ListernerDic.ContainsKey(msgType))
        {
            if (!ListernerDic[msgType].Contains(action))
            {
                Debug.LogWarning("移除不存在的消息监听："+ msgType);
                return;
            }
            ListernerDic[msgType].Remove(action);
        }
    }

    public void BroadcastMsg(eMsgType msgType, object[] args = null)
    {
        if (ListernerDic.ContainsKey(msgType))
        {
            foreach (Action<object[]> act in ListernerDic[msgType])
            {
                act.Invoke(args);
            }
        }
    }
}