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
        anim[mixingAnim].RemoveMixingTransform(mixingTransform);
        anim[mixingAnim].AddMixingTransform(
        mixingTransform,
        true
        );

        anim.Play(baseAnim);
        anim.Play(mixingAnim);
        print(transform.gameObject.name);
    }

    void OnDisable()
{
    if (anim == null)
        anim = GetComponent<Animation>();

    anim.Stop(mixingAnim);
    anim[mixingAnim].RemoveMixingTransform(mixingTransform);
}

    
}
