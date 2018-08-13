using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipView : MonoBehaviour
{
    public bool levelComplete = false;
    private GameStateController _gameStateController;
    private StellarBodyController _stellarBodyController;

    private void Start()
    {
        _gameStateController = FindObjectOfType<GameStateController>();
        _stellarBodyController = FindObjectOfType<StellarBodyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StellarBodyModel model = other.GetComponent<StellarBodyModel>();
        if (model)
        {
            if (model.isTarget)
            {
                // win
                if (!levelComplete)
                {
                    _stellarBodyController.ActiveModelNotNeeded();
                    _gameStateController.LevelComplete();
                    levelComplete = true;
                }
            }
        }
    }
}
