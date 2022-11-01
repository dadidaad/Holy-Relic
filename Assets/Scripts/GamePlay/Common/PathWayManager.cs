using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathWayManager : MonoBehaviour
{
    private List<Pathway> paths;
    private bool flag1 = false;
    private bool flag2 = false;
    private bool flag3 = false;
    private bool flag4 = false;
    private bool flag5 = false;

    private void Start()
    {
        Pathway[] temp = GetComponentsInChildren<Pathway>();
        paths = new List<Pathway>(temp);
    }

    private void Update()
    {
        if (DataManager.instance.progress.currentWave >= 3 && !flag1)
        {
            Pathway path1 = paths.FirstOrDefault(e=>e.name=="Pathway (1)");
            SpawnPoint spawnPoint1 = path1.GetComponentInChildren<SpawnPoint>();
            spawnPoint1.WaveStart(this.gameObject, "continue");
            flag1 = true;
        }
        if (DataManager.instance.progress.currentWave >= 5 && !flag2)
        {
            Pathway path2 = paths.FirstOrDefault(e => e.name == "Pathway (2)");
            SpawnPoint spawnPoint2 = path2.GetComponentInChildren<SpawnPoint>();
            spawnPoint2.WaveStart(this.gameObject, "continue");
            flag2 = true;
        }
        if (DataManager.instance.progress.currentWave >= 10 && !flag3)
        {
            Pathway path3 = paths.FirstOrDefault(e => e.name == "Pathway (3)");
            SpawnPoint spawnPoint3 = path3.GetComponentInChildren<SpawnPoint>();
            spawnPoint3.WaveStart(this.gameObject, "continue");
            flag3 = true;
        }
        if (DataManager.instance.progress.currentWave >= 20 && !flag4)
        {
            Pathway path4 = paths.FirstOrDefault(e => e.name == "Pathway (4)");
            SpawnPoint spawnPoint4 = path4.GetComponentInChildren<SpawnPoint>();
            spawnPoint4.WaveStart(this.gameObject, "continue");
            flag4 = true;
        }
        if (DataManager.instance.progress.currentWave >= 25 && !flag5)
        {
            Pathway path5 = paths.FirstOrDefault(e => e.name == "Pathway (5)");
            SpawnPoint spawnPoint5 = path5.GetComponentInChildren<SpawnPoint>();
            spawnPoint5.WaveStart(this.gameObject, "continue");
            flag5 = true;
        }

    }
}
