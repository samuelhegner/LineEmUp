using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using System;
using Photon.Pun;

public class BulletTrail : Bullet
{
    [SerializeField] private Line trailLine;
    private float trailDistance = 5f;
    private float trailSpeed;
    private PhotonView photonView;


    Vector3 startPos;
    Vector3 endPos;

    bool bulletStopped;

    public void SetTrailDistance(float distance) {
        trailDistance = distance;
    }
    public PhotonView PhotonView { get => photonView; set => photonView = value; }

    private void OnEnable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds += finishTrail;
    }
    private void OnDisable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds -= finishTrail;
    }

   

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        trailSpeed = GetComponent<BulletMover>().Speed;
        startPos = transform.position;
        endPos = transform.position;
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (!bulletStopped) 
        {
            updateStartPosition();
            updateEndPosition();
            updateLineRender();
        }
    }

    private void updateLineRender()
    {
        trailLine.End = transform.InverseTransformPoint(endPos);
        trailLine.Start = transform.InverseTransformPoint(startPos);
    }

    private void updateEndPosition()
    {
        if (trailDistance <= Vector3.Distance(startPos, endPos))
        {
            endPos = startPos + (-transform.forward.normalized * trailDistance);
        }
    }

    private void updateStartPosition()
    {
        startPos = transform.position;
    }


    IEnumerator finishTrailEnd()
    {
        while (trailLine.Start != trailLine.End)
        {
            trailLine.End = Vector3.MoveTowards(trailLine.End, trailLine.Start, trailSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        photonView.RPC("RPC_DestroyBullet", RpcTarget.All);
    }

    [PunRPC]
    void RPC_DestroyBullet() 
    {
        if (!photonView.IsMine)
            return;

        PhotonNetwork.Destroy(gameObject);
    }



    private void finishTrail()
    {
        bulletStopped = true;
        StartCoroutine(finishTrailEnd());
    }
}
