using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationAdv : MonoBehaviour
{
    public Animation anims;

    public List<AnimationClipAdv> animationClips = new List<AnimationClipAdv>();

    [System.Serializable]
    public class AnimationClipAdv
    {
        public string clipName;
        public float speed;
        public AudioSource sound;
        public float soundDelay;
    }

    public void PlayAnim(string clipName)
    {
        StopCoroutine("PlayAnimSound");

        AnimationClipAdv animationClipAdv = GetAnimationClipAdv(clipName);

        if (animationClipAdv == null)
        {
            anims.Stop(clipName);
            anims.Play(clipName);
        }
        else 
        {
            anims[clipName].speed = animationClipAdv.speed;
            anims.CrossFade(clipName,.1f);
            if (animationClipAdv.sound != null) { StartCoroutine("PlayAnimSound", animationClipAdv); }
        }
    }


    public float GetAnimLength(string clipName)
    {
        AnimationClipAdv animationClipAdv = GetAnimationClipAdv(clipName);

        if (animationClipAdv == null)
        {
            return anims[clipName].length;
        }
        else
        {
            return anims[clipName].length / animationClipAdv.speed;
        }
    }


    IEnumerator PlayAnimSound(AnimationClipAdv animationClipAdv)
    {
        yield return new WaitForSeconds(animationClipAdv.soundDelay);
        animationClipAdv.sound.Play();

    }


    public AnimationClipAdv GetAnimationClipAdv(string clipName)
    {
        for (int i = 0; i < animationClips.Count; i++)
        {
            if (animationClips[i].clipName == clipName) 
            {
                return animationClips[i];
            }
        }
        return null;
    }
}
