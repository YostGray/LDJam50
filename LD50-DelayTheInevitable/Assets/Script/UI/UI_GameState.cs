using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameState : UI_WinBase
{
    public ExText dayText;//日期与时间

    public Text O2;
    public Text H2O;
    public Text CO2;
    public Text Power;

    public Image StaminaBar;
    public Image StaveBar;
    public Image ThirtyBar;
    private Vector2 fullSize;

    private void Start()
    {
        fullSize = new Vector2(156,13);
    }

    public void UpDateUI(WorldCtrl worldCtrl)
    {
        int dayNum = worldCtrl.dayNum;
        float todayTimePassed = worldCtrl.timePassed - (dayNum - 1) * worldCtrl.DayTime;
        string orgStr = MultLanguageUtility.GetExTextStr(dayText.textUID);
        dayText.text = string.Format(orgStr, dayNum, TodayTimePassed2TimeString(todayTimePassed, worldCtrl.DayTime));

        SetMatShow(worldCtrl, eMaterialType.O2_w, O2, 0.1f, 1000);
        SetMatShow(worldCtrl, eMaterialType.H2O_w, H2O, 0.1f, 1000);
        SetMatShow(worldCtrl, eMaterialType.CO2_w, CO2, 0.1f, 1000);
        SetMatShow(worldCtrl, eMaterialType.Power_w, Power, 1, 500);

        float rate = 0;
        rate = worldCtrl.playerData.hungraryNum / worldCtrl.playerData.hungraryNumMax;
        StaveBar.rectTransform.sizeDelta = new Vector2(rate * fullSize.x, fullSize.y);

        rate = worldCtrl.playerData.thirtyNum / worldCtrl.playerData.thirtyNumMax;
        ThirtyBar.rectTransform.sizeDelta = new Vector2(rate * fullSize.x, fullSize.y);

        rate = worldCtrl.playerData.tiredNum / worldCtrl.playerData.tiredNumMax;
        StaminaBar.rectTransform.sizeDelta = new Vector2(rate * fullSize.x, fullSize.y);
    }

    private void SetMatShow(WorldCtrl worldCtrl, eMaterialType type,Text text,float rate,float warningValue)
    {
        float value = worldCtrl.GetWorldMat(type);
        float showValue = Mathf.CeilToInt(value * rate);
        if (value <= warningValue)
        {
            text.text = string.Format("<color=red>{0}</color>", showValue);
        }
        else
        {
            text.text = showValue.ToString();
        }
    }

    private string TodayTimePassed2TimeString(float todayTimePassed,int dayLong)
    {
        float time = todayTimePassed / dayLong * 24;
        int hour = Mathf.FloorToInt(time);
        int min = Mathf.FloorToInt(60 * (time - hour));

        StringBuilder sb = new StringBuilder();
        if (hour <= 9)
        {
            sb.Append("0");
        }
        sb.Append(hour);
        sb.Append(":");
        if (min <= 9)
        {
            sb.Append("0");
        }
        sb.Append(min);
        return sb.ToString();
    }
}
