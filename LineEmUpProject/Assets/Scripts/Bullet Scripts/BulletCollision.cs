using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public event Action enemyHit; //event that triggers when an enemy is hit

    //disable collisions when a bullet is out of bounds
    private void disableColision()
    {
        enabled = false;
    }

    /// <summary>
    /// Callback when an object enters the collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) //enemy layer
        {
            other.GetComponent<IDamageable>()?.TakeDamage();
            enemyHit?.Invoke();
        }
    }

    private void OnEnable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds += disableColision;
    }
    private void OnDisable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds -= disableColision;
    }
}
