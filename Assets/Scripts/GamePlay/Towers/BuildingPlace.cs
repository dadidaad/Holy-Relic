using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingPlace : MonoBehaviour
{
    private void Start()
    {
        string tag = this.gameObject.tag;
        TowerInfor towerInfor = DataManager.instance.progress.towerInfors.FirstOrDefault(e => e.tag == tag);
        if (towerInfor != null)
        {
            Tower tower = GetComponentInChildren<Tower>();
            tower.LoadSaveTower("KingDra");
        }
    }
}
