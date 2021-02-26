using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Photon.Pun;
using System;

public class PlayerAdjustVisuals : MonoBehaviour, IPunObservable
{
    [Header("Light Settings")]
    [SerializeField] HDAdditionalLightData playerLight;
    [SerializeField] float minLightIntensity;
    [SerializeField] float maxLightIntensity;

    float currentIntensity;

    private void Start()
    {
        currentIntensity = minLightIntensity;
    }

    void updateLightIntensity(float charge, float maxCharge) 
    {
        currentIntensity = FloatExtensions.Map(charge, 0, maxCharge, minLightIntensity, maxLightIntensity);
    }

    private void Update()
    {
        setLightIntensity(currentIntensity);
    }

    /// <summary>
    /// Lerp the light intensity to the new light intensity
    /// </summary>
    /// <param name="currentIntensity"></param>
    private void setLightIntensity(float currentIntensity) //created to make the light changing smoother with networking implementation
    {
        if (currentIntensity != minLightIntensity) 
        {
            playerLight.intensity = Mathf.Lerp(playerLight.intensity, currentIntensity, Time.deltaTime * 20f);
        }
        else
        {
            playerLight.intensity = currentIntensity;
        }
    }

    //Update the visuals of the player for other players.
    //Not sure what best practice is, in terms of using IPunObservable or using RPCs
    //TODO: Research best practice and when to use each case
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(currentIntensity);
        }
        else
        {
            //Network player, receive data
            currentIntensity = (float)stream.ReceiveNext();
        }
    }

    private void OnEnable()
    {
        GetComponent<PlayerShoot>().updateVisuals += updateLightIntensity;
    }

    private void OnDisable()
    {
        GetComponent<PlayerShoot>().updateVisuals -= updateLightIntensity;
    }
}
