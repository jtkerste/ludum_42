using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public int numLevels = 4;
    public GameObject winStateUI;
    public GameObject inGameUI;
    public GameObject titleUI;

    private int currLevel = 1;
    private StellarBodyController _stellarBodyController;
    private ShipController _shipController;

    // Use this for initialization
    void Start()
    {
        _stellarBodyController = FindObjectOfType<StellarBodyController>();
        _shipController = FindObjectOfType<ShipController>();
        winStateUI.SetActive(false);
        titleUI.SetActive(true);
        inGameUI.SetActive(false);
    }

    public void ToTitle()
    {
        ResetAll();
        foreach (var comp in FindObjectsOfType<PlanetLabelView>())
        {
            comp.GetComponentInChildren<MeshRenderer>().enabled = false;
        }

        inGameUI.SetActive(false);
        titleUI.SetActive(true);
        winStateUI.SetActive(false);
    }

    public void StartGame()
    {
        ResetAll();
        inGameUI.SetActive(true);
        titleUI.SetActive(false);

        foreach (var comp in FindObjectsOfType<PlanetLabelView>())
        {
            comp.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

    public void ResetAll()
    {
        winStateUI.SetActive(false);
        currLevel = 1;
        _stellarBodyController.targetIndex = 0;
        _stellarBodyController.startIndex = 0;
        _stellarBodyController.notNeeded.Clear();
        _shipController.shipStartIndex = 0;
        _shipController.thrustIndex = 0;
        _stellarBodyController.ResetAll();
        _shipController.ShipAte();
        foreach (var comp in FindObjectsOfType<PlanetLabelView>())
        {
            comp.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

    public void ResetLevel()
    {
        _stellarBodyController.ResetKeepBlackHoles();
        _shipController.ShipAte();
        foreach (var comp in FindObjectsOfType<PlanetLabelView>())
        {
            comp.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

    public void RespawnShip()
    {
        _shipController.ShipAte();
    }

    public void LaunchShip()
    {
        _shipController.Launch();
    }

    public void LevelComplete()
    {
        if (currLevel < numLevels)
        {
            currLevel++;
            _stellarBodyController.targetIndex++;
            _stellarBodyController.startIndex++;
            _shipController.thrustIndex++;
            _shipController.shipStartIndex++;
            ResetLevel();
        }
        else
        {
            winStateUI.SetActive(true);
            inGameUI.SetActive(false);
            _stellarBodyController.GrowAll();
        }
    }
}
