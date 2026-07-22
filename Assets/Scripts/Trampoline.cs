using System.Collections;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public AudioSource bounceSound;
    
    private void Start()
    {
        StartCoroutine(DetectPlayer());
    }
    

    /*
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.name.IndexOf("Player") > -1)
        {
            
        }
    }*/

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            //Debug.Log(Vector3.Distance(TPSController.ins.transform.position, transform.position));
            if (Vector3.Distance(TPSController.ins.fighter.transform.position, transform.position) < 2f)
            { 
                TPSController.ins.fighter.activeFighterModel.StartCoroutine(TPSController.ins.fighter.activeFighterModel.JumpCoroutine(30, .75f));
                bounceSound.Play();
                break;
            }

            yield return null;  
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(DetectPlayer()); 
    }
}
