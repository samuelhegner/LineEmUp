using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float chargeTime = 5f;
    [SerializeField] private GameObject bulletPrefab;

    public event Action<float, float> updateVisuals;

    float charge;

    Coroutine charging;

    bool chargeReleasedEarly;


    private void OnEnable()
    {
        PlayerController controller = GetComponent<PlayerController>();
        controller.chargeDown += startCharge;
        controller.chargeRelease += releaseCharge;

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

        shootBullet();
        resetCharge(); 
    }

    private void shootBullet()
    {
        GameObject newBullet = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation); //TODO: Object Pool for performance
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

    private void resetCharge()
    {
        chargeReleasedEarly = false;
        charging = null;
        charge = 0;
        updateVisuals?.Invoke(charge, chargeTime);
    }
}
