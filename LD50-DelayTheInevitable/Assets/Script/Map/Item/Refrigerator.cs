using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour, ICouldInvestage
{
    private BoxCollider2D boxCollider2D;
    public Animator doorAnimator;

    public AudioClip open;
    public AudioClip canNotOpen;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        doorAnimator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;
        other.GetComponent<PlayerController>().SetCouldInvestageItem(this, InvestageType.normal);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;
        other.GetComponent<PlayerController>().SetCouldInvestageItem (null, InvestageType.normal);
        doorAnimator.SetBool("Open", false);
    }

    public void Investage()
    {
        bool isOpen = doorAnimator.GetBool("Open");
        if (!isOpen)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.worldCtrl.playerData.UseChangePlayerRes(eMaterialType.PlayerHungrary, 100);
            gm.worldCtrl.playerData.UseChangePlayerRes(eMaterialType.PlayerThirty, 500);
        }
        doorAnimator.SetBool("Open", !isOpen);
    }
}
