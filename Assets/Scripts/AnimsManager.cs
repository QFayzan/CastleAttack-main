using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script picks up the selected weapon name automatically and we just have to call PlayAnim and pass the animType value like  idle, run, walk, jump
//if this script does not finds any animatin then it plays the same anmation for weaponType = NoWeapon , for eg we call PlayAnim(walk) for lets say weaponType = pistol and there is no weaponType = pistol and animType = Walk then it will play the walk of weaponType = NoWeapon
//so NoWeapon value is a special type of value


public class AnimsManager : MonoBehaviour
{
    private void Awake()
    {
        activeAnimType = "Idle";
    }

    public Transform animClipsParent;

    public GameObject[] objsToDeativate;



    //[HideInInspector]
    public string activeAnimType = "";


    public Anim PlayActiveAnim()
    {
        if (activeAnimType == "Idle") { return PlayIdle(); }
        if (activeAnimType == "Walk") { return PlayWalk(); }
        if (activeAnimType == "Run")  { return PlayRun(); }
        if (activeAnimType == "Jump") { return PlayJump(); }
        if (activeAnimType == "Attack") { return PlayAttack(); }
        return null;
    }

    public bool IsLoopAnimPlaying()
    {
        if (activeAnimType == "Idle" || activeAnimType == "Walk" || activeAnimType == "Run") { return true; }else{ return false; }
    }


    public Anim PlayIdle()
    {
        activeAnimType = "Idle";
        StopCoroutine("PlayIdleAfterDelay");
        return PlayAnim("Idle");
    }

    public Anim PlayWalk()
    {
        activeAnimType = "Walk";
        StopCoroutine("PlayIdleAfterDelay");
        return PlayAnim("Walk");
    }

    public Anim PlayRun()
    {
        activeAnimType = "Run";
        StopCoroutine("PlayIdleAfterDelay");
        return PlayAnim("Run");
    }

    public Anim PlayJump()
    {
        activeAnimType = "Jump";

        StopCoroutine("PlayIdleAfterDelay");

        return PlayAnim("Jump");
    }


    public Anim PlayAttack()
    {
        activeAnimType = "Attack";

        StopCoroutine("PlayIdleAfterDelay");

        return PlayAnim("Attack");
    }



    public void PlayJumpWhileMoving()
    {
        activeAnimType = "Jump";

        StopCoroutine("PlayIdleAfterDelay");

        PlayAnim("Jump", true);
    }

    

    public Anim PlayOther(string weaponType, string animType)
    {
        activeAnimType = "Other";
        Anim animClip = PlayAnim(weaponType, animType);

        if (animClip != null) 
        {
            StopCoroutine("PlayIdleAfterDelay");
            StartCoroutine("PlayIdleAfterDelay", animClip.GetAnimationLength());
            return animClip;
        }

        return null;
    }


    public Anim PlayOneShot(string weaponType, string animType)
    {
        activeAnimType = "OneShot";
        Anim animClip = PlayAnim(weaponType, animType);
        return animClip;
    }



    IEnumerator PlayIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayIdle();
    }

    
    public Anim GetAnim(string weaponID, string animType)
    {
        for (int i = 0; i < animClipsParent.childCount; i++)
        {
            Anim animClip = animClipsParent.GetChild(i).GetComponent<Anim>();
            if (animClip.weaponID == weaponID && animClip.animType == animType)
            {
                return animClip;
            }
        }

        return null;
    }

    public float GetAnimLength(string weaponID, string animType)
    {
        Animation anims = GetAnim(weaponID, animType).anims;
        return anims[anims.clip.name].length;
    }




    public Anim PlayAnim(string animType, bool skipSeconds = false)
    {
        return PlayAnim(WeaponManager.ins.activeWeapon.weaponID, animType, skipSeconds);
    }

    public Anim PlayAnim(string weaponID, string animType, bool skipSeconds = false)
    {
        for (int i = 0; i < objsToDeativate.Length; i++) { objsToDeativate[i].SetActive(false); }


        Anim animClip = GetAnim(weaponID, animType);
        if (animClip == null) { animClip = GetAnim("NoWeapon", animType); }

        if (animClip == null) { return null; }

        for (int i = 0; i < animClipsParent.childCount; i++) { animClipsParent.GetChild(i).gameObject.SetActive(false); }
        animClip.anims.gameObject.SetActive(true);

        animClip.anims[animClip.anims.clip.name].speed = animClip.animSpeed;
        animClip.anims.Play();

        if (animClip.animSound != null) { animClip.PlayAnimSound(); }

        for (int i = 0; i < animClip.objsToAtivate.Length; i++) { animClip.objsToAtivate[i].SetActive(true); }

        return animClip;    
    }

    

}
