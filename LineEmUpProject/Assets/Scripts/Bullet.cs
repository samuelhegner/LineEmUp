using System.Collections;
using UnityEngine;


public abstract class Bullet : MonoBehaviour
{
    public virtual void setUp(BulletSetupInfo info) { }
}

public class BulletSetupInfo 
{
    public float chargeAmmount;
    public float maxChargeAmmount;
    public Vector3 startingPosition;
}
