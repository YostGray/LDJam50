using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class WorldCtrl
{
    public readonly int DayTime = 720;
    public int dayNum { private set; get; } = 1;//第几天
    public float timePassed { private set; get; } = 0;//过去了几秒

    Dictionary<eMaterialType, float> worldMatDic = new Dictionary<eMaterialType, float>();//世界资源字典
    public PlayerData playerData { private set; get; }

    public Action<WorldCtrl> updateAction { set; get; }


    public void SetPlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
    }

    public void WorldUpdate(float totalPassedTime)
    {
        timePassed = totalPassedTime;
        if (timePassed > DayTime * dayNum)
        {
            dayNum++;
        }

        if (playerData != null)
        {
            Process playerLivingProcess = playerData.GetLivingProcess();
            RunProcess(playerLivingProcess);
            if (playerData.IsDead())
            {
                object[] args = { dayNum };
                MessageCenter.Instance.BroadcastMsg(eMsgType.PlayerDead, args);
            }
        }

        updateAction?.Invoke(this);
    }

    private void RunProcess(Process process)
    {
        bool couldCost = true;
        foreach (KeyValuePair<eMaterialType, float> item in process.costPreSecond)
        {
            switch (item.Key)
            {
                case eMaterialType.O2_w:
                case eMaterialType.H2O_w:
                case eMaterialType.CO2_w:
                case eMaterialType.Power_w:
                case eMaterialType.Organism4Eat:
                case eMaterialType.OrganismCanNotEat:
                    if (!worldMatDic.ContainsKey(item.Key) || worldMatDic[item.Key] < item.Value)
                    {
                        couldCost = false;
                    }
                    break;
                case eMaterialType.PlayerHungrary:
                case eMaterialType.PlayerThirty:
                case eMaterialType.PlayerTired:
                case eMaterialType.PlayerShit:
                case eMaterialType.PlayerPee:
                    if (!playerData.CheckPlayCost(item.Key, item.Value))
                    {
                        couldCost = false;
                    }
                    break;
                default:
                    break;
            }
        }

        if (couldCost)
        {
            foreach (KeyValuePair<eMaterialType, float> item in process.costPreSecond)
            {
                switch (item.Key)
                {
                    case eMaterialType.O2_w:
                    case eMaterialType.H2O_w:
                    case eMaterialType.CO2_w:
                    case eMaterialType.Power_w:
                    case eMaterialType.Organism4Eat:
                    case eMaterialType.OrganismCanNotEat:
                        worldMatDic[item.Key] -= item.Value;
                        break;
                    case eMaterialType.PlayerHungrary:
                    case eMaterialType.PlayerThirty:
                    case eMaterialType.PlayerTired:
                    case eMaterialType.PlayerShit:
                    case eMaterialType.PlayerPee:
                        playerData.UseChangePlayerRes(item.Key, -item.Value);
                        break;
                    default:
                        break;
                }
            }

            foreach (KeyValuePair<eMaterialType, float> item in process.outputPreSecond)
            {
                switch (item.Key)
                {
                    case eMaterialType.O2_w:
                    case eMaterialType.H2O_w:
                    case eMaterialType.CO2_w:
                    case eMaterialType.Power_w:
                    case eMaterialType.Organism4Eat:
                    case eMaterialType.OrganismCanNotEat:
                        worldMatDic[item.Key] += item.Value;
                        break;
                    case eMaterialType.PlayerHungrary:
                    case eMaterialType.PlayerThirty:
                    case eMaterialType.PlayerTired:
                    case eMaterialType.PlayerShit:
                    case eMaterialType.PlayerPee:
                        playerData.UseChangePlayerRes(item.Key, item.Value);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            process?.canNotProcessAction?.Invoke();
        }
    }

    public float GetWorldMat(eMaterialType materialType)
    {
        worldMatDic.TryGetValue(materialType, out float value);
        return value;
    }

    /// <summary>
    /// 初始化世界以及玩家的物资
    /// </summary>
    public void InitWorldAndPlayMats()
    {
        worldMatDic.Clear();

        worldMatDic.Add(eMaterialType.O2_w, 720);//一天的量
        worldMatDic.Add(eMaterialType.H2O_w, 1500);//两天的量
        worldMatDic.Add(eMaterialType.CO2_w, 2100);//三天的量
        worldMatDic.Add(eMaterialType.Power_w, 1000);

        worldMatDic.Add(eMaterialType.Organism4Eat, 3500);//五天的量
        worldMatDic.Add(eMaterialType.OrganismCanNotEat, 1500);//一天的量

        playerData.SetPlayerInitMat();
    }
}
