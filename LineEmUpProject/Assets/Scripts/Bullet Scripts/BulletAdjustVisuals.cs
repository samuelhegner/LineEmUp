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


    public override void setUp(BulletSetupInfo info)
    {
        lightIntensity = FloatExtensions.Map(info.chargeAmmount, 0, info.maxChargeAmmount, minLightIntensity, maxLightIntensity);
        trailLength = FloatExtensions.Map(info.chargeAmmount, 0, info.maxChargeAmmount, minTrailDistance, maxTrailDistance);
        bulletLight.intensity = lightIntensity;
        print("Set Light to: " + lightIntensity);
        trail.SetTrailDistance(trailLength);
    }

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
