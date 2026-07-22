using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;

    public Transform explosionPrefab;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.forward * 17; 
    }


    void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
