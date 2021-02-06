using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdjustVisuals : MonoBehaviour
{
    [SerializeField] Light playerLight;
    [SerializeField] float minLightIntensity;
    [SerializeField] float maxLightIntensity;

    private void OnEnable()
    {
        GetComponent<PlayerShoot>().updateVisuals += updateLightIntensity;
    }

    private void OnDisable()
    {
        GetComponent<PlayerShoot>().updateVisuals -= updateLightIntensity;
    }

    void updateLightIntensity(float charge, float maxCharge) 
    {
        playerLight.intensity = FloatExtensions.Map(charge, 0, maxCharge, minLightIntensity, maxLightIntensity);
    }
}
