using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipView : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StellarBodyModel model = other.GetComponent<StellarBodyModel>();
        if (model)
        {
            if (model.isTarget)
            {
                // win
                Debug.Log("o.O? win");
            }
        }
    }
}
