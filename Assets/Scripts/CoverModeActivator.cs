using System.Collections;
using UnityEngine;

public class CoverModeActivator : MonoBehaviour
{
    public float range = 10;
    
    void OnEnable()
    {
        StartCoroutine("CheckIfCharacterIsInsideRange");
    }
    

    IEnumerator CheckIfCharacterIsInsideRange()
    {
        yield return new WaitForSeconds(Random.Range(0,1.0f));
        while (true)
        {
            if (Vector3.Distance(transform.position, TPSController.ins.fighter.transform.position) < range)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(.5f);
        }
    }
}
