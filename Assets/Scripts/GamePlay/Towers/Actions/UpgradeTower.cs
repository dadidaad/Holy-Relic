using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTower : TowerAction
{
    // Prefab for empty building place
    public GameObject towerPrefab;
    public GameObject disableOb;


    // Text field for tower price
    private Text priceText;
    // Price of tower in gold
    private int price = 0;
    // User interface manager allows to check current gold amount
    private UIManager uiManager;

    private int level;


    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        GameObject parent = transform.parent.parent.gameObject;

        level = parent.GetComponentInChildren<UnitInfo>().level;
        priceText = GetComponentInChildren<Text>();
        uiManager = FindObjectOfType<UIManager>();

        price = towerPrefab.GetComponent<Price>().price;
        priceText.text =(level * price).ToString();


        enabledIcon.SetActive(true);  

        Debug.Assert(towerPrefab, "Wrong initial parameters");
    }
    void Update()
    {
        // Mask build icon wich blocking icon if player has not anough gold
        if (enabledIcon == true && disableOb != null)
        {
            if (uiManager.GetGold() >= (price*level))
            {
                disableOb.SetActive(false);
            }
            else
            {
                disableOb.SetActive(true);
            }
        }
    }
    protected override void Clicked()
    {
        if (disableOb.activeSelf == false)
        {
            // Upgrade the tower
            Tower tower = GetComponentInParent<Tower>();
            if (tower != null)
            {
                tower.UpgradeTower(towerPrefab, level, price * level);
            }
        }
    }
}
