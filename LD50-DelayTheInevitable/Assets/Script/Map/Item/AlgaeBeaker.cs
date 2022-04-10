using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 绿藻烧杯
/// </summary>
class AlgaeBeaker : MonoBehaviour, ICouldInvestage
{
    bool isPicked = false;
    public Sprite pickedImg;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isPicked)
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
        if (isPicked)
        {
            return;
        }
        playerController.AddBagItem(new BagAlgaeBeaker(), 3);
        this.GetComponent<SpriteRenderer>().sprite = pickedImg;
        isPicked = true;
    }
}
