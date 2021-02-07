using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using System;

public class BulletTrail : Bullet
{
    [SerializeField] private Line trailLine;
    [SerializeField] private float trailDistance = 100f;
    private float trailSpeed;



    Vector3 startPos;
    Vector3 lastFramePos;

    bool bulletStopped;

    public float TrailDistance { set => trailDistance = value; }

    private void OnEnable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds += finishTrail;
    }
    private void OnDisable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds -= finishTrail;
    }

    private void finishTrail()
    {
        bulletStopped = true;
        StartCoroutine(finishTrailEnd());
    }

    private void Awake()
    {
        trailSpeed = GetComponent<BulletMover>().Speed;
    }

    void Update()
    {
        if (!bulletStopped) 
        {
            updateStartPosition();
            trailLine.End = transform.InverseTransformPoint(transform.position);
        }
        
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

    IEnumerator finishTrailEnd()
    {
        while (trailLine.Start != trailLine.End)
        {
            startPos = transform.localPosition + (-transform.forward.normalized * trailDistance);
            trailLine.Start = Vector3.MoveTowards(trailLine.Start, trailLine.End, trailSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
