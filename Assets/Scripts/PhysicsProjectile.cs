using System.Collections;
using UnityEngine;

public class PhysicsProjectile : MonoBehaviour
{
    public float destroyAfter = 5;
    public Transform explosion;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(destroyAfter);

        Instantiate(explosion, transform.position, Quaternion.identity);

        DestroyImmediate(gameObject);
    }

    
}
