using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerData
{
    public ePlayerState ePlayerState = ePlayerState.normal;


    public readonly int hungraryNumMax = 1000;
    public readonly int thirtyNumMax = 1000;
    public readonly int tiredNumMax = 1000;
    readonly int negHungraryNumMax = -100;
    readonly int negThirtyNumMax = -100;
    readonly int negTiredNumMax = 0;
    public float hungraryNum { private set; get; } = 0;//饥饿值 可为负数 与可食用有机物 1：1
    public float thirtyNum { private set; get; } = 0;//口渴值 与H2O 1：1
    public float tiredNum { private set; get; } = 0;//劳累值

    readonly int O2WantedMax = 10;
    float O2Wanted = 0;//氧气期望  呼吸不到会累计这个值

    float ShitNum = 0;//屎值
    float PeeNum = 0;//尿值

    public Dictionary<ePlayerState, Process> diffStateLivingProcess = new Dictionary<ePlayerState, Process>();


    readonly float zheSunSuLv = 0.9f;
    private void BenchSet(Process process,float value)
    {
        process.totalCostTime = -1;
        process.costPreSecond.Add(eMaterialType.O2_w, value);
        process.costPreSecond.Add(eMaterialType.PlayerHungrary, value);
        process.costPreSecond.Add(eMaterialType.PlayerThirty, value);
        process.costPreSecond.Add(eMaterialType.PlayerTired, value);

        process.outputPreSecond.Add(eMaterialType.CO2_w, value * zheSunSuLv);
        process.outputPreSecond.Add(eMaterialType.H2O_w, value * zheSunSuLv);
        process.outputPreSecond.Add(eMaterialType.PlayerShit, value * zheSunSuLv);
        process.outputPreSecond.Add(eMaterialType.PlayerPee, value * zheSunSuLv);

        process.canNotProcessAction = () =>
        {
            O2Wanted += value;
        };
    }
    public PlayerData()
    {
        Process normalProcess = new Process();
        BenchSet(normalProcess, 1);
        Process workProcess = new Process();
        BenchSet(workProcess, 2);
        Process sleepProcess = new Process();
        BenchSet(sleepProcess, 0.5f);

        diffStateLivingProcess.Add(ePlayerState.normal, normalProcess);
        diffStateLivingProcess.Add(ePlayerState.working, workProcess);
        diffStateLivingProcess.Add(ePlayerState.sleep, sleepProcess);
    }

    public bool IsDead()
    {
        if (O2Wanted > O2WantedMax)
        {
            return true;
        }
        if (hungraryNum < negHungraryNumMax)
        {
            return true;
        }
        if (thirtyNum < negThirtyNumMax)
        {
            return true;
        }
        return false;
    }

    public Process GetLivingProcess()
    {
        return diffStateLivingProcess[ePlayerState];
    }

    public bool CheckPlayCost(eMaterialType eMaterialType , float value)
    {
        switch (eMaterialType)
        {
            case eMaterialType.PlayerHungrary:
                //if (hungraryNum - negHungraryNumMax > value)
                //{
                //    return true;
                //}
            case eMaterialType.PlayerThirty:
                //if (thirtyNum - negThirtyNumMax > value)
                //{
                //    return true;
                //}
            case eMaterialType.PlayerTired:
                //if (tiredNum - negTiredNumMax > value)
                //{
                //    return true;
                //}
                return true;
            case eMaterialType.PlayerShit:
                if (ShitNum > value)
                {
                    return true;
                }
                break;
            case eMaterialType.PlayerPee:
                if (PeeNum > value)
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    public void UseChangePlayerRes(eMaterialType eMaterialType, float value)
    {
        switch (eMaterialType)
        {
            case eMaterialType.PlayerHungrary:
                hungraryNum += value;
                break;
            case eMaterialType.PlayerThirty:
                thirtyNum += value;
                break;
            case eMaterialType.PlayerTired:
                tiredNum += value;
                break;
            case eMaterialType.PlayerShit:
                ShitNum += value;
                break;
            case eMaterialType.PlayerPee:
                PeeNum += value;
                break;
            default:
                break;
        }
    }

    public void SetPlayerInitMat()
    {
        hungraryNum = hungraryNumMax * 0.5f;
        thirtyNum = thirtyNumMax * 0.8f;
        tiredNum = tiredNumMax * 0.85f;
    }
}

public enum ePlayerState
{
    normal,
    working,
    sleep,
}
