using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 多语言工具类
/// </summary>
public static class MultLanguageUtility
{
    private static readonly string multLanguagtDataPath = Application.dataPath + "/Resources/MultLanguage/MultLaguageData.json";
    private static MultLanguageDatas multLanguageDatas;
    public static eMultLanguageTag languageTag;


    /// <summary>
    /// 是否有存储这个UID
    /// </summary>
    public static bool IsHaveUID(int uid)
    {
        ReadMultLanguage();
        return multLanguageDatas.multLanguageDic.ContainsKey(uid);
    }

    public static void ReadMultLanguage()
    {
        if (multLanguageDatas == null)//读取
        {
            string json = Resources.Load<TextAsset>("MultLanguage/MultLaguageData")?.text;
            multLanguageDatas = JsonConvert.DeserializeObject<MultLanguageDatas>(json);
//#if UNITY_EDITOR
//            if (File.Exists(multLanguagtDataPath))
//            {
//                using (StreamReader sr = new StreamReader(multLanguagtDataPath))
//                {
//                    string json = sr.ReadToEnd();
//                    if (json.Length > 0)
//                    {
//                        multLanguageDatas = JsonConvert.DeserializeObject<MultLanguageDatas>(json);
//                    }
//                }
//            }
//#endif

        }
        if (multLanguageDatas == null)//读取失败就创建
        {
            multLanguageDatas = new MultLanguageDatas();
        }
    }

    public static void SaveMultLanguage()
    {
        if (multLanguageDatas == null)
        {
            return;
        }
#if UNITY_EDITOR
        if (!Directory.Exists(Path.GetDirectoryName(multLanguagtDataPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(multLanguagtDataPath));
        }
        if (!File.Exists(multLanguagtDataPath))
        {
            File.Create(multLanguagtDataPath).Close();
        }
        string json = JsonConvert.SerializeObject(multLanguageDatas);
        File.WriteAllText(multLanguagtDataPath, json);
#else
        Debug.LogError("非编辑器,无法写入资源，请检查");
        return;
#endif
    }



    public static Dictionary<eMultLanguageTag, string> GetExTextStrDic(int uid)
    {
        ReadMultLanguage();
        if (!IsHaveUID(uid))
            return null;
        return multLanguageDatas.multLanguageDic[uid].strDic;
    }

    public static string GetExTextStr(int uid, eMultLanguageTag languageTag)
    {
        Dictionary<eMultLanguageTag, string> strDic = GetExTextStrDic(uid);
        if (strDic == null)
            return string.Empty;
        if (strDic.ContainsKey(languageTag))
            return strDic[languageTag];
        return string.Empty;
    }

    public static string GetExTextStr(int uid)
    {
        eMultLanguageTag languageTag = GetLanguageTag();
        return GetExTextStr(uid, languageTag);
    }

    public static void SetExTextStr(int uid, eMultLanguageTag languageTag,string str)
    {
        if (!multLanguageDatas.multLanguageDic.ContainsKey(uid))
        {
            multLanguageDatas.multLanguageDic[uid] = new MultLanguageData() {
                textUID = uid
            };
        }

        if (multLanguageDatas.multLanguageDic[uid].strDic == null)
        {
            multLanguageDatas.multLanguageDic[uid].strDic = new Dictionary<eMultLanguageTag, string>();
        }

        multLanguageDatas.multLanguageDic[uid].strDic[languageTag] = str;
    }

    /// <summary>
    /// 获取语言类型tag
    /// </summary>
    public static eMultLanguageTag GetLanguageTag()
    {
        if ((int)languageTag == 0)
        {
            if (PlayerPrefs.HasKey("languageTag"))
                languageTag = (eMultLanguageTag)PlayerPrefs.GetInt("languageTag");
            else
                languageTag = eMultLanguageTag.EN;
        }
        return languageTag;
    }

    public static void SwitchLanguage(eMultLanguageTag tag)
    {
        PlayerPrefs.SetInt("languageTag",(int)tag);
        languageTag = tag;
    }
}

public class MultLanguageDatas
{
    public Dictionary<int, MultLanguageData> multLanguageDic = new Dictionary<int, MultLanguageData>();//key:textUID
}

public class MultLanguageData//TODO 分割语言存储再载入更加节省内存
{
    public int textUID = 0;
    public Dictionary<eMultLanguageTag, string> strDic = new Dictionary<eMultLanguageTag, string>(); //key:eMultLanguageTag
}

public enum eMultLanguageTag
{
    ZH = 1,//中文
    EN = 2,//英文
}

