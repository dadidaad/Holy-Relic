using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// Tower building and operation.
/// </summary>
public class Tower : MonoBehaviour
{
    // Prefab for actions tree
    public GameObject actions;
    // Visualisation of attack or defend range for this tower
    public GameObject range;

    public List<GameObject> prefabsTower;

    // User interface manager
    private UIManager uiManager;
    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("GamePaused", GamePaused);
        EventManager.StartListening("UserClick", UserClick);
        EventManager.StartListening("UserUiClick", UserClick);
        
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("GamePaused", GamePaused);
        EventManager.StopListening("UserClick", UserClick);
        EventManager.StopListening("UserUiClick", UserClick);
    }

    /// <summary>
    /// Atart this instance.
    /// </summary>
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        Debug.Assert(uiManager && actions, "Wrong initial parameters"); 
        CloseActions();
    }

    /// <summary>
    /// Opens the actions tree.
    /// </summary>
    private void OpenActions()
    {
        actions.SetActive(true);
    }

    /// <summary>
    /// Closes the actions tree.
    /// </summary>
    private void CloseActions()
    {
        if (actions.activeSelf == true)
        {
            actions.SetActive(false);
        }
    }

    /// <summary>
    /// Builds the tower.
    /// </summary>
    /// <param name="towerPrefab">Tower prefab.</param>
    public void BuildTower(GameObject towerPrefab)
    {
        // Close active actions tree
        CloseActions();
        Price price = towerPrefab.GetComponent<Price>();
        // If anough gold
        if (uiManager.SpendGold(price.price) == true)
        {
            
            // Create new tower and place it on same position
            GameObject newTower = Instantiate<GameObject>(towerPrefab, transform.parent);
            newTower.name = towerPrefab.name;
            newTower.transform.position = transform.position;
            newTower.transform.rotation = transform.rotation;
           
            UnitInfo unit = newTower.GetComponentInChildren<UnitInfo>();
            AttackRanged ar = newTower.GetComponentInChildren<AttackRanged>();
            ar.damage = (int)Mathf.Floor((ar.damage * 2 * 1) / 5) + 2;

            GameObject parent = transform.parent.gameObject;
            var temp = DataManager.instance.progress.towerInfors.FirstOrDefault(e => e.tag == parent.tag);
            if (temp == null)
            {
                temp = new TowerInfor();
                temp.tag = parent.tag;
                temp.level = 1;
                temp.name = newTower.name;
                DataManager.instance.progress.towerInfors.Add(temp);
            }
            newTower.tag = parent.tag;
           // Destroy old tower
            Destroy(gameObject);
            EventManager.InvokeEvent("TowerBuild", newTower, null);

           
        }
    }
    public void LoadSaveTower(string preFab)
    {
        
        GameObject parent = transform.parent.gameObject;
        var data = DataManager.instance.progress.towerInfors.FirstOrDefault(e => e.tag == parent.tag);
        if (data != null)
        {
            GameObject newTower = Instantiate<GameObject>(prefabsTower.FirstOrDefault(e => e.name == preFab), transform.parent);
            newTower.name = prefabsTower.FirstOrDefault(e => e.name == preFab).name;
            newTower.transform.position = transform.position;
            newTower.transform.rotation = transform.rotation;
            newTower.GetComponentInChildren<UnitInfo>().level = data.level;
            AttackRanged ar = newTower.GetComponentInChildren<AttackRanged>();
            ar.damage = (int)Mathf.Floor((ar.damage * 2 * data.level) / 5) + 2;
            newTower.tag = parent.tag;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Upgrade the tower.
    /// </summary>
    /// <param name="towerPrefab">Tower prefab.</param>
    public void UpgradeTower(GameObject towerPrefab, int level, int updatePrice)
    {
        // Close active actions tree
        CloseActions();
        //Price price = towerPrefab.GetComponent<Price>();
        // If anough gold
        if (uiManager.SpendGold(updatePrice) == true)
        {
            // Create new tower and place it on same position
            GameObject newTower = Instantiate<GameObject>(towerPrefab, transform.parent);
            newTower.name = towerPrefab.name;
            newTower.transform.position = transform.position;
            newTower.transform.rotation = transform.rotation;
            level += 1;
            newTower.GetComponentInChildren<UnitInfo>().level = level;
            
            AttackRanged ar = newTower.GetComponentInChildren<AttackRanged>();
            ar.damage = (int)Mathf.Floor((ar.damage * 2* level) / 5) + 2;
            // Destroy old tower
            Destroy(gameObject);
            GameObject parent = transform.parent.gameObject;
            var temp = DataManager.instance.progress.towerInfors.FirstOrDefault(e => e.tag == parent.tag);
            if (temp == null)
            {
                temp = new TowerInfor();
                temp.tag = parent.tag;
                temp.level = 1;
                temp.name = newTower.name;
                DataManager.instance.progress.towerInfors.Add(temp);
            }
            else
            {
                temp.level = level;
            }
            newTower.tag = parent.tag;
            EventManager.InvokeEvent("UpgradeBuild", newTower, null);
        }
    }

    /// <summary>
    /// Sells the tower with half of price.
    /// </summary>
    /// <param name="emptyPlacePrefab">Empty place prefab.</param>
    public void SellTower(GameObject emptyPlacePrefab)
    {
        CloseActions();
        //DefendersSpawner defendersSpawner = GetComponent<DefendersSpawner>();
        //// Destroy defenders on tower sell
        //if (defendersSpawner != null)
        //{
        //    foreach (KeyValuePair<GameObject, Transform> pair in defendersSpawner.defPoint.activeDefenders)
        //    {
        //        Destroy(pair.Key);
        //    }
        //}
        Price price = GetComponent<Price>();
        uiManager.AddGold(price.price / 2);
        // Place building place
        GameObject newTower = Instantiate<GameObject>(emptyPlacePrefab, transform.parent);
        newTower.name = emptyPlacePrefab.name;
        newTower.transform.position = transform.position;
        newTower.transform.rotation = transform.rotation;
        GameObject parent = transform.parent.gameObject;
        var temp = DataManager.instance.progress.towerInfors.FirstOrDefault(e => e.tag == parent.tag);
        DataManager.instance.progress.towerInfors.Remove(temp);
        // Destroy old tower
        Destroy(gameObject);
        EventManager.InvokeEvent("TowerSell", null, null);
    }

    /// <summary>
    /// Disable tower raycast and close building tree on game pause.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void GamePaused(GameObject obj, string param)
    {
        if (param == bool.TrueString) // Paused
        {
            CloseActions();
        }
    }

    /// <summary>
    /// On user click.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void UserClick(GameObject obj, string param)
    {
        if (obj == gameObject) // This tower is clicked
        {
            // Show range
            ShowRange(true);
            if (actions.activeSelf == false)
            {
                // Open building tree if it is not
                OpenActions();
            }
        }
        else // Other click
        {
            // Hide range
            ShowRange(false);
            // Close active building tree
            CloseActions();
        }
    }

    /// <summary>
    /// Display tower's attack or defend range.
    /// </summary>
    /// <param name="condition">If set to <c>true</c> condition.</param>
	public void ShowRange(bool condition)
    {
        if (range != null)
        {
            range.SetActive(condition);
        }
    }
}
