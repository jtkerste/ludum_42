using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public bool useRealGravity = false;

    private ShipController _shipController;
    private ShipModel _shipModel;

    private List<PlanetModel> _planetModels = new List<PlanetModel>();

    // Use this for initialization
    void Start()
    {
        _shipController = FindObjectOfType<ShipController>();
        _shipModel = FindObjectOfType<ShipModel>();

        _planetModels = new List<PlanetModel>(FindObjectsOfType<PlanetModel>());
    }

    // Update is called once per frame
    void Update()
    {
        if (_shipModel == null || _shipController == null) return;

        // determine the gravitational force on the ship from each planet
        for (int i = 0; i < _planetModels.Count; i++)
        {
            // calculate the force
            float massShip = _shipModel.mass;
            float massPlanet = _planetModels[i].mass;

            Vector3 positionShip = _shipModel.transform.position;
            Vector3 positionPlanet = _planetModels[i].transform.position;

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

            // apply the force
            _shipController.ApplyGravity(force, positionPlanet);
        }
    }
}