using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Vector3 initialThrustDirection;
    public float initialThrustForce;

    public float rotationSpeed = 1.0f;
    public ShipModel shipModel;

    private Rigidbody _shipRigidBody;

    public void Start()
    {
        _shipRigidBody = shipModel.GetComponent<Rigidbody>();

        if (_shipRigidBody)
        {
            _shipRigidBody.AddForce(initialThrustDirection.normalized * initialThrustForce);
        }
    }

    public void ApplyGravity(float force, Vector3 position)
    {
        if (!_shipRigidBody) return;


        Vector3 direction = position - shipModel.transform.position;
        _shipRigidBody.AddForce(direction.normalized * force);
    }

}
