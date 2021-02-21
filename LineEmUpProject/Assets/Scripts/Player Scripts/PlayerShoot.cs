using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float chargeTime = 5f;
    [SerializeField] private GameObject bulletPrefab;

    PhotonView PV;

    public event Action<float, float> updateVisuals;

    float charge;

    Coroutine charging;

    bool chargeReleasedEarly;


    private void OnEnable()
    {
        if (!PV.IsMine) return;
        PlayerController controller = GetComponent<PlayerController>();
        controller.chargeDown += startCharge;
        controller.chargeRelease += releaseCharge;
    }

    private void OnDisable()
    {
        if (!PV.IsMine) return;
        PlayerController controller = GetComponent<PlayerController>();
        controller.chargeDown -= startCharge;
        controller.chargeRelease -= releaseCharge;
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();   
    }

    private void startCharge()
    {
        charging = StartCoroutine(chargeShot());
    }

    private void releaseCharge() 
    {
        if (charging != null)
        {
            chargeReleasedEarly = true;
        }
    }


    IEnumerator chargeShot() 
    {
        while (charge < chargeTime && !chargeReleasedEarly)
        {
            charge += Time.deltaTime;
            updateVisuals?.Invoke(charge, chargeTime);
            yield return new WaitForEndOfFrame();
        }

        PV.RPC("RPC_shootBullet", RpcTarget.All);
        resetCharge(); 
    }

    

    private void resetCharge()
    {
        chargeReleasedEarly = false;
        charging = null;
        charge = 0;
        updateVisuals?.Invoke(charge, chargeTime);
    }

    [PunRPC]
    private void RPC_shootBullet()
    {
        if (PV.IsMine) 
        {
            GameObject newBullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), transform.position, transform.rotation);
            Bullet[] componentsToSetUp = newBullet.GetComponents<Bullet>();
            
            for (int i = 0; i < componentsToSetUp.Length; i++)
            {
                BulletSetupInfo info = new BulletSetupInfo();
                info.startingPosition = transform.position;
                info.chargeAmmount = charge;
                info.maxChargeAmmount = chargeTime;
                componentsToSetUp[i].setUp(info);
            }
        }
    }
}
