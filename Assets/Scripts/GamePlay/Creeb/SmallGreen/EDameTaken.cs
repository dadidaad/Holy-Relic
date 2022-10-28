using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDameTaken : DamageTaker
{

    protected override IEnumerator DieCoroutine()
    {
        Animator anim = GetComponentInChildren<SpriteRenderer>().GetComponent<Animator>();
        //If unit has animator
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            //	 Search for clip
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals("GreenDie"))
                {
                    print("clip name is GreenDie");
                    //Play animation
                    anim.SetTrigger("isDead");
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
