using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TrajectoryAiming : MonoBehaviour
{
    public Transform aimimgSpherePrefab;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("SpawnSpheres");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnSpheres()
    {
        while (true) 
        { 
            Transform aimimgSphereClone = Instantiate(aimimgSpherePrefab, transform.position, transform.rotation);


            Vector3 targetPos = aimimgSphereClone.position +
                    aimimgSphereClone.forward * 50 +
                    aimimgSphereClone.up * -10;


            aimimgSphereClone.DOJump(targetPos, 20, 1, 1).OnComplete(delegate { Destroy(aimimgSphereClone.gameObject); });


            yield return new WaitForSeconds(.05f);  
        }
    }
}
