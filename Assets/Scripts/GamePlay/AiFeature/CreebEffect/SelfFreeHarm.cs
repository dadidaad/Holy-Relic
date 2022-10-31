using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfFreeHarm : MonoBehaviour
{
    public GameObject selfFreeHarm;
    
    [SerializeField]
    int timeSelfFreeHarm = 3;

    private int count = 0;
    
    private void Update()
    {
        DamageTaker targetTaker = selfFreeHarm.GetComponent<DamageTaker>();
        if (selfFreeHarm != null && targetTaker.currentHitpoints < targetTaker.hitpoints && count < timeSelfFreeHarm)
        {
            targetTaker.currentHitpoints = targetTaker.hitpoints;
            targetTaker.healthBar.localScale = new Vector2(targetTaker.originHealthBarWidth, targetTaker.healthBar.localScale.y);
            print("hitpoints: " + targetTaker.hitpoints);
            print("current: " + targetTaker.currentHitpoints);
            print("count " + count);
            count++;
        }
        
    }
}
