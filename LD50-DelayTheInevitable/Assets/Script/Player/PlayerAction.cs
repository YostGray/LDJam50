using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PlayerAction : MonoBehaviour
{
    readonly Vector3 faceLeft = new Vector3(-1, 1, 1);

    Rigidbody2D playerRigidbody2D;
    PlayerAttribute playerAttribute;
    Animator playerAnimator;

    private LayerMask groundLayerMask;
    private LayerMask interactiveLayerMask;
    private bool isClimbing = false;

    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerAttribute = GetComponent<PlayerAttribute>();
        playerAnimator = GetComponent<Animator>();

        groundLayerMask = LayerMask.GetMask("Ground");
    }

    public void PlayerMove(bool isLeft, bool isRunning)
    {
        if (playerAttribute.canMove)
        {
            if (isFaceWall(FaceMode.headFace, isLeft) || isFaceWall(FaceMode.bodyFace, isLeft) || isFaceWall(FaceMode.footFace, isLeft))
            {
                return;
            }
            if (isRunning)
            {
                //if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("走路"))
                //    playerAnimator.speed = 1.5f;
                if (isLeft)
                    playerRigidbody2D.velocity = new Vector2(playerAttribute.moveVelocity * -1.5f, playerRigidbody2D.velocity.y);
                else
                    playerRigidbody2D.velocity = new Vector2(playerAttribute.moveVelocity * 1.5f, playerRigidbody2D.velocity.y);
            }
            else
            {
                //if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("走路"))
                //    playerAnimator.speed = 1f;

                if (isLeft)
                    playerRigidbody2D.velocity = new Vector2(-playerAttribute.moveVelocity, playerRigidbody2D.velocity.y);
                else
                    playerRigidbody2D.velocity = new Vector2(playerAttribute.moveVelocity, playerRigidbody2D.velocity.y);
            }
        }
    }

    public void PlayerJump()
    {
        if (playerAttribute.canJump)
        {
            if (playerAttribute.canJump && isOntheGround())
                playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, playerAttribute.jumpVelocity);
        }
    }

    public void DownJump()
    {
        float deep = 18f;
        Vector2 position1, position2;
        Vector2 position = this.transform.position;

        position1 = new Vector2(position.x - 3.5f, position.y);
        position2 = new Vector2(position.x + 3.5f, position.y);

        RaycastHit2D
        hit1 = Physics2D.Raycast(position1, Vector2.down, deep, groundLayerMask | interactiveLayerMask),
        hit2 = Physics2D.Raycast(position2, Vector2.down, deep, groundLayerMask | interactiveLayerMask);
        if (hit1.collider != null && hit1.collider.tag == "Plat" ||
            hit2.collider != null && hit2.collider.tag == "Plat")
        {
            playerRigidbody2D.velocity = new Vector2(0, -5);
            transform.position = transform.position + new Vector3(0,-5,0);
        }
    }

    private void PlayerCrouchedMove(bool isLeft)
    {
        if (playerAttribute.canMove)
        {
            if (isFaceWall(FaceMode.headFace, isLeft) || isFaceWall(FaceMode.bodyFace, isLeft) || isFaceWall(FaceMode.footFace, isLeft))
            {
                return;
            }
            if (isLeft)
                playerRigidbody2D.velocity = new Vector2(-playerAttribute.moveVelocity * 0.8f, playerRigidbody2D.velocity.y);
            else
                playerRigidbody2D.velocity = new Vector2(playerAttribute.moveVelocity * 0.8f, playerRigidbody2D.velocity.y);
        }
    }

    public void PlayerClimbLadder(bool isUp,Ladder ladder)
    {
        if (!isUp && isOntheGround(true))
        {
            return;
        }
        isClimbing = true;
        float x = ladder.transform.position.x - transform.position.x;
        int flag = x > 0 ? 1 : -1;
        x = flag * Math.Min(Math.Abs(x), playerAttribute.Climbing2LadderCenterSpeed) * Time.deltaTime;
        float y = playerAttribute.ClimbingLadderSpeed * (isUp ? 1 : -1) * Time.deltaTime;
        playerRigidbody2D.velocity = Vector2.zero;
        Vector3 posChange = new Vector3(x, y, transform.position.z);
        transform.position = transform.position + posChange;
    }

    /// <summary>
    /// 是否朝向墙壁的射线检测
    /// </summary>
    /// <param name="faceMode"></param>
    /// <param name="isFaceLeft"></param>
    /// <returns></returns>
    private bool isFaceWall(FaceMode faceMode, bool isFaceLeft)
    {
        float deep = 10f;
        Vector2 startPosition = new Vector2();
        Vector2 position = this.transform.position;

        switch (faceMode)
        {
            case FaceMode.bodyFace:
                startPosition = new Vector2(position.x, position.y + 8f); // new Vector2 (position.x, position.y);
                break;
            case FaceMode.headFace:
                startPosition = new Vector2(position.x, position.y + 20f);
                break;
            case FaceMode.footFace:
                startPosition = new Vector2(position.x, position.y - 1f);
                break;
        }

        if (!isFaceLeft)
        {
            RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.right, deep, groundLayerMask | interactiveLayerMask);
            Debug.DrawLine(startPosition, new Vector2(startPosition.x + deep, startPosition.y), Color.red);
            return hit.collider != null && hit.collider.tag != "Plat";
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.left, deep, groundLayerMask | interactiveLayerMask);
            Debug.DrawLine(startPosition, new Vector2(startPosition.x - deep, startPosition.y), Color.red);
            return hit.collider != null && hit.collider.tag != "Plat";
        }
    }

    private bool isOntheGround(bool withOutPlat = false)
    {
        float deep = 18f;
        Vector2 position1, position2;
        Vector2 position = this.transform.position;

        position1 = new Vector2(position.x - 3.5f, position.y);
        position2 = new Vector2(position.x + 3.5f, position.y);

        RaycastHit2D
        hit1 = Physics2D.Raycast(position1, Vector2.down, deep, groundLayerMask | interactiveLayerMask),
        hit2 = Physics2D.Raycast(position2, Vector2.down, deep, groundLayerMask | interactiveLayerMask);

        Debug.DrawLine(position1, new Vector2(position1.x, position1.y - deep), Color.yellow, 1);
        Debug.DrawLine(position2, new Vector2(position2.x, position2.y - deep), Color.yellow, 1);

        if (withOutPlat)
        {
            if ((hit1.collider != null && hit1.collider.tag != "Plat") || 
                (hit2.collider != null && hit2.collider.tag != "Plat"))
            {
                return true;
            }
            return false;
        }

        return (hit1.collider != null || hit2.collider != null);
    }
    void FixedUpdate()
    {
        //动画相关
        float velocityX = playerRigidbody2D.velocity.x;
        float abs_velocityX = Mathf.Abs(velocityX);
        playerAnimator.SetFloat("horizontalSpeed", abs_velocityX);
        playerAnimator.SetFloat("verticalSpeed", playerRigidbody2D.velocity.y);
        if (isClimbing && playerRigidbody2D.velocity.sqrMagnitude > 1)
        {
            isClimbing = false;
        }
        if (playerAnimator.GetBool("isClimbing") != isClimbing)
        {
            playerAnimator.SetBool("isClimbing", isClimbing);
        }

        if (velocityX < -0.5)
            transform.localScale = faceLeft;
        else if (velocityX > 0.5)
            transform.localScale = Vector3.one;
    }
}

public enum FaceMode
{
    headFace,
    bodyFace,
    footFace
}