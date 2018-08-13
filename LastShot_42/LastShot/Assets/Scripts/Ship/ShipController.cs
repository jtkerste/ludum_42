using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public bool isMoving = false;

    public bool useRealGravity = false;

    public Vector3 initialThrustDirection;
    public List<float> initialThrustForces = new List<float>();
    public int thrustIndex = 0;

    public float rotationSpeed = 1.0f;

    public GameObject shipPrefab;
    public Transform shipStartContainer;
    private List<Transform> _shipStartTransforms = new List<Transform>();
    public int shipStartIndex = 0;

    private ShipModel _shipModel;
    private GameObject _ship;
    private Rigidbody _shipRigidBody;


    private bool _launched = false;

    public void Start()
    {
        foreach (Transform t in shipStartContainer)
        {
            _shipStartTransforms.Add(t);
        }
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
        _ship.transform.position = _shipStartTransforms[shipStartIndex].position;
        _ship.transform.rotation = _shipStartTransforms[shipStartIndex].rotation;

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
            Vector3 dir = _ship.transform.up;
            _shipRigidBody.AddForce(dir.normalized * initialThrustForces[thrustIndex]);
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
