using UnityEngine;

public class Temp : MonoBehaviour
{
    public Animation anim;

    public Transform blendAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        // Run animation on base layer
        anim["Idle"].layer = 0;

        // Shoot animation on higher layer
        anim["RifleWalk"].layer = 1;

        //blendAnim = anim.transform.Find("mixamorig:Spine");

        // Only affect upper body
        anim["RifleWalk"].AddMixingTransform(
            blendAnim,
            true
        );*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            anim.CrossFade("Idle");
            anim.Blend("RifleWalk", 1f, 0.1f);
        }


        

        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.Stop("RiflePose");

            anim["RiflePose"].layer = 1;

            anim["RiflePose"].AddMixingTransform(
            blendAnim,
            true
            );

            anim.CrossFade("RiflePose", .1f);

        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.Stop("RiflePose");

            

        }
    }
}
