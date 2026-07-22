using System.Collections;
using UnityEngine;

public class Anim : MonoBehaviour
{
    public string weaponID;
    public string animType;

    public Animation anims;
    public ShootingWeaponModel shootingWeapon;


    public GameObject[] objsToAtivate;

    public float animSpeed = 1;

    public AudioSource animSound;
    public float animSoundDelay;

    public float skipSeconds;
    public float jumpWait;

    public float GetAnimationLength() { return anims[anims.clip.name].length/animSpeed; }

    public void PlayAnimSound()
    {

        StartCoroutine(PlayAnimSoundCoroutine());
    }


    IEnumerator PlayAnimSoundCoroutine()
    {

        yield return new WaitForSeconds(animSoundDelay);

        animSound.Play();
    }
}
