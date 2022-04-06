using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExText : Text,IMultLanguageRefresh
{
    /// <summary>
    /// 文字唯一ID，用于生成与寻找多语言表
    /// </summary>
    public int textUID = 0;

#if UNITY_EDITOR

    protected override void OnValidate()
    {
        if (textUID == 0)//尚未初始化
        {
            textUID = GameDevelopmentUtility.GetOrGenGameDevData().GetNextTextUID(gameObject);
        }
        //Debug.LogWarning(string.Format("Text:{0} not have UID",this.name));
        base.OnValidate();
    }
#endif


    protected override void Awake()
    {
        RefreshMultLanguage();
        base.Awake();
    }

    public void RefreshMultLanguage()
    {
        this.text = MultLanguageUtility.GetExTextStr(textUID);
    }
}
