using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleView : MonoBehaviour
{
    public StellarBodyModel stellarBodyModel;
    public Transform aura;

    private StellarBodyController _stellarBodyController;
    private BlackHoleController _blackHoleController;
    private ShipController _shipController;

    // Use this for initialization
    void Start()
    {
        _stellarBodyController = FindObjectOfType<StellarBodyController>();
        _shipController = FindObjectOfType<ShipController>();
        _blackHoleController = FindObjectOfType<BlackHoleController>();

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StellarBody")
        {
            StellarBodyModel otherModel = other.GetComponent<StellarBodyModel>();
            if (other.name.Contains("BlackHole"))
            {
                // make sure we don't eat the one we are placing
                if (stellarBodyModel.timeCreated < otherModel.timeCreated)
                {
                    _stellarBodyController.ThisEatsThat(otherModel, stellarBodyModel);
                }
                else
                {
                    _stellarBodyController.ThisEatsThat(stellarBodyModel, otherModel);
                }
            }
            else
            {
                _stellarBodyController.ThisEatsThat(stellarBodyModel, other.GetComponent<StellarBodyModel>());
            }
        }
        else if (other.tag == "Ship")
        {
            _stellarBodyController.ThisEatsShip(stellarBodyModel, other.GetComponent<ShipModel>());
            _shipController.ShipAte();
        }
        else if (other.tag == "Asteroid")
        {
            _stellarBodyController.AddMassTo(stellarBodyModel, other.GetComponent<AsteroidModel>().mass);
        }
    }

    // Update is called once per frame
    void Update()
    {
        aura.transform.Rotate(0.1f, 0.1f, 0.1f);
    }
}
