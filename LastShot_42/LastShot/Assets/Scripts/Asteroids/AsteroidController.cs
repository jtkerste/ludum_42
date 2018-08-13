using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public bool useRealGravity = true;
    public Transform asteroidContainer;
    public GameObject asteroidPrefab;
    public Transform asteroidSpawner;
    public int maxAsteroids = 1;
    public float forceMin = 1.0f;
    public float forceMax = 5.0f;

    private List<Vector3> _spawnPositions = new List<Vector3>();
    public int numAsteroids = 0;

    public List<AsteroidModel> _asteroids = new List<AsteroidModel>();

    private void Start()
    {
        foreach (Transform t in asteroidSpawner)
        {
            _spawnPositions.Add(t.position);
        }

        for (int i = 0; i < maxAsteroids; i++)
        {
            SpawnAsteroid(null);
        }
    }

    public void SpawnAsteroid(AsteroidModel prev)
    {
        if (prev)
        {
            _asteroids.Remove(prev);
        }

        // pick a random spawner position
        int index = Random.Range(0, _spawnPositions.Count);

        Vector3 position = _spawnPositions[index];

        GameObject asteroid = Instantiate(asteroidPrefab);
        asteroid.transform.parent = asteroidContainer;
        asteroid.transform.position = position;

        // get a random direction and force
        Vector3 dir = new Vector3(
            Random.Range(-1.0f, 1.0f),
            Random.Range(-1.0f, 1.0f),
            0.0f
        );
        float force = Random.Range(forceMin, forceMax);
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        rb.AddForce(dir.normalized * force);

        AsteroidView view = asteroid.GetComponent<AsteroidView>();
        AsteroidModel model = asteroid.GetComponent<AsteroidModel>();
        _asteroids.Add(model);
        view.controller = this;
        numAsteroids++;
    }

    public void ApplyGravity(float mass, Vector3 position)
    {
        foreach (AsteroidModel model in _asteroids)
        {
            Rigidbody rb = model.GetComponent<Rigidbody>();
            if (!rb || !model) return;

            // calculate the force
            float massShip = model.mass;

            Vector3 positionShip = model.transform.position;

            float distance = Vector3.Distance(positionShip, position);

            float force = 0.0f;
            if (useRealGravity)
            {
                force = (massShip * mass) / (distance * distance);
            }
            else
            {
                force = (massShip * mass) / (distance);
            }

            Vector3 direction = position - model.transform.position;
            rb.AddForce(direction.normalized * force);
        }
    }
}
