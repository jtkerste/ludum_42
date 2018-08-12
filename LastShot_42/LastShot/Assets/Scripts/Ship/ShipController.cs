using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public bool isMoving = false;

    public bool useRealGravity = false;

    public Vector3 initialThrustDirection;
    public float initialThrustForce;

    public float rotationSpeed = 1.0f;

    public GameObject shipPrefab;
    public Transform _initialShipTransform;

    private ShipModel _shipModel;
    private GameObject _ship;
    private Rigidbody _shipRigidBody;


    private bool _launched = false;

    public void Start()
    {
        _SetUpShip();
    }

    public void ApplyGravity(float massPlanet, Vector3 positionPlanet)
    {
        if (!_shipRigidBody || !_shipModel) return;

        // calculate the force
        float massShip = _shipModel.mass;

        Vector3 positionShip = _shipModel.transform.position;

        float distance = Vector3.Distance(positionShip, positionPlanet);

        float force = 0.0f;
        if (useRealGravity)
        {
            force = (massShip * massPlanet) / (distance * distance);
        }
        else
        {
            force = (massShip * massPlanet) / (distance);
        }

        Vector3 direction = positionPlanet - _shipModel.transform.position;
        _shipRigidBody.AddForce(direction.normalized * force);
    }

    private void _SetUpShip()
    {
        _ship = Instantiate(shipPrefab);
        _ship.transform.position = _initialShipTransform.position;
        _ship.transform.rotation = _initialShipTransform.rotation;

        _shipModel = _ship.GetComponent<ShipModel>();
        _shipRigidBody = _ship.GetComponent<Rigidbody>();

        _shipRigidBody.isKinematic = true;
        _launched = false;
        isMoving = false;
    }

    public void Launch()
    {
        if (_shipRigidBody && !_launched)
        {
            _shipRigidBody.isKinematic = false;
            _shipRigidBody.AddForce(initialThrustDirection.normalized * initialThrustForce);
            _launched = true;
            isMoving = true;
        }
    }

    public void ShipAte()
    {
        Destroy(_ship);
        isMoving = false;
        _SetUpShip();
    }
}
