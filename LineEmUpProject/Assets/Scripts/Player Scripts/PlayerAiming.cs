using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    Vector3 aimingDirection;

    private void Update()
    {
        if (aimingDirection != Vector3.zero && aimingDirection.magnitude > 0.1f)
            transform.rotation = Quaternion.LookRotation(aimingDirection, Vector3.up);
    }

    public void updateAimingData(Vector3 smoothAimInput) 
    {
        aimingDirection = smoothAimInput;
    }


}
