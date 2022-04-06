using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField]
    private CameraMode cameraMode;
    private Transform targetTramsform;
    private float targetX, targetY, thisX, thisY;
    [SerializeField]
    private float cameraSpeed = 0.1f, maxDistance = 10f;
    [SerializeField]
    private float boderH = 32, boderW = 16;
    private Vector3 target;

    public void SetCameraTarget(Transform targetTramsform, CameraMode cameraMode = CameraMode.none)
    {
        this.targetTramsform = targetTramsform;
        this.transform.position = new Vector3(targetTramsform.position.x, targetTramsform.position.y, -10);
        if (cameraMode != CameraMode.none)
        {
            this.cameraMode = cameraMode;
        }
        if (this.cameraMode == CameraMode.simpleChild)
        {
            this.transform.parent = targetTramsform;
        }
    }

    void Update()
    {
        if (targetTramsform == null)
        {
            return;
        }
        switch (cameraMode)
        {
            case CameraMode.simpleChild:
                return;
            case CameraMode.smoothFollow:
                targetX = targetTramsform.position.x;
                targetY = targetTramsform.position.y;

                target = new Vector3(targetX, targetY, -10);
                //transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * CameraSpeed);

                if (Vector2.Distance(targetTramsform.position, transform.position) >= maxDistance)
                {
                    target = new Vector3(targetX, targetY, -10);
                    float Speed = Vector2.Distance(transform.position, target) * cameraSpeed;
                    transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * Speed);
                }
                break;
            case CameraMode.withBoard:
                targetX = targetTramsform.position.x;
                targetY = targetTramsform.position.y;

                thisX = this.transform.position.x;
                thisY = this.transform.position.y;
                if (targetY - thisY > boderH)
                {
                    transform.position = new Vector3(thisX, thisY + (targetY - thisY - boderH), -10);
                }
                else if (targetY - thisY < -boderH)
                {
                    transform.position = new Vector3(thisX, thisY + (targetY - thisY + boderH), -10);
                }

                if (targetX - thisX > boderW)
                {
                    transform.position = new Vector3(thisX + (targetX - thisX - boderW), transform.position.y, -10);
                }
                else if (targetX - thisX < -boderW)
                {
                    transform.position = new Vector3(thisX + (targetX - thisX + boderW), transform.position.y, -10);
                }
                break;
            default:
                break;
        }
    }
}

public enum CameraMode
{
    none,
    simpleChild,
    smoothFollow,
    withBoard,
}
