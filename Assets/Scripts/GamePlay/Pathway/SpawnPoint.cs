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
        if (waves.Count > waveIdx)
        {
            //yield return new WaitForSeconds(waves[waveIdx].delayBeforeWave);

            foreach (GameObject enemy in waves[waveIdx].enemies)
            {
                GameObject prefab = null;
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
                // Set pathway
                newEnemy.GetComponent<AiStatePatrol>().path = path;
                NavAgent agent = newEnemy.GetComponent<NavAgent>();
                // Set speed offset
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
	private void WaveStart(GameObject obj, string param)
	{
		int waveNumber;
		int.TryParse(param, out waveNumber);
		StartCoroutine("RunWave", waveNumber);
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
}
