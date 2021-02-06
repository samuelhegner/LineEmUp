using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    Vector3 aimingDirection;

    private void LateUpdate()
    {
        if(aimingDirection != Vector3.zero && aimingDirection.magnitude > 0.1f)
            transform.forward = aimingDirection;
    }

    public void updateAimingData(Vector3 smoothAimInput) 
    {
        aimingDirection = smoothAimInput;
    }


}
