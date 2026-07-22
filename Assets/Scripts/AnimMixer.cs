using UnityEngine;

public class AnimMixer : MonoBehaviour
{
    public string baseAnim;
    public string mixingAnim;
    public string attackAnim;

    public Transform mixingTransform;

    private Animation anim;

    void OnEnable()
    {
        if (anim == null) { anim = GetComponent<Animation>(); }

        anim[baseAnim].layer = 0;

        anim[mixingAnim].layer = 1;

        anim[mixingAnim].AddMixingTransform(
        mixingTransform,
        true
        );

        anim.Play(baseAnim);
        anim.Play(mixingAnim);
    }

    
}
