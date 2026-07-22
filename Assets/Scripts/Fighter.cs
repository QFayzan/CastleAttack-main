using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public Transform fighterModels;
    


    public CharacterController characterController;
    public GameObject climbAnimation;


    public AudioSource fighterLoopAnimsAudioSource, weaponsAudioSource;
    

    public Transform lookAtHelper;


    [Space(30)]
    public bool takenCover;
    
    public FighterModel activeFighterModel;
    public bool isMoving;
    public bool isMovingWithFullIntensity;

    public WeaponBtn[] allWeaponButtonControllers;
    public WeaponAttributes[] allWeaponAttributes;
    public bool isFlying = false;


    

    void Update()
    {
        //for gravity
        if (characterController.enabled)
        {
            if(!isFlying)
            { characterController.Move(new Vector3(0, -10 * Time.deltaTime, 0)); }
        }
       
    }


    public void PlayIdleAnim()
    {
        activeFighterModel.PlayIdleAnim();
    }

    public void PlayWalkAnim()
    {
        activeFighterModel.PlayWalkAnim();
    }

    public void PlayRunAnim()
    {
        activeFighterModel.PlayRunAnim();
    }



    public void SelectFighterModel(string fighterID)
    {
        for (int j = 0; j < allWeaponButtonControllers.Length; j++)
        {
            allWeaponButtonControllers[j].fighterID = fighterID;
            allWeaponAttributes[j].fighterID = fighterID;
        }
        for (int i = 0; i < fighterModels.childCount; i++)
        {
            FighterModel fighterModel = fighterModels.GetChild(i).GetComponent<FighterModel>();
            if (fighterModel.fighterID == fighterID)
            {
                fighterModel.gameObject.SetActive(true);

                if (isMoving)
                {
                    if (isMovingWithFullIntensity) { fighterModel.PlayRunAnim(); } else { fighterModel.PlayWalkAnim(); }
                }
                else { fighterModel.PlayIdleAnim(); }

                activeFighterModel = fighterModel;

               
            }
            else { fighterModel.gameObject.SetActive(false); }
        }
    }



    public void SelectWeapon(string fighterID, string weaponID)
    {
        if (activeFighterModel != null && activeFighterModel.fighterID == fighterID)
        {
            activeFighterModel.SelectWeapon(weaponID);
            
        }
        else
        {
            SelectFighterModel(fighterID);
            SelectWeapon(fighterID, weaponID);
        }
        
    }


    public FighterModel.WeaponInfo GetActiveWeapon()
    {
        return activeFighterModel.activeWeapon;
    }

   

    public void Move(Vector3 dir, bool fullIntensity)
    {
        isMoving = true;    
        isMovingWithFullIntensity = fullIntensity;

        if (isMovingWithFullIntensity) { dir *= activeFighterModel.runSpeed; } else { dir *= activeFighterModel.walkSpeed; }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 moveDirection = (forward * -dir.x) + (right * dir.z);

        characterController.Move(moveDirection * Time.deltaTime);
        lookAtHelper.localPosition = new Vector3(dir.z, 0, -dir.x);

        if (GetActiveWeapon() == null)
        { fighterModels.transform.LookAt(lookAtHelper); }
        else
        { 
            if (!GetActiveWeapon().lookForwardLock) { fighterModels.transform.LookAt(lookAtHelper); } 
        }

    }

    public void StoppedMoving()
    {
        isMoving = false;
        isMovingWithFullIntensity = false;
        if (!activeFighterModel.isJumping) { PlayIdleAnim(); }
    }
}
