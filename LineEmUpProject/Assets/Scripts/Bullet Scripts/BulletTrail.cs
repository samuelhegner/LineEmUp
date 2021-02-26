using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using System;
using Photon.Pun;


/// <summary>
/// Crude way of rendering a bullet trail with Shapes asset
/// </summary>
public class BulletTrail : Bullet
{
    [SerializeField] private Line trailLine;
   
    private float trailDistance = 5f;
    private float trailSpeed;

    private PhotonView photonView;


    Vector3 startPos;
    Vector3 endPos;

    bool bulletStopped;


    /// <summary>
    /// Fill dependencies and set up start and end positions
    /// </summary>
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        trailSpeed = GetComponent<BulletMover>().Speed;
        startPos = transform.position;
        endPos = transform.position;
    }


    void Update()
    {
        //while the bullet is still moving update the trail
        if (!bulletStopped) 
        {
            updateStartPosition();
            updateEndPosition();
            updateLineRender();
        }
    }

    /// <summary>
    /// Set the trails length on setup
    /// </summary>
    /// <param name="distance"></param>
    public void SetTrailDistance(float distance)
    {
        trailDistance = distance;
    }

    /// <summary>
    /// Set the new start and end positions of the line renderer
    /// </summary>
    private void updateLineRender()
    {
        trailLine.End = transform.InverseTransformPoint(endPos);
        trailLine.Start = transform.InverseTransformPoint(startPos);
    }

    /// <summary>
    /// update the new end position of the trail
    /// </summary>
    private void updateEndPosition()
    {
        if (trailDistance <= Vector3.Distance(startPos, endPos))
        {
            endPos = startPos + (-transform.forward.normalized * trailDistance);
        }
    }

    /// <summary>
    /// Set the start position to the current position
    /// </summary>
    private void updateStartPosition()
    {
        startPos = transform.position;
    }

    /// <summary>
    /// when the bullet is off screen, finish the trail line
    /// </summary>
    private void finishTrail()
    {
        bulletStopped = true;
        StartCoroutine(finishTrailEnd());
    }

    /// <summary>
    /// Coroutine to set the end position towards to the start position
    /// </summary>
    /// <returns></returns>
    IEnumerator finishTrailEnd()
    {
        while (trailLine.Start != trailLine.End)
        {
            trailLine.End = Vector3.MoveTowards(trailLine.End, trailLine.Start, trailSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        photonView.RPC("RPC_DestroyBullet", RpcTarget.All); // delete the bullet from the game and all players
    }

    [PunRPC]
    void RPC_DestroyBullet() 
    {
        if (!photonView.IsMine)
            return;

        PhotonNetwork.Destroy(gameObject);
    }

    private void OnEnable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds += finishTrail; 
    }
    private void OnDisable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds -= finishTrail;
    }
}
