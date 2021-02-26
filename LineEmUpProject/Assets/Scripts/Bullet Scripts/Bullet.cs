using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract class that allows for easy setup of a new bullet with a certain level of charge
/// </summary>
public abstract class Bullet : MonoBehaviour
{
    public virtual void setUp(BulletSetupInfo info) { }
}

/// <summary>
/// Class that contains all of the required information to set up the bullet
/// </summary>
public class BulletSetupInfo 
{
    public float chargeAmmount;
    public float maxChargeAmmount;
    public Vector3 startingPosition;
}
