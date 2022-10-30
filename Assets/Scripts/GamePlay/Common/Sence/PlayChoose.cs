using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayChoose : MonoBehaviour
{
	// Scene to exit
	public string exitSceneName;
	// Folder for level visualisation
	public Transform levelFolder;
	// Choosen level
	public GameObject currentLevel;
	// All levels
	public List<GameObject> levelsPrefabs = new List<GameObject>();

	// Index of last allowed level for choosing
	private int maxActiveLevelIdx;
	// Index of current displayed level
	private int currentDisplayedLevelIdx;

	void OnEnable()
	{
		EventManager.StartListening("ButtonPressed", ButtonPressed);
	}

	void OnDisable()
	{
		EventManager.StopListening("ButtonPressed", ButtonPressed);
	}

	void Awake()
	{
		maxActiveLevelIdx = -1;
		Debug.Assert(currentLevel &&  levelFolder, "Wrong initial settings");
	}

	void Start()
	{
		//int hitIdx = -1;
		//int levelsCount = DataManager.instance.progress.openedLevels.Count;
		//if (levelsCount > 0)
		//{
		//	// Get name of last opened level from stored data
		//	string openedLevelName = DataManager.instance.progress.openedLevels[levelsCount - 1];

		//	int idx;
		//	for (idx = 0; idx < levelsPrefabs.Count; ++idx)
		//	{
		//		// Try to find last opened level in levels list
		//		if (levelsPrefabs[idx].name == openedLevelName)
		//		{
		//			hitIdx = idx;
		//			break;
		//		}
		//	}
		//}
		//// Level found
		//if (hitIdx >= 0)
		//{
		//	if (levelsPrefabs.Count > hitIdx + 1)
		//	{
		//		maxActiveLevelIdx = hitIdx + 1;
		//	}
		//	else
		//	{
		//		maxActiveLevelIdx = hitIdx;
		//	}
		//}
		//// level does not found
		//else
		//{
		//	if (levelsPrefabs.Count > 0)
		//	{
		//		maxActiveLevelIdx = 0;
		//	}
		//	else
		//	{
		//		Debug.LogError("Have no levels prefabs!");
		//	}
		//}
		//if (maxActiveLevelIdx >= 0)
		//{
		//	DisplayLevel(maxActiveLevelIdx);
		//}
	}
	private void DisplayLevel(int levelIdx)
	{
		Transform parentOfLevel = currentLevel.transform.parent;
		Vector3 levelPosition = currentLevel.transform.position;
		Quaternion levelRotation = currentLevel.transform.rotation;
		Destroy(currentLevel);
		currentLevel = Instantiate(levelsPrefabs[levelIdx], parentOfLevel);
		currentLevel.name = levelsPrefabs[levelIdx].name;
		currentLevel.transform.position = levelPosition;
		currentLevel.transform.rotation = levelRotation;
		currentDisplayedLevelIdx = levelIdx;
	}

	private void Exit()
	{
		SceneManager.LoadScene(exitSceneName);
	}

	/// <summary>
	/// Go to choosen level.
	/// </summary>
	private void GoToLevel()
	{
		if (currentLevel.name == "ProgressInfo")
		{
			SceneManager.LoadScene("DemoSence");
		}
		else
		{
			SceneManager.LoadScene(currentLevel.name);
		}
	}

	private void NewGame()
	{
		if (File.Exists(Application.persistentDataPath + DataManager.instance.gameProgressFile) == true)
		{
			File.Delete(Application.persistentDataPath + DataManager.instance.gameProgressFile);
			DataManager.instance.progress.currentWave = 0;
			DataManager.instance.progress.enemiesInWave = 3;
			DataManager.instance.progress.defeatAttempts = "";
			DataManager.instance.progress.gold = "";
			DataManager.instance.progress.towerInfors = new List<TowerInfor>();
		}
		if (currentLevel.name == "ProgressInfo")
		{
			SceneManager.LoadScene("DemoSence");
		}
		else
		{
			SceneManager.LoadScene(currentLevel.name);
		}
	}

	private void ButtonPressed(GameObject obj, string param)
	{
		switch (param)
		{
			case "Start":
				GoToLevel();
				break;
			case "Exit":
				Exit();
				break;
			case "NewGame":
				NewGame();
				break;
		}
	}
}
