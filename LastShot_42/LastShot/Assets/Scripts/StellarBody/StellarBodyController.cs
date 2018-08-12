using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarBodyController : MonoBehaviour
{
    public int targetIndex = 0;
    public Transform planetContainer;
    public GameObject planetPrefab;
    public Transform planetConfig;
    private ShipController _shipController;

    private List<StellarBodyModel> _stellarBodyModels = new List<StellarBodyModel>();

    // Use this for initialization
    void Start()
    {
        _shipController = FindObjectOfType<ShipController>();

        _Initialize();
    }

    public void ResetAll()
    {
        foreach (StellarBodyModel model in _stellarBodyModels)
        {
            Destroy(model.gameObject);
        }
        _stellarBodyModels.Clear();
        _Initialize();
    }

    private void _Initialize()
    {
        // spawn our planets
        for(int i = 0; i < planetConfig.childCount; i++)
        {
            Transform state = planetConfig.GetChild(i);
            GameObject planet = Instantiate(planetPrefab);
            planet.transform.parent = planetContainer;
            planet.transform.position = state.position;
            planet.transform.localScale = state.localScale;
            planet.GetComponentInChildren<Renderer>().material.color = new Color(
                state.eulerAngles.x,
                state.eulerAngles.y,
                state.eulerAngles.z
            );
            StellarBodyModel model = planet.GetComponent<StellarBodyModel>();
            if (i == targetIndex)
            {
                model.isTarget = true;
            }
            else
            {
                model.isTarget = false;
            }
            model.mass = planet.transform.localScale.x;
            _stellarBodyModels.Add(planet.GetComponent<StellarBodyModel>());
        }
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

    public void ThisEatsThat(StellarBodyModel one, StellarBodyModel two)
    {
        if (!_stellarBodyModels.Contains(one)) return;
        one.mass += two.mass;
        one.transform.localScale = Vector3.one * one.mass;
        _stellarBodyModels.Remove(two);
        Destroy(two.gameObject);
    }

    public void ThisEatsShip(StellarBodyModel one, ShipModel ship)
    {
        one.mass += ship.mass;
        one.transform.localScale = Vector3.one * one.mass;
    }
}