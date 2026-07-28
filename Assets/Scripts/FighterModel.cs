using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FighterModel : MonoBehaviour
{
    public Fighter fighter;

    public string fighterID;
    
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    public AnimationAdv fighterAnims;


    public AudioClip walkSound, runSound;

    public Transform mixingTransformForAnimBlending;
    public List<WeaponInfo> weaponsInfo = new List<WeaponInfo>();








    [Space(30)]
    public WeaponInfo activeWeapon;
    public bool isWalking;
    public bool isRunning;
    public bool isJumping;

    [Header("Secondary Weapon Try")]
    public bool isTwoWeapon = false;
    public WeaponInfo secondaryWeapon;


    [System.Serializable]
    public class WeaponInfo
    {
        public string weaponID;

        public bool takeCoverModeEligible;
        public bool lookForwardLock;
        public bool lookForwardOnAttack;


        public string weaponHoldingAnimName;
        public string weaponAttackAnimName;
        public GameObject weapon;
        public ShootingWeaponModel shootingWeapon;

        public GameObject onlyActivateOnAttack;
        public AudioClip weaponAttackSound;
        public float weaponAttackSoundDelay;

        public float attackInterval = .2f;

        public bool hasWheels = false;
       
    }


    

    void Start()
    {
        
        

            fighterAnims.anims["Idle"].layer = 0;
            fighterAnims.anims["Walk"].layer = 0;
            fighterAnims.anims["Run"].layer = 0;
            fighterAnims.anims["Jump"].layer = 0;

        for (int i = 0; i < weaponsInfo.Count; i++)
        {
            if (!string.IsNullOrEmpty(weaponsInfo[i].weaponHoldingAnimName))
            {
                fighterAnims.anims[weaponsInfo[i].weaponHoldingAnimName].AddMixingTransform(mixingTransformForAnimBlending, true);
                fighterAnims.anims[weaponsInfo[i].weaponHoldingAnimName].layer = 1;
            }

            if (!string.IsNullOrEmpty(weaponsInfo[i].weaponAttackAnimName))
            {
                fighterAnims.anims[weaponsInfo[i].weaponAttackAnimName].AddMixingTransform(mixingTransformForAnimBlending, true);
                fighterAnims.anims[weaponsInfo[i].weaponAttackAnimName].layer = 2;
            }

        }

        GameplayScreen.ins.attack2Btn.SetActive(isTwoWeapon);
        
        

    }




    public void StopLayerAnimations(int layer)
    {
        foreach (AnimationState state in fighterAnims.anims)
        {
            // if (state.layer == layer) { fighterAnims.anims.Stop(state.name); }

            if (state.layer == layer)
            {
                state.weight = 0;
                // Fades the weapon animation out smoothly over 0.25 seconds
                fighterAnims.GetComponent<Animation>().Blend(state.name, 0f, 0.25f);
            }
        }
    }



    public void SelectWeapon(string id)
    {
        if (activeWeapon != null && activeWeapon.weapon != null) { activeWeapon.weapon.SetActive(false); }
        for (int i = 0; i < weaponsInfo.Count; i++)
        {
            if (weaponsInfo[i].weaponID == id)
            {
                activeWeapon = weaponsInfo[i];
                if (!fighter.takenCover) { BlendWeaponHoldAnim(); }
                if (weaponsInfo[i].weapon != null) { weaponsInfo[i].weapon.SetActive(true); }


            }
        }
    }

    //public void DeselectWeapon()
    //{
    //    //OG Code
    //    //StopLayerAnimations(1);
    //    //if (activeWeapon != null && activeWeapon.weapon != null) { activeWeapon.weapon.SetActive(false); PlayIdleAnim(); }


    //    //AI ANswer
    //    // Instead of instantly stopping the layer, fade it out over 0.3 seconds.
    //    // Assuming your legacy animation component is accessible:
    //    // animation.Blend("YourWeaponHoldAnim", 0f, 0.3f); 

    //    // If you don't have direct access to blend the layer out, 
    //    // at least CrossFade the idle animation instead of using .Play()

    //    if (activeWeapon != null && activeWeapon.weapon != null)
    //    {
    //        activeWeapon.weapon.SetActive(false);

    //        // Replace PlayIdleAnim() with a CrossFade equivalent
    //        // Example: legacyAnimation.CrossFade("Idle", 0.25f);
    //    }


    //}


    public void BlendWeaponHoldAnim()
    {
        StopLayerAnimations(1);
        if (!string.IsNullOrEmpty(activeWeapon.weaponHoldingAnimName)) { fighterAnims.PlayAnim(activeWeapon.weaponHoldingAnimName); }
    }

    public void BlendWeaponAttackAnim()
    {
        if (!fighter.takenCover) {
            StopLayerAnimations(2);
            if (!string.IsNullOrEmpty(activeWeapon.weaponAttackAnimName)) { fighterAnims.PlayAnim(activeWeapon.weaponAttackAnimName); }
        }

        fighter.weaponsAudioSource.clip = activeWeapon.weaponAttackSound;
        fighter.weaponsAudioSource.Play();
        if (activeWeapon.onlyActivateOnAttack != null) { activeWeapon.onlyActivateOnAttack.SetActive(true); }
        if (activeWeapon.lookForwardOnAttack) { fighter.fighterModels.localEulerAngles = new Vector3(0,5,0); }
    }

    public void AttackStopped()
    {
        if (activeWeapon.onlyActivateOnAttack != null) { activeWeapon.onlyActivateOnAttack.SetActive(false); }

        
    }



    public void PlayIdleAnim()
    {
        fighter.fighterLoopAnimsAudioSource.Stop();

        //fighterAnims.PlayAnim("Idle");

        fighterAnims.anims.CrossFade("Idle",0.05f);
    }



    public void PlayWalkAnim()
    {
        if (isJumping) { return; }

        fighter.fighterLoopAnimsAudioSource.clip = walkSound;
        fighter.fighterLoopAnimsAudioSource.Play();

        fighterAnims.PlayAnim("Walk");
    }

    public void PlayRunAnim()
    {
        if (isJumping) { return; }

        fighter.fighterLoopAnimsAudioSource.clip = walkSound;
        fighter.fighterLoopAnimsAudioSource.Play();

        fighterAnims.PlayAnim("Run");
    }


    public void Jump()
    {
        if (isJumping) { return; }
        StartCoroutine(JumpCoroutine(20, .25f));
    }
    public void PlayClimbAnim()
    {
        fighterAnims.PlayAnim("Climb");
    }
    public void HeliUpDown(float Power, float Time)
    {
         StartCoroutine(JumpCoroutine(Power, Time));
    }


    public IEnumerator JumpCoroutine(float jumpPower, float jumpTime)
    {
        isJumping = true;
        fighterAnims.PlayAnim("Jump");

        float t = jumpTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            if (fighter.characterController.enabled) { fighter.characterController.Move(new Vector3(0, jumpPower * Time.deltaTime, 0)); }
            yield return null;
        }

        while (!fighter.characterController.isGrounded) { yield return null; }

        isJumping = false;
        PlayIdleWalkOrRunIfNotJumping();
    }



    //this function plays between idle, walk and run automatically
    public void PlayIdleWalkOrRunIfNotJumping()
    {
        if (fighter.isMoving)
        {
            if (fighter.isMovingWithFullIntensity) { PlayRunAnim(); } else { PlayWalkAnim(); }
        }
        else
        {
            if (!isJumping) { PlayIdleAnim(); }
        }
    }



    

    public void StoppedMoving()
    {
        fighter.isMoving = false;
        fighter.isMovingWithFullIntensity = false;
    }


    //fixes for animation

  

    public void DeselectWeapon()
    {
        // We MUST tell both the holding layer (1) and attack layer (2) to let go of the arms
        StopLayerAnimations(1);
        StopLayerAnimations(2);

        if (activeWeapon != null && activeWeapon.weapon != null)
        {
            activeWeapon.weapon.SetActive(false);
        }
    }
}
