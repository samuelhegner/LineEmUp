using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using System;

public class BulletTrail : Bullet
{
    [SerializeField] private Line trailLine;
    [SerializeField] private float trailDistance = 100f;

    Vector3 startPos;

    public float TrailDistance { set => trailDistance = value; }

    void Update()
    {
        updateStartPosition();
        trailLine.End = transform.InverseTransformPoint(transform.position);
    }

    public override void setUp(BulletSetupInfo info)
    {
        setStartPosition(info.startingPosition);
        trailLine.Start = transform.InverseTransformPoint(startPos);
    }

    private void updateStartPosition()
    {
        Vector3 toBullet = transform.position - startPos;

        if (trailDistance <= toBullet.magnitude)
        {
            startPos = transform.localPosition + (-transform.forward.normalized * trailDistance);
        }

        trailLine.Start = transform.InverseTransformPoint(startPos);

    }

    void setStartPosition(Vector3 position) 
    {
        startPos = position;
    }
}
