using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BuildingPlaceInspector : MonoBehaviour
{
    // Tower of this building place
    private GameObject myTower;


    void OnEnable()
    {
        myTower = GetComponentInChildren<Tower>().gameObject;
        Debug.Assert(myTower , "Wrong stuff settings");
    }
	// Start is called before the first frame update
	public GameObject ChooseTower(GameObject towerPrefab)
	{
		// Destroy old tower
		if (myTower != null)
		{
			DestroyImmediate(myTower);
		}
		// Create new tower
		myTower = Instantiate(towerPrefab, transform);
		myTower.name = towerPrefab.name;
		myTower.transform.SetAsLastSibling();
		return myTower;
	}
}
