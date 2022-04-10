using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    public GameObject Pop1, Pop2, Pop3;//三种气泡


    public bool isOperatingUI = false;
    private PlayerAction playerAction;
    private PlayerAttribute playerAttribute;
    public Ladder ladder2Climb { set; get; }//要爬的梯子
    private ICouldInvestage couldInvestageItem { set; get; }//目前可以调查的物品

    public void Awake()
    {
        playerAction = gameObject.GetComponent<PlayerAction>();
        playerAttribute = gameObject.GetComponent<PlayerAttribute>();
    }

    public void SetCouldInvestageItem(ICouldInvestage could, InvestageType type)
    {
        Pop1.SetActive(false);
        Pop2.SetActive(false);
        Pop3.SetActive(false);
        if (could == null)
        {
            return;
        }
        switch (type)
        {
            case InvestageType.normal:
                Pop1.SetActive(true);
                break;
            case InvestageType.import:
                Pop2.SetActive(true);
                break;
            case InvestageType.emergency:
                Pop3.SetActive(true);
                break;
            default:
                break;
        }
        couldInvestageItem = could;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Time.timeScale = 10;
        else if (Input.GetKeyUp(KeyCode.Alpha1))
            Time.timeScale = 1;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Time.timeScale = 100;
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            Time.timeScale = 1;

        bool bag = Input.GetKeyDown(KeyCode.B);//背包
        bool menu = Input.GetButtonDown("Menu");
        bool Cancel = Input.GetButtonDown("Cancel");
        bool ok = Input.GetButtonDown("OK");


        if (bag)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (isOperatingUI)
            {
                gm.HideUI("UI_Des");
                isOperatingUI = false;
                return;
            }
            UI_Des uiDes = gm.ShowUI("UI_Des").GetComponent<UI_Des>();
            uiDes.ShowItemDic(playerAttribute.BagPackage);
            isOperatingUI = true;
            return;
        }

        if (isOperatingUI)
        {
            return;
        }


        bool jump = Input.GetButtonDown("Jump");

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        float hor_abs = Math.Abs(hor);
        float ver_abs = Math.Abs(ver);

        if (couldInvestageItem != null && ok)
        {
            couldInvestageItem.Investage(this);
            return;
        }

        if (ver < -0.1 && jump)
        {
            playerAction.DownJump();
            return;
        }

        if (jump)
        {
            playerAction.PlayerJump();
            return;
        }

        if (hor_abs > ver_abs)
        {
            if (hor_abs > 0)
            {
                if (hor_abs > 0.5)
                {
                    playerAction.PlayerMove(hor < 0, true);
                }
                else
                {
                    playerAction.PlayerMove(hor < 0, false);
                }
            }
        }
        else
        {
            if (ver > 0.1)
            {
                if (ladder2Climb != null)
                {
                    playerAction.PlayerClimbLadder(true, ladder2Climb);
                }
            }
            else if (ver < -0.1)
            {
                if (ladder2Climb != null)
                {
                    playerAction.PlayerClimbLadder(false, ladder2Climb);
                }
            }
        }


    }

    public void AddBagItem(BagItemBase bagItemBase, int num)
    {
        var type = bagItemBase.GetType();
        var BagPackage = playerAttribute.BagPackage;
        if (BagPackage.ContainsKey(type))
        {
            BagPackage[type].num += num;
        }
        else
        { 
            bagItemBase.num = num;
            BagPackage.Add(type, bagItemBase);
        }
    }
    public bool RemoveBagItem(System.Type type, int num)
    {
        //if (!type.IsAssignableFrom(typeof(BagItemBase)))
        //{
        //    Debug.LogError("不是背包物品");
        //    return false;
        //}
        var BagPackage = playerAttribute.BagPackage;
        if (BagPackage.ContainsKey(type))
        {
            int currentValue = BagPackage[type].num;
            if (currentValue > num)
            {
                BagPackage[type].num -= num;
                return true;
            }
            else if (currentValue == num)
            {
                BagPackage.Remove(type);
                return true;
            }
        }
        return false;
    }
}