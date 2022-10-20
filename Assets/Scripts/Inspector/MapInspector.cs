using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapInspector : MonoBehaviour
{
    // Thuộ tính này Chứa SpriteRender cho Map Game Object
    public SpriteRenderer map;

    //Thuộc tính này chỉ vị vị trí của Parent cho Spawn Icon
    public Transform startIconParent;
    //Thuộc tính này chỉ vị vị trí của Parent cho Capture Icon
    public Transform endtIconParent;
    void OnEnable()
    {
        Debug.Assert(startIconParent && endtIconParent && map, "Settings Fail");
    }

    public GameObject AddStartIcon(GameObject spawnIconPrefab)
    {
        GameObject newIcon = Instantiate(spawnIconPrefab, startIconParent);
        newIcon.name = spawnIconPrefab.name;
        return newIcon;
    }

	public GameObject AddEndIcon(GameObject captureIconPrefab)
    {
        GameObject newIcon = Instantiate(captureIconPrefab, endtIconParent);
        newIcon.name = captureIconPrefab.name;
        return newIcon;
    }

    public void LoadMap(GameObject mapObject)
    {
        if (mapObject != null)
        {
            if (map != null)
            {
                DestroyImmediate(map.gameObject);
            }
            GameObject newMap = Instantiate(mapObject, transform);
            newMap.name = mapObject.name;
            map = newMap.GetComponent<SpriteRenderer>();
        }
    }
    public void MapChange(Sprite sprite)
    {
        if (map != null && sprite != null)
        {
            map.sprite = sprite;
        }
    }
}
