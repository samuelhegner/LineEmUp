using System;
using System.Collections;
using UnityEngine;


public class BulletOffScreenChecker : MonoBehaviour
{
    public event Action bulletOutOfBounds;

    void Update()
    {
        if (!BoundaryChecker.withinBulletEdges(transform.position))
        {
            bulletOutOfBounds?.Invoke();
            enabled = false;
        }
    }
}
