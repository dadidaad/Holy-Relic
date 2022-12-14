using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelManagerInspector : MonoBehaviour
{
    [HideInInspector]
    // List with all enemies prefabs
    public List<GameObject> enemiesList = new List<GameObject>();

    [HideInInspector]
    // Enemies list for this level
    public List<GameObject> enemies
    {
        get
        {
            return levelManager.allowedEnemies;
        }
        set
        {
            levelManager.allowedEnemies = value;
        }
    }

    [HideInInspector]
    // List with all towers prefabs
    public List<GameObject> towersList = new List<GameObject>();

    [HideInInspector]
    // Towers list for this level
    public List<GameObject> towers
    {
        get
        {
            return levelManager.allowedTowers;
        }
        set
        {
            levelManager.allowedTowers = value;
        }
    }

    [HideInInspector]
    // List with all spells prefabs
    public List<GameObject> spellsList = new List<GameObject>();

    [HideInInspector]
    // Spells list for this level
    public List<GameObject> spells
    {
        get
        {
            return levelManager.allowedSpells;
        }
        set
        {
            levelManager.allowedSpells = value;
        }
    }

    [HideInInspector]
    // Defeat attempts before loose for this level
    public int defeatAttempts
    {
        get
        {
            return levelManager.defeatAttempts;
        }
        set
        {
            levelManager.defeatAttempts = value;
        }
    }

    private LevelManager levelManager;
    void OnEnable()
    {
        levelManager = GetComponent<LevelManager>();
        Debug.Assert(levelManager, "Wrong stuff settings");
    }
}

