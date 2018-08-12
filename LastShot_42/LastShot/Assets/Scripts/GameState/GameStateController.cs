using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    private StellarBodyController _stellarBodyController;
    private ShipController _shipController;

    // Use this for initialization
    void Start()
    {
        _stellarBodyController = FindObjectOfType<StellarBodyController>();
        _shipController = FindObjectOfType<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _stellarBodyController.ResetAll();
            _shipController.ShipAte();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_shipController.isMoving)
            {
                _shipController.ShipAte();
            }
            else
            {
                _shipController.Launch();
            }
        }
    }
}
