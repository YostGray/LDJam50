using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LogIn : UI_WinBase
{
    public Button btn_startGame;
    public Button btn_historyScore;
    public Toggle tog_EN;
    public Toggle tog_CN;

    // Start is called before the first frame update
    void Start()
    {
        switch (MultLanguageUtility.GetLanguageTag())
        {
            case eMultLanguageTag.ZH:
                tog_CN.isOn = true;
                break;
            case eMultLanguageTag.EN:
                tog_EN.isOn = true;
                break;
            default:
                break;
        }
        tog_EN.onValueChanged.AddListener(OnENTogValueChange);
        tog_CN.onValueChanged.AddListener(OnCNTogValueChange);
        btn_startGame.onClick.AddListener(OnClickStartGame);
    }

    private void OnENTogValueChange(bool isOn)
    {
        if (isOn)
        {
            MultLanguageUtility.SwitchLanguage(eMultLanguageTag.EN);
            MessageCenter.Instance.BroadcastMsg(eMsgType.LanguageChange);
        }
    }

    private void OnCNTogValueChange(bool isOn)
    {
        if (isOn)
        {
            MultLanguageUtility.SwitchLanguage(eMultLanguageTag.ZH);
            MessageCenter.Instance.BroadcastMsg(eMsgType.LanguageChange);
        }
    }

    private void OnClickStartGame()
    {
        gameManager.LodaMainScene();
        gameManager.DeleteUI("UI_LogIn");
    }
}
