using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIN_DesItem : MonoBehaviour
{
    public GameObject onSelect;
    public Image img;

    public BagItemBase bagItemBase;

    public void ShowItem(BagItemBase bagItemBase)
    {
        this.bagItemBase = bagItemBase;
        Sprite sprite = Resources.Load<Sprite>("Item/" + bagItemBase.resName);
        if (sprite != null)
        {
            img.sprite = sprite;
        }
    }
}
