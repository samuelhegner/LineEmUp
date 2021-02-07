using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public event Action enemyHit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) 
        {
            Destroy(other.gameObject);
            enemyHit?.Invoke();
        }
    }
}
