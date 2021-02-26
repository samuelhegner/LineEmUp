using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set the players rotation to face the aiming direction
/// </summary>
public class PlayerAiming : MonoBehaviour
{
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
