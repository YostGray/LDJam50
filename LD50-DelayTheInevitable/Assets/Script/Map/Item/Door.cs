using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ICouldInvestage
{
	private Collider2D box;
	public LayerMask player;
	public Animator doorAnimator;
	public BoxCollider2D realCollider;

	public AudioClip open;
	public AudioClip canNotOpen;
	private AudioSource thisAudioSource;

	//
	public bool isLocked = false;
	public Func<bool> unlockFunc;

	void Start()
	{
		box = GetComponent<BoxCollider2D>();
		thisAudioSource = GetComponent<AudioSource>();
	}

	public void palyOpenAudio()
	{
		playAudio(open);
	}

	void playAudio(AudioClip cilp)
	{
		thisAudioSource.clip = cilp;
		if (!thisAudioSource.isPlaying)
			thisAudioSource.Play();
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag != "Player")
			return;
		if (isLocked)
		{
			other.GetComponent<PlayerController>().SetCouldInvestageItem(this,InvestageType.normal);
		}
		else
		{
			doorAnimator.SetBool("open", true);
		}
	}

    private void OnTriggerExit2D(Collider2D other)
    {
		if (other.tag != "Player")
			return;
		other.GetComponent<PlayerController>().SetCouldInvestageItem(null, InvestageType.normal);
		doorAnimator.SetBool("open", false);
	}

	public void RealOpen()
	{
		realCollider.enabled = false;
	}
	public void RealClose()
	{
		realCollider.enabled = true;
	}

    public void Investage()
    {
        
    }
}
