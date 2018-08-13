using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public StellarBodyModel activeBody;

    private void Start()
    {
        _stellarBodyController = FindObjectOfType<StellarBodyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ignore placement if over ui
            if(!EventSystem.current.IsPointerOverGameObject())
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
            blackHole.transform.localScale = Vector3.one * startSize;
            Vector3 position = camera.ScreenToWorldPoint(mousePosition);
            position.z = 0;

            blackHole.transform.position = position;

            StellarBodyModel stellarBodyModel = blackHole.GetComponent<StellarBodyModel>();
            stellarBodyModel.mass = startMass;

            // add the black hole to the stellar body controller
            _stellarBodyController.AddBody(stellarBodyModel);

            // wait until release
            while (Input.GetMouseButton(0))
            {
                // slowly grow the black hole
                stellarBodyModel.mass += massGrowthRate * Time.deltaTime;
                stellarBodyModel.transform.localScale = Vector3.one * stellarBodyModel.mass / 5.0f;
                yield return null;
            }   
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
