using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTower : TowerAction
{
    // Prefab for empty building place
    public GameObject towerPrefab;

    public int level = 1;
    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        Debug.Assert(towerPrefab, "Wrong initial parameters");
    }

    protected override void Clicked()
    {
        level += 1;
        // Upgrade the tower
        Tower tower = GetComponentInParent<Tower>();
        if (tower != null)
        {
            tower.UpgradeTower(towerPrefab, level);
        }
    }
}
