using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarBodyController : MonoBehaviour
{
    public List<int> targetList = new List<int>();
    public int targetIndex = 0;
    public List<int> startList = new List<int>();
    public int startIndex = 0;
    public List<string> planetLabels = new List<string>();
    public Transform planetContainer;
    public GameObject planetPrefab;
    public GameObject planetLabel;
    public GameObject startPrefab;
    public Transform planetConfig;

    private ShipController _shipController;
    private AsteroidController _asteroidController;

    public List<int> notNeeded = new List<int>();

    private List<StellarBodyModel> _stellarBodyModels = new List<StellarBodyModel>();

    // Use this for initialization
    void Start()
    {
        _shipController = FindObjectOfType<ShipController>();
        _asteroidController = FindObjectOfType<AsteroidController>();

        _Initialize();
    }

    public void ResetAll()
    {
        StopAllCoroutines();
        foreach (StellarBodyModel model in _stellarBodyModels)
        {
            Destroy(model.gameObject);
        }
        _stellarBodyModels.Clear();
        _Initialize();
    }

    public void ResetKeepBlackHoles()
    {
        List<StellarBodyModel> blackHoles = new List<StellarBodyModel>();
        foreach (StellarBodyModel model in _stellarBodyModels)
        {
            if (model.gameObject.name.Contains("BlackHole"))
            {
                blackHoles.Add(model);
            }
            else
            {
                Destroy(model.gameObject);
            }
        }
        _stellarBodyModels.Clear();
        foreach (StellarBodyModel model in blackHoles)
        {
            _stellarBodyModels.Add(model);
        }
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
                state.localEulerAngles.x,
                state.localEulerAngles.y,
                state.localEulerAngles.z
            );
            StellarBodyModel model = planet.GetComponent<StellarBodyModel>();

            if (i == startList[startIndex])
            {
                //GameObject start = Instantiate(startPrefab);
                //start.transform.parent = planet.transform;
                //start.transform.localPosition = new Vector3(0, 0, -3);
            }
            if (i == targetList[targetIndex])
            {
                model.isTarget = true;

                // indicate the target
                //GameObject target = Instantiate(targetPrefab);
                //target.transform.parent = planet.transform;
                //target.transform.localPosition = new Vector3(0, 0, -3);
            }
            else
            {
                model.isTarget = false;
            }

            if (!string.IsNullOrEmpty(planetLabels[i]))
            {
                GameObject label = Instantiate(planetLabel);
                label.transform.parent = planet.transform;
                label.transform.localPosition = new Vector3(0, 0, -4);
                TextMesh text = label.GetComponentInChildren<TextMesh>();
                text.text = planetLabels[i];
                if (!notNeeded.Contains(i))
                {
                    model.isNeeded = true;
                }
            }
            model.mass = planet.transform.localScale.x * 10;
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

            _asteroidController.ApplyGravity(massPlanet, positionPlanet);
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
        if (!_stellarBodyModels.Contains(one) || !_stellarBodyModels.Contains(two)) return;
        one.mass += two.mass;
        one.transform.localScale = Vector3.one * one.mass / 5.0f;
        if (two.isNeeded)
        {
            StartCoroutine(_Grow(one));
        }
        _stellarBodyModels.Remove(two);
        Destroy(two.gameObject);
    }

    public IEnumerator _Grow(StellarBodyModel model)
    {
        while (gameObject)
        {
            model.mass += 0.4f;
            model.transform.localScale = Vector3.one * model.mass / 5.0f;
            yield return null;
        }
    }

    public void ThisEatsShip(StellarBodyModel one, ShipModel ship)
    {
        one.mass += 0.5f;
        one.transform.localScale = Vector3.one * one.mass / 5.0f;
    }

    public void AddMassTo(StellarBodyModel one, float mass)
    {
        one.mass += mass;
        one.transform.localScale = Vector3.one * one.mass / 5.0f;
    }

    public void GrowAll()
    {
        foreach (StellarBodyModel model in _stellarBodyModels)
        {
            if (model.name.Contains("BlackHole"))
            {
                StartCoroutine(_Grow(model));
            }
        }
    }

    public void ActiveModelNotNeeded()
    {
        notNeeded.Add(startList[targetIndex]);
    }
}