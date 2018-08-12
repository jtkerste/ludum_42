using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarBodyController : MonoBehaviour
{
    private ShipController _shipController;

    private List<StellarBodyModel> _stellarBodyModels = new List<StellarBodyModel>();

    // Use this for initialization
    void Start()
    {
        _shipController = FindObjectOfType<ShipController>();

        _stellarBodyModels = new List<StellarBodyModel>(FindObjectsOfType<StellarBodyModel>());
    }

    // Update is called once per frame
    void Update()
    {
        if (_shipController == null) return;

        // determine the gravitational force on the ship from each planet
        for (int i = 0; i < _stellarBodyModels.Count; i++)
        {
            float massPlanet = _stellarBodyModels[i].mass;
            Vector3 positionPlanet = _stellarBodyModels[i].transform.position;

            // apply the force
            _shipController.ApplyGravity(massPlanet, positionPlanet);
        }
    }

    public void RemoveBody(StellarBodyModel model)
    {
        _stellarBodyModels.Remove(model);
    }

    public void AddBody(StellarBodyModel model)
    {
        _stellarBodyModels.Add(model);
    }
}