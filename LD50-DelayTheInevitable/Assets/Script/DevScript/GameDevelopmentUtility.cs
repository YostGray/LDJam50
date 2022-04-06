using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

#if UNITY_EDITOR

/// <summary>
/// 开发用游戏全局配置
/// </summary>
public static class GameDevelopmentUtility
{
    private static readonly string path4GDD = Application.dataPath + "/Editor/GameDevData.json";
    private static GameDevelopmentData gameDevelopmentData;

    /// <summary>
    /// 获取或创建 开发用游戏全局配置
    /// </summary>
    public static GameDevelopmentData GetOrGenGameDevData()
    {
        if (gameDevelopmentData == null)//读取
        {
            if (File.Exists(path4GDD))
            {
                using (StreamReader sr = new StreamReader(path4GDD))
                {
                    string json = sr.ReadToEnd();
                    if (json.Length > 0)
                    {
                        gameDevelopmentData = JsonConvert.DeserializeObject<GameDevelopmentData>(json);
                    }
                }
            }
        }
        if (gameDevelopmentData == null)//读取失败就创建
        {
            gameDevelopmentData = new GameDevelopmentData();
        }
        return gameDevelopmentData;
    }

    /// <summary>
    /// 保存 开发用游戏全局配置
    /// </summary>
    public static void SaveGameDevData()
    {
        if (gameDevelopmentData == null)
        {
            return;
        }
        if (!Directory.Exists(Path.GetDirectoryName(path4GDD)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path4GDD));
        }
        if (!File.Exists(path4GDD))
        {
            File.Create(path4GDD).Close();
        }

        string json = JsonConvert.SerializeObject(gameDevelopmentData);
        File.WriteAllText(path4GDD, json);
    }
}

public class GameDevelopmentData
{
    public int textUID;//文字唯一ID目前最大值
    public Dictionary<int, int> textUID2TextGUIDDic;//字典

    public GameDevelopmentData()
    {
        textUID = 0;
        textUID2TextGUIDDic = new Dictionary<int, int>();
    }

    public int GetNextTextUID(GameObject gameObject)
    {
        textUID++;
        textUID2TextGUIDDic[textUID] = gameObject.GetInstanceID();
        GameDevelopmentUtility.SaveGameDevData();
        return textUID;
    }

    public int RemoveTextUID(int textUID)
    {
        textUID2TextGUIDDic.Remove(textUID);
        GameDevelopmentUtility.SaveGameDevData();
        return textUID;
    }
}

#endif