using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletMover : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletRigidbody;
    [SerializeField] private float speed;

    public float Speed { get => speed;}



    /// <summary>
    /// Move bullet forward at a given speed
    /// </summary>
    private void FixedUpdate()
    {
        Vector3 movement = transform.forward * speed * Time.fixedDeltaTime;
        bulletRigidbody.MovePosition(transform.position + movement);
    }

    /// <summary>
    /// Stop the bullet
    /// </summary>
    void disableMovement() 
    {
        enabled = false;
    }

    private void OnEnable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds += disableMovement;
    }
    private void OnDisable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds -= disableMovement;
    }
}
