using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    public Camera camera;

    public float sizeGrowthRate = 0.01f;
    public float massGrowthRate = 0.01f;

    public float startSize = 1.0f;
    public float startMass = 1.0f;

    public GameObject blackHolePrefab;

    public bool placementAllowed = true;

    public Transform blackHoleContainer;

    private StellarBodyController _stellarBodyController;

    private void Start()
    {
        _stellarBodyController = FindObjectOfType<StellarBodyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(_StartBlackHolePlace(Input.mousePosition));
        }
        if (Input.GetKey(KeyCode.J) && Input.GetKeyDown(KeyCode.K))
        {
            _DestroyAllBlackHoles();
        }
    }

    private IEnumerator _StartBlackHolePlace(Vector3 mousePosition)
    {
        if (_stellarBodyController != null)
        {
            // initialize
            GameObject blackHole = Instantiate(blackHolePrefab);
            blackHole.transform.parent = blackHoleContainer;

            Vector3 position = camera.ScreenToWorldPoint(mousePosition);
            position.z = 0;

            blackHole.transform.position = position;

            StellarBodyModel stellarBodyModel = blackHole.GetComponent<StellarBodyModel>();

            // wait until release
            while (Input.GetMouseButton(0))
            {
                // slowly grow the black hole
                blackHole.transform.localScale += Vector3.one * sizeGrowthRate * Time.deltaTime;
                stellarBodyModel.mass += massGrowthRate * Time.deltaTime;
                yield return null;
            }

            // add the black hole to the stellar body controller
            _stellarBodyController.AddBody(stellarBodyModel);
        }
    }

    private void _DestroyAllBlackHoles()
    {
        BlackHoleModel[] blackHoles = FindObjectsOfType<BlackHoleModel>();
        for (int i = 0; i < blackHoles.Length; i++)
        {
            StellarBodyModel model = blackHoles[i].GetComponent<StellarBodyModel>();
            _stellarBodyController.RemoveBody(model);
            Destroy(blackHoles[i].gameObject);
        }
    }
}
