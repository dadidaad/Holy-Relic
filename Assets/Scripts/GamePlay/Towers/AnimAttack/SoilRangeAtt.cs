using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilRangeAtt : AttackRanged
{
    public Transform body;
    private Animator anim;

    void Awake()
    {
        anim = body.GetComponent<Animator>();
        print("anim"+anim.name);
        cooldownCounter = cooldown;
        Debug.Assert(arrowPrefab && firePoint, "Wrong initial parameters");
    }
    protected override IEnumerator FireCoroutine(Transform target, GameObject bulletPrefab)
    {
        if (target != null && bulletPrefab != null)
        {

            // Delay to synchronize with animation
            yield return new WaitForSeconds(fireDelay);
            if (target != null)
            {
                //tower animation
                TowerAnimation();
                // Create arrow
                GameObject arrow = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                IBullet bullet = arrow.GetComponent<IBullet>();
                bullet.SetDamage(damage);
                bullet.Fire(target);
                // Play sound effect
                //if (sfx != null && AudioManager.instance != null)
                //{
                //	AudioManager.instance.PlayAttack(sfx);
                //}
            }
        }
    }

    void TowerAnimation()
    {
        //if unit has animator
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            // search for animationClips
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "RockTowerFire")
                {
                    print("clip name is RockTowerFire");
                    // play animation
                    anim.SetTrigger("ToFire");
                    cooldown = clip.averageDuration;
                    break;
                }
            }
        }
    }
}
