using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
public class LoadedHumanoidAnimatorManager : LoadedEntityAnimationManager
{
    public static readonly float GRAB_SHEATHED_WEAPON_ANI_TIME = 0.200f;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

    }

    public override void AttackLeft()
    {
        Animator.SetTrigger("Attack1");
    }



    public void DrawFromSheath(GameObject hand, GameObject sheathedWeapon)
    {
        //Start animation to grab sheathed weapon
        Animator.SetTrigger("GrabSheathedWeapon");
        StartCoroutine(WaitForGrabSheath(hand, sheathedWeapon));
    }

    private IEnumerator WaitForGrabSheath(GameObject hand, GameObject sheathedWeapon)
    {
        //Wait till grab animation is done
        yield return new WaitForSeconds(GRAB_SHEATHED_WEAPON_ANI_TIME);
        sheathedWeapon.transform.parent = hand.transform;
        sheathedWeapon.transform.localPosition = Vector3.zero;
        sheathedWeapon.transform.localRotation = Quaternion.identity;

        Animator.SetTrigger("UnsheathWeapon");
       // yield Wai
    }


    
}