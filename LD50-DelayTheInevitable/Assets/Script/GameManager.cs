using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public bool isGameRunning = false;
    public GameObject UICanvas;
    public CameraCtrl mainCamerCtrl;
    public WorldCtrl worldCtrl;

    private Dictionary<string, GameObject> UIDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        ///²»Ïú»Ù
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(UICanvas);
        DontDestroyOnLoad(mainCamerCtrl.gameObject);
        MessageCenter.Instance.AddListerner(eMsgType.LanguageChange, ChangeLaguage);
        MessageCenter.Instance.AddListerner(eMsgType.PlayerDead, OnPlayerDead);
        ShowUI("UI_LogIn");
        Application.targetFrameRate = 144;
    }

    private float passedTime = 0;
    private float totalPassedTime = 0;
    private void FixedUpdate()
    {
        if (!isGameRunning)
        {
            return;
        }
        totalPassedTime += Time.fixedDeltaTime;
        passedTime += Time.fixedDeltaTime;
        if (passedTime < 1)
        {
            return;
        }
        passedTime -= 1;
        worldCtrl?.WorldUpdate(totalPassedTime);
    }

    public GameObject ShowUI(string name)
    {
        if (UIDic.ContainsKey(name))
        {
            GameObject go = UIDic[name];
            go.SetActive(true);
            return go;
        }
        else
        {
            GameObject go = Resources.Load<GameObject>("UI/" + name);
            GameObject newGo = Instantiate(go, UICanvas.transform);
            newGo.GetComponent<UI_WinBase>()?.SetGameManager(this);
            UIDic.Add(name, newGo);
            newGo.SetActive(true);
            return newGo;
        }
    }

    public void HideUI(string name)
    {
        if (UIDic.ContainsKey(name))
        {
            GameObject go = UIDic[name];
            go.SetActive(false);
        }
    }

    public void DeleteUI(string name)
    {
        if (UIDic.ContainsKey(name))
        {
            GameObject go = UIDic[name];
            Destroy(go);
            UIDic.Remove(name);
        }
    }

    public void LodaMainScene()
    {
        totalPassedTime = 0;
        passedTime = 0;
        SceneManager.LoadSceneAsync("Main").completed += (AsyncOperation e) =>{
            Vector3 startPos = new Vector3(16, 14, 0);
            GameObject go = Resources.Load<GameObject>("Player/Player");
            GameObject newGo = Instantiate(go, startPos, Quaternion.identity);
            mainCamerCtrl?.SetCameraTarget(newGo.transform);
            PlayerData playerData = new PlayerData();
            GameObject stateUI = ShowUI("UI_GameState");

            worldCtrl = new WorldCtrl();
            worldCtrl.SetPlayerData(playerData);
            worldCtrl.updateAction = stateUI.GetComponent<UI_GameState>().UpDateUI;
            worldCtrl.InitWorldAndPlayMats();
            worldCtrl.WorldUpdate(0);
            isGameRunning = true;
        };
    }

    private void OnPlayerDead(object[] args)
    {
        int day = (int)args[0];

        isGameRunning = false;
        Debug.LogWarning("Player DEAD");
        GameObject stateUI = ShowUI("UI_Dead");
        stateUI.GetComponent<UI_Dead>().GameOver(day);
    }

    private void ChangeLaguage(object[] args)
    {
        ExText[] exTexts = UICanvas.transform.GetComponentsInChildren<ExText>();
        foreach (IMultLanguageRefresh multLanguageRefresh in exTexts)
        {
            multLanguageRefresh.RefreshMultLanguage();
        }
    }
}
