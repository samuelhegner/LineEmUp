using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BulletAdjustVisuals : Bullet, IPunObservable
{
    [SerializeField] private HDAdditionalLightData bulletLight;
    float lightIntensity;
    [SerializeField] float minLightIntensity;
    [SerializeField] float maxLightIntensity;

    [SerializeField] private BulletTrail trail;
    float trailLength;
    [SerializeField] float minTrailDistance;
    [SerializeField] float maxTrailDistance;

    /// <summary>
    /// Sets up the bullets visuals, once the bullet is instanciated with a certain charge ammount
    /// </summary>
    /// <param name="info"></param>
    public override void setUp(BulletSetupInfo info)
    {
        lightIntensity = FloatExtensions.Map(info.chargeAmmount, 0, info.maxChargeAmmount, minLightIntensity, maxLightIntensity); //set the bullets light to map from max to mix
        trailLength = FloatExtensions.Map(info.chargeAmmount, 0, info.maxChargeAmmount, minTrailDistance, maxTrailDistance); // set the trail length to map from max to mix
        bulletLight.intensity = lightIntensity; //Set the new intensity
        trail.SetTrailDistance(trailLength); //Set the new trail length
    }


    //Update the visuals of the bullet for all players.
    //Not sure what best practice is, in terms of using IPunObservable or using RPCs
    //TODO: Research best practice and when to use each case
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(lightIntensity);
            stream.SendNext(trailLength);
        }
        else
        {
            //Network player, receive data
            bulletLight.intensity = (float)stream.ReceiveNext();
            trailLength = (float)stream.ReceiveNext();
            trail.SetTrailDistance(trailLength);
        }
    }
}
