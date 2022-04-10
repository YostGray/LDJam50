using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG;

/// <summary>
/// 绿藻罐子
/// </summary>
public class AlgaeTank : MonoBehaviour, ICouldInvestage, ICouldSwitch
{
    public SpriteRenderer green;
    private bool isGreenTank = false;
    private Process process;

    private void Awake()
    {
        process = new Process();

        process.totalCostTime = -1;//持續進行的
        process.costPreSecond.Add(eMaterialType.H2O_w, 1);
        process.costPreSecond.Add(eMaterialType.CO2_w, 1);

        process.outputPreSecond.Add(eMaterialType.Organism4Eat,1);
        process.outputPreSecond.Add(eMaterialType.O2_w, 1);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isGreenTank)
        {
            return;
        }
        if (other.tag != "Player")
            return;
        other.GetComponent<PlayerController>().SetCouldInvestageItem(this, InvestageType.import);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;
        other.GetComponent<PlayerController>().SetCouldInvestageItem(null, InvestageType.import);
    }

    public void Investage(PlayerController playerController)
    {
        if (isGreenTank)
        {
            return;
        }
        if (playerController.RemoveBagItem(typeof(BagAlgaeBeaker), 1))
        {
            isGreenTank = true;
            Tween tween = green.DOFade(1,3);
            tween.SetAutoKill(true);
            tween.Play();
            GameManager gm = FindObjectOfType<GameManager>();
            gm.worldCtrl.processDic.Add(process);
            playerController.SetCouldInvestageItem(null, InvestageType.import);
        }
    }
}
