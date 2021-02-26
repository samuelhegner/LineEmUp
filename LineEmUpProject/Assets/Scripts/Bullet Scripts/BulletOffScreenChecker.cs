using System;
using System.Collections;
using UnityEngine;


/// <summary>
/// Allows an easy way of checking whether a bullet is off screen or not
/// </summary>
public class BulletOffScreenChecker : MonoBehaviour
{
    public event Action bulletOutOfBounds; // event that is called when the bullet is out of bounds

    void Update()
    {
        if (!BoundaryChecker.withinBulletEdges(transform.position))
        {
            bulletOutOfBounds?.Invoke();
            enabled = false;
        }
    }
}
