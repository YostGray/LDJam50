using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pc : MonoBehaviour //,ICouldInvestage
{
    public string titleCN;
    public string titleEN;
    public string desCN;
    public string desEN;

    public void Investage()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        UI_Des uiDes = gm.ShowUI("UI_Des").GetComponent<UI_Des>();
        uiDes.Clean();
        switch (MultLanguageUtility.GetLanguageTag())
        {
            case eMultLanguageTag.ZH:
                uiDes.ShowDes(titleCN, desCN);
                break;
            case eMultLanguageTag.EN:
                uiDes.ShowDes(titleEN, desEN);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var playerCtrl = collision.gameObject.GetComponent<PlayerController>();
            Investage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var playerCtrl = collision.gameObject.GetComponent<PlayerController>();
            GameManager gm = FindObjectOfType<GameManager>();
            gm.HideUI("UI_Des");
        }
    }
}
