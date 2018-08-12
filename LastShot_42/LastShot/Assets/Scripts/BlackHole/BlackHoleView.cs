using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleView : MonoBehaviour
{
    public Transform aura;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        aura.transform.Rotate(0.1f, 0.1f, 0.1f);
    }
}
