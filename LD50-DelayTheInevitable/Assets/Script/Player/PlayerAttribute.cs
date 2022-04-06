using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 玩家属性
/// </summary>
public class PlayerAttribute : MonoBehaviour
{
    public int hp = 1;
    public bool canMove = true, canJump = true, canWallJump = false, canRun = false;//能力开关
    public float moveVelocity = 5.0f;
    public float jumpVelocity = 10.0f;

    public bool isClimbingLadder = false;
    public float Climbing2LadderCenterSpeed = 5.0f;
    public float ClimbingLadderSpeed = 5.0f;
}
