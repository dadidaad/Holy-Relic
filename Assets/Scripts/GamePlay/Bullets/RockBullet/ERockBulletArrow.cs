using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.GraphicsBuffer;

public class ERockBulletArrow : BulletArrow
{
    protected override IEnumerator DieCoroutine()
    {
        Animator anim = GetComponent<Animator>();
        //If unit has animator
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            //	 Search for clip
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals("RockCrash"))
                {
                    print("clip name is RockCrash");
                    //Play animation
                    anim.SetTrigger("isCrash");
                    print("Clip length: " + clip.length);
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
        }
        yield return true;
        Destroy(gameObject);
    }

}
