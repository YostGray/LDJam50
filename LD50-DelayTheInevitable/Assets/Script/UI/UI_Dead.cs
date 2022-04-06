using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Dead : UI_WinBase
{
    public Button btn_startGame;
    public Button btn_historyScore;
    public ExText text_days;

    // Start is called before the first frame update
    void Start()
    {
        btn_startGame.onClick.AddListener(OnClickStartGame);
    }

    public void GameOver(int day)
    {
        text_days.text = string.Format(MultLanguageUtility.GetExTextStr(text_days.textUID), day);
    }

    private void OnClickStartGame()
    {
        gameManager.LodaMainScene();
        gameManager.DeleteUI("UI_Dead");
    }
}
