using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAdjustVisuals : Bullet
{
    [SerializeField] private Light bulletLight;
    [SerializeField] float minLightIntensity;
    [SerializeField] float maxLightIntensity;

    [SerializeField] private BulletTrail trail;
    [SerializeField] float minTrailDistance;
    [SerializeField] float maxTrailDistance;
    public override void setUp(BulletSetupInfo info)
    {
        bulletLight.intensity = FloatExtensions.Map(info.chargeAmmount, 0, info.maxChargeAmmount, minLightIntensity, maxLightIntensity);
        trail.TrailDistance = FloatExtensions.Map(info.chargeAmmount, 0, info.maxChargeAmmount, minTrailDistance, maxTrailDistance);
    }
}
