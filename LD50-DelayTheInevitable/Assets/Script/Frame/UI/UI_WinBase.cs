using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI�������
/// </summary>
public class UI_WinBase : MonoBehaviour
{
    public GameManager gameManager;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
}
