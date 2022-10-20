using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnPointInspector : MonoBehaviour
{
	[HideInInspector]
	// Số lượng Enemies mỗi wave
	public List<int> enemiesNumber = new List<int>();

	// Spawn point component
	private SpawnPoint spawnPoint;

	// Update is called once per frame
	void OnEnable()
	{
		spawnPoint = GetComponent<SpawnPoint>();
		Debug.Assert(spawnPoint, "Settings Fail");
		enemiesNumber.Clear();
		foreach (SpawnPoint.Wave wave in spawnPoint.waves)
		{
			enemiesNumber.Add(wave.enemies.Count);
		}
	}

	public void UpdateWaveList()
	{
		// Update waves
		while (spawnPoint.waves.Count > enemiesNumber.Count)
		{
			spawnPoint.waves.RemoveAt(spawnPoint.waves.Count - 1);
		}
		while (spawnPoint.waves.Count < enemiesNumber.Count)
		{
			spawnPoint.waves.Add(new SpawnPoint.Wave());
		}
		// Update enemies count
		for (int i = 0; i < enemiesNumber.Count; i++)
		{
			while (spawnPoint.waves[i].enemies.Count > enemiesNumber[i])
			{
				spawnPoint.waves[i].enemies.RemoveAt(spawnPoint.waves[i].enemies.Count - 1);
			}
			while (spawnPoint.waves[i].enemies.Count < enemiesNumber[i])
			{
				spawnPoint.waves[i].enemies.Add(null);
			}
		}
	}

	public void AddWave()
	{
		enemiesNumber.Add(1);
	}

	/// <summary>
	/// Removes the wave.
	/// </summary>
	public void RemoveWave()
	{
		if (enemiesNumber.Count > 0)
		{
			enemiesNumber.RemoveAt(enemiesNumber.Count - 1);
		}
	}
}
