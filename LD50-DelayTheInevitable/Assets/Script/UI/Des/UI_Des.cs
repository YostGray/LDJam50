using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 描述与选择物品的UI
/// </summary>
public class UI_Des : UI_WinBase
{
    public GameObject Des;
    public GameObject Items;
    public UIN_DesItem Item;

    public Text text_Title;
    public Text text_des;

    void Start()
    {

    }

    public void Clean()
    {
        Des.SetActive(false);
        Items.SetActive(false);
    }

    public void ShowDes(string title,string des)
    {
        Des.SetActive(true);
        text_Title.text = title;
        text_des.text = des;
    }

    public void ShowItemList()
    {

    }

    public void SetOnSelectItemAction()
    {
        
    }
}
