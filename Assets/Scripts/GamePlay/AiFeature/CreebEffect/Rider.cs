using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rider : MonoBehaviour
{
    public GameObject valsilly;
    bool boosted = false;
    private void Update()
    {
        DamageTaker targetTaker = valsilly.GetComponent<DamageTaker>();
        NavAgent targetNav = valsilly.GetComponent<NavAgent>();
        if (valsilly != null && targetTaker.currentHitpoints <= targetTaker.halfPoint && !boosted)
        {
            boosted = true;
            targetNav.speed *= 2;
        }
    }
}
