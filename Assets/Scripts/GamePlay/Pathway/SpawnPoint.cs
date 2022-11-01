using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        // Delay before wave run
        public float delayWave;
        // List of enemies in this wave
        public List<GameObject> enemies = new List<GameObject>();
    }

    public float speed = 0.15f;
    public float unitSpawnDelay = 2f;
    public List<Wave> waves;
    public bool endlessWave = false;


    [HideInInspector]
    public List<GameObject> randomEnemiesList = new List<GameObject>();
    private Pathway path;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool finished = false;
    private UIManager uiManager;

    void Awake()
    {
        path = GetComponentInParent<Pathway>();
        Debug.Assert(path != null, "Settings fail");
    }

    void OnEnable()
    {
        EventManager.StartListening("EnemiesDie", EnemiesDie);
        EventManager.StartListening("WaveStart", WaveStart);
    }

    void OnDisable()
    {
        EventManager.StopListening("EnemiesDie", EnemiesDie);
        EventManager.StopListening("WaveStart", WaveStart);
    }
    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    private void EnemiesDie(GameObject obj, string param)
    {
        // If this is active enemy
        if (activeEnemies.Contains(obj) == true)
        {
            // Remove it from buffer
            activeEnemies.Remove(obj);
        }
    }
    private IEnumerator RunWave(int waveIdx)
    {
        if (endlessWave == true)
        {
            int enemyInWave = 3;
            int currentWave = 0;
            if(DataManager.instance.progress.currentWave != 0)
            {
                currentWave = DataManager.instance.progress.currentWave;
                enemyInWave = DataManager.instance.progress.enemiesInWave;
            }
            while (true)
            {
                currentWave += 1;
                if (this.GetComponentInParent<Pathway>().name == "Pathway")
                {
                    DataManager.instance.progress.currentWave = currentWave-1;
                    DataManager.instance.progress.enemiesInWave = enemyInWave;
                    uiManager.setWaveText(currentWave);
                }

                yield return new WaitForSeconds(5);
                Debug.Log("Wave: " + currentWave + " - Enemy: " + enemyInWave);
                int random = Random.Range(0, 3);
                for (int i = 0; i < enemyInWave; i++)
                {
                    GameObject prefab = null;
                    // If enemy prefab not specified - spawn random enemy
                    if (randomEnemiesList.Count > 0)
                    {
                        if (currentWave < 10)
                            prefab = EnemySelection(currentWave, 0);
                        else
                            prefab = EnemySelection(currentWave, random);
                    }
                    else
                    {
                        Debug.LogError("Have no enemy prefab. Please specify enemies in Level Manager or in Spawn Point");
                    }
                    // Create enemy
                    GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
                    newEnemy.name = prefab.name;
                    DamageTaker dt = newEnemy.GetComponent<DamageTaker>();
                    if (currentWave % 2 == 0)
                    {
                        dt.hitpoints = prefab.GetComponent<DamageTaker>().hitpoints + ((currentWave - 1) * 2);
                    }
                    else if (currentWave != 1 && currentWave % 2 != 0)
                    {
                        dt.hitpoints = prefab.GetComponent<DamageTaker>().hitpoints + ((currentWave - 2) * 2);
                    }
                    dt.currentHitpoints = dt.hitpoints;
                    // Set pathway
                    newEnemy.GetComponent<AiStatePatrol>().path = path;
                    NavAgent agent = newEnemy.GetComponent<NavAgent>();
                    // Set speed offset
                    if (currentWave % 10 == 0)
                    {
                        speed += 0.05f;
                    }
                    // Add enemy to list
                    activeEnemies.Add(newEnemy);
                    // Wait for delay before next enemy run
                    yield return new WaitForSeconds(unitSpawnDelay);
                }
                if (currentWave % 4 == 0)
                    enemyInWave += 1;
            }
        }
        else
        {
            if (waves.Count > waveIdx)
            {
                //yield return new WaitForSeconds(waves[waveIdx].delayBeforeWave);
                Debug.Log("Wave: " + waveIdx + " - Enemy: " + waves[waveIdx].enemies.Count * 2);
                foreach (GameObject enemy in waves[waveIdx].enemies)
                {
                    GameObject prefab;
                    prefab = enemy;
                    // If enemy prefab not specified - spawn random enemy
                    if (prefab == null && randomEnemiesList.Count > 0)
                    {
                        prefab = randomEnemiesList[Random.Range(0, randomEnemiesList.Count)];
                    }
                    if (prefab == null)
                    {
                        Debug.LogError("Have no enemy prefab. Please specify enemies in Level Manager or in Spawn Point");
                    }
                    // Create enemy
                    GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
                    newEnemy.name = prefab.name;
                    DamageTaker dt = newEnemy.GetComponent<DamageTaker>();
                    if (waveIdx != 0 && waveIdx % 2 == 0)
                    {
                        dt.hitpoints = prefab.GetComponent<DamageTaker>().hitpoints + ((waveIdx - 1) * 2);
                    }
                    else if (waveIdx > 1 && waveIdx % 2 != 0)
                    {
                        dt.hitpoints = prefab.GetComponent<DamageTaker>().hitpoints + ((waveIdx - 2) * 2);
                    }
                    dt.currentHitpoints = dt.hitpoints;
                    // Set pathway
                    newEnemy.GetComponent<AiStatePatrol>().path = path;
                    NavAgent agent = newEnemy.GetComponent<NavAgent>();
                    // Set speed offset
                    if (waveIdx % 10 == 0)
                    {
                        speed += 0.05f;
                    }
                    agent.speed = Random.Range(agent.speed * (1f - speed), agent.speed * (1f + speed));
                    // Add enemy to list
                    activeEnemies.Add(newEnemy);
                    // Wait for delay before next enemy run
                    yield return new WaitForSeconds(unitSpawnDelay);
                }
                if (waveIdx + 1 == waves.Count)
                {
                    finished = true;
                }
            }
        }

    }
    internal void WaveStart(GameObject obj, string param)
    {
        int waveNumber;
        int.TryParse(param, out waveNumber);
        Pathway path1 = GetComponentInParent<Pathway>();
        if (path1.name == "Pathway" && param == "0")
        {
            StartCoroutine("RunWave", waveNumber);
        }
        if (path1.name != "Pathway" && param == "continue")
        {
            StartCoroutine("RunWave", waveNumber);
        }
    }

    void Update()
    {
        // If all spawned enemies are dead
        if ((finished == true) && (activeEnemies.Count <= 0))
        {
            EventManager.InvokeEvent("AllEnemiesAreDead", null, null);
            gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    private GameObject EnemySelection(int currentWave, int regularEnemy)
    {
        if (currentWave % 20 == 0 && currentWave > 19 && GameObject.FindWithTag("Boss") == null)
        {
            return randomEnemiesList[6];
        }
        else if (currentWave % 7 == 0 && currentWave > 13)
        {
            return randomEnemiesList[5];
        }
        else if (currentWave % 3 == 0 && currentWave > 5 && GameObject.FindWithTag("BuffHp") == null)
        {
            return randomEnemiesList[4];
        }
        else if (currentWave % 4 == 0 && currentWave > 7 && GameObject.FindWithTag("BuffSpeed") == null)
        {
            return randomEnemiesList[3];
        }
        else if (currentWave % 5 == 0 && currentWave > 14)
        {
            return randomEnemiesList[8];
        }
        else if(currentWave % 1 == 0 && currentWave > 0 && GameObject.FindGameObjectsWithTag("Sacrifice").Length < 3)
        {
            return randomEnemiesList[7];
        }
        else
        {
            return randomEnemiesList[regularEnemy];
        }
    }
}
