using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarBodyModel : MonoBehaviour
{
    public bool isTarget = false;
    public bool isNeeded = false;
    public float mass = 1.0f;
    public float timeCreated = 0;

    public void Start()
    {
        timeCreated = Time.time;
    }
}
