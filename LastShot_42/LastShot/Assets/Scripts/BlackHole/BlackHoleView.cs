using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleView : MonoBehaviour
{
    public StellarBodyModel stellarBodyModel;
    public Transform aura;

    private StellarBodyController _stellarBodyController;
    private ShipController _shipController;

    // Use this for initialization
    void Start()
    {
        _stellarBodyController = FindObjectOfType<StellarBodyController>();
        _shipController = FindObjectOfType<ShipController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StellarBody")
        {
            _stellarBodyController.ThisEatsThat(stellarBodyModel, other.GetComponent<StellarBodyModel>());
        }
        else if (other.tag == "Ship")
        {
            _stellarBodyController.ThisEatsShip(stellarBodyModel, other.GetComponent<ShipModel>());
            _shipController.ShipAte();
        }
    }

    // Update is called once per frame
    void Update()
    {
        aura.transform.Rotate(0.1f, 0.1f, 0.1f);
    }
}
