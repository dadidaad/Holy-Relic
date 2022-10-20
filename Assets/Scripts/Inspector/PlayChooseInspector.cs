using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayChooseInspector : MonoBehaviour
{
	// Level description
	public Transform levelFolder;

	// Level chooser component
	private PlayChoose levelChooser;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		levelChooser = GetComponent<PlayChoose>();
		Debug.Assert(levelFolder && levelChooser, "Wrong initial settings");
		// Delete all missing prefabs from list
		levelChooser.levelsPrefabs.RemoveAll(GameObject => GameObject == null);
	}

	public void AddLevel(GameObject levelPrefab)
	{
		if (levelPrefab != null)
		{
			// Add to allowed levels list
			if (levelChooser.levelsPrefabs.Contains(levelPrefab) == false)
			{
				levelChooser.levelsPrefabs.Add(levelPrefab);
			}
		}
	}

	/// <summary>
	/// Sets the active level.
	/// </summary>
	/// <param name="levelPrefab">Level prefab.</param>
	public void SetActiveLevel(GameObject level)
	{
		ProgressDescriptionInspector oldLevel = levelFolder.GetComponentInChildren<ProgressDescriptionInspector>();
		// Destroy old level description
		if (oldLevel != null)
		{
			DestroyImmediate(oldLevel.gameObject);
		}
		// Update level description
		level.transform.SetParent(levelFolder, false);
		level.transform.SetAsFirstSibling();
		levelChooser.currentLevel = level;
	}

	public List<GameObject> GetLevelPrefabs()
	{
		return levelChooser.levelsPrefabs;
	}
}
