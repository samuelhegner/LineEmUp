using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float chargeTime = 5f;

    [Header("Bullet Prefabs")]
    [SerializeField] private GameObject bulletPrefab;

    PhotonView photonView;

    public event Action<float, float> updateVisuals; //event to update the player visuals when charging a shot

    float charge;

    Coroutine charging;

    bool chargeReleasedEarly;



    private void Awake()
    {
        photonView = GetComponent<PhotonView>();   
    }

    /// <summary>
    /// When the shoot button is pressed down
    /// </summary>
    private void startCharge()
    {
        charging = StartCoroutine(chargeShot());
    }


    /// <summary>
    /// When the shoot button is released
    /// </summary>
    private void releaseCharge() 
    {
        if (charging != null)
        {
            chargeReleasedEarly = true;
        }
    }

    /// <summary>
    /// Reset Variables for the next shot
    /// </summary>
    private void resetCharge()
    {
        chargeReleasedEarly = false;
        charging = null;
        charge = 0;
        updateVisuals?.Invoke(charge, chargeTime);//update the players visuals
    }

    /// <summary>
    /// Coroutine to charge a shot up, whilst the player holds shoot
    /// </summary>
    /// <returns></returns>
    IEnumerator chargeShot() 
    {
        while (charge < chargeTime && !chargeReleasedEarly)
        {
            charge += Time.deltaTime;
            updateVisuals?.Invoke(charge, chargeTime);
            yield return new WaitForEndOfFrame();
        }

        photonView.RPC("RPC_shootBullet", RpcTarget.All);
        resetCharge(); 
    }



    [PunRPC]
    private void RPC_shootBullet()
    {
        if (photonView.IsMine) // if this is on the local player, shoot a bullet
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y = 0.5f;

            GameObject newBullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), spawnPos, transform.rotation); //Create the bullet at the spawn point
           
            Bullet[] componentsToSetUp = newBullet.GetComponents<Bullet>();//All of the bullet scripts that need to set up based on the charge ammount
            
            for (int i = 0; i < componentsToSetUp.Length; i++) //Setup all of the bullets component with an information package
            {
                BulletSetupInfo info = new BulletSetupInfo();
                info.startingPosition = transform.position;
                info.chargeAmmount = charge;
                info.maxChargeAmmount = chargeTime;
                componentsToSetUp[i].setUp(info);
            }
        }
    }

    private void OnEnable()
    {
        if (!photonView.IsMine) return; //only allow shooting on local player
        PlayerController controller = GetComponent<PlayerController>();
        controller.chargeDown += startCharge;
        controller.chargeRelease += releaseCharge;
    }

    private void OnDisable()
    {
        if (!photonView.IsMine) return;
        PlayerController controller = GetComponent<PlayerController>();
        controller.chargeDown -= startCharge;
        controller.chargeRelease -= releaseCharge;
    }
}
