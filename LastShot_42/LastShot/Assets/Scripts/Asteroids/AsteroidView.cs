using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidView : MonoBehaviour
{
    public bool dead = false;
    public AsteroidController controller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AsteroidCatch" || other.tag == "StellarBody")
        {
            if (!dead)
            {
                controller.SpawnAsteroid(GetComponent<AsteroidModel>());
                dead = true;
            }
            Destroy(gameObject);
        }
    }
}
