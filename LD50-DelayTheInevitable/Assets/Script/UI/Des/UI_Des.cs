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

    private Stack<UIN_DesItem> pool = new Stack<UIN_DesItem>();
    private List<UIN_DesItem> current = new List<UIN_DesItem>();

    void Start()
    {
        pool.Push(Item);
        Item.gameObject.SetActive(false);
        current.Clear();
    }

    public void Clean()
    {
        Des.SetActive(false);
        Items.SetActive(false);
    }

    public void ShowDes(string title,string des)
    {
        Des.SetActive(true);
        Items.SetActive(false);
        text_Title.text = title;
        text_des.text = des;
    }

    public void ShowItemDic(Dictionary<System.Type, BagItemBase> dic)
    {
        Des.SetActive(true);
        Items.SetActive(true);
        switch (MultLanguageUtility.GetLanguageTag())
        {
            case eMultLanguageTag.ZH:
                text_Title.text = "背包";
                break;
            case eMultLanguageTag.EN:
                text_Title.text = "Bag";
                break;
        }
        if (dic.Count <= 0)
        {
            switch (MultLanguageUtility.GetLanguageTag())
            {
                case eMultLanguageTag.ZH:
                    text_des.text = "空空如也";
                    break;
                case eMultLanguageTag.EN:
                    text_des.text = "It's Empty";
                    break;
            }
            return;
        }
        foreach (UIN_DesItem item in current)
        {
            pool.Push(Item);
            Item.gameObject.SetActive(false);
        }
        current.Clear();

        bool isFirst = true;
        foreach (KeyValuePair<System.Type, BagItemBase> bagData in dic)
        {
            UIN_DesItem item;
            if (pool.Count > 0)
            {
                item = pool.Pop();
            }
            else
            {
                item = Instantiate(Item);
            }
            item.ShowItem(bagData.Value);
            item.gameObject.SetActive(true);
            if (isFirst)
            {
                SetOnSelectItemAction(item);
                isFirst = false;
            }
        }
    }

    public void SetOnSelectItemAction(UIN_DesItem item)
    {
        item.onSelect.SetActive(true);
        text_des.text = item.bagItemBase.des;
    }
}
