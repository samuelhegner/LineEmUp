using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public event Action enemyHit;
    private void OnEnable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds += disableColision;
    }
    private void OnDisable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds -= disableColision;
    }

    private void disableColision()
    {
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            other.GetComponent<IDamageable>()?.TakeDamage();
            enemyHit?.Invoke();
        }
    }
}
