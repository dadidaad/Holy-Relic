using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revise : MonoBehaviour
{
    public GameObject phantom;
    bool isRevised = false;
    private void Update()
    {
        DamageTaker targetTaker = phantom.GetComponent<DamageTaker>();
        if (phantom != null && targetTaker.currentHitpoints <= targetTaker.halfPoint && !isRevised)
        {
            isRevised = true;
            targetTaker.currentHitpoints = targetTaker.hitpoints;           
        }
    }
}
