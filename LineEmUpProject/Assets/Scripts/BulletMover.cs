using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletMover : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletRigidbody;
    [SerializeField] private float speed;

    public float Speed { get => speed;}

    private void OnEnable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds += disableMovement;
    }
    private void OnDisable()
    {
        GetComponent<BulletOffScreenChecker>().bulletOutOfBounds -= disableMovement;
    }

    private void FixedUpdate()
    {
        Vector3 movement = transform.forward * speed * Time.fixedDeltaTime;
        bulletRigidbody.MovePosition(transform.position + movement);
    }

    void disableMovement() 
    {
        enabled = false;
    }
}
