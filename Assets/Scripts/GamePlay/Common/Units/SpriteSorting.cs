using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorting : MonoBehaviour
{
    // Static will no change order on update, only on start
    public bool isStatic;
    // Multiplier for accuracy inreasing
    public float rangeFactor = 100f;

    private Dictionary<SpriteRenderer, int> sprites = new Dictionary<SpriteRenderer, int>();
    // Start is called before the first frame update

    void Awake()
    {
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprites.Add(sprite, sprite.sortingOrder);
        }
    }
    void Start()
    {
        UpdateSortingOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStatic == false)
        {
            UpdateSortingOrder();
        }
    }

    private void UpdateSortingOrder()
    {
        foreach (KeyValuePair<SpriteRenderer, int> sprite in sprites)
        {
            sprite.Key.sortingOrder = sprite.Value - (int)(transform.position.y * rangeFactor);
        }
    }
}
