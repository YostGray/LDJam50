using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var playerCtrl = collision.gameObject.GetComponent<PlayerController>();
            playerCtrl.ladder2Climb = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var playerCtrl = collision.gameObject.GetComponent<PlayerController>();
            playerCtrl.ladder2Climb = null;
        }
    }
}
