using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShootingWeaponModel : MonoBehaviour
{
    public bool aimShooting;
    public bool physicsProjectileShooting;


    public GameObject muzzleFlash;
    public Transform projectileSpawnPoint;
    public Transform projectilePrefab;
    public float projectileSpeed;

    public Transform bulletImpactPrefab;
    
    

    public AudioSource weaponSound;

    public bool trajectoryAiming;
    public Rigidbody physicsProjectilePrefab;  //Destroy and Impact of physicsProjectile will be handled by PhysicsProjectile script attached to physicsProjectilePrefab
    public float force;
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 30;
    public float timeBetweenPoints = 0.1f;
    public LayerMask collisionMask;


    void OnEnable()
    {
        if (trajectoryAiming) 
        { 
            GameplayScreen.ins.trajectoryAimingStrength.gameObject.SetActive(true);
            force = GameplayScreen.ins.trajectoryAimingStrength.value;
            GameplayScreen.ins.trajectoryAimingStrength.onValueChanged.AddListener(TrajectoryAimingOnValueChanged);
        }   
    }

    void OnDisable()
    {
        if (trajectoryAiming)
        {
            GameplayScreen.ins.trajectoryAimingStrength.gameObject.SetActive(false);
            GameplayScreen.ins.trajectoryAimingStrength.onValueChanged.RemoveListener(TrajectoryAimingOnValueChanged);
        }
    }

    void TrajectoryAimingOnValueChanged(float val)
    {
        force = val;
    }


    void Update()
    {
        if (trajectoryAiming) 
        {
            DrawTrajectory();
        }
    }

    void DrawTrajectory()
    {
        trajectoryLine.positionCount = trajectoryPoints;

        Vector3 startPosition = projectileSpawnPoint.position;
        Vector3 startVelocity = projectileSpawnPoint.forward * force;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = i * timeBetweenPoints;

            Vector3 point =
                startPosition +
                startVelocity * time +
                0.5f * Physics.gravity * time * time;

            trajectoryLine.SetPosition(i, point);

            // Stop line when hitting something
            if (i > 0)
            {
                Vector3 previousPoint = trajectoryLine.GetPosition(i - 1);

                if (Physics.Linecast(previousPoint, point, out RaycastHit hit, collisionMask))
                {
                    trajectoryLine.SetPosition(i, hit.point);
                    trajectoryLine.positionCount = i + 1;
                    break;
                }
            }
        }
    }


    public void Shoot()
    {
        if (muzzleFlash != null)
        {
            StopCoroutine("ActivateMuzzleFlash");
            StartCoroutine("ActivateMuzzleFlash");
        }

        if (projectilePrefab != null)
        {
            //Transform projectileClone = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }


        if (weaponSound != null) { weaponSound.Play(); }
    }



    public void ShootAtTarget(RaycastHit raycastHit)
    {
        if (muzzleFlash != null) 
        {
            StopCoroutine("ActivateMuzzleFlash");
            StartCoroutine("ActivateMuzzleFlash");
        }

        Vector3 impactSpawnPos = raycastHit.point + raycastHit.normal * 0.01f;

        // Rotate decal to match surface
        Quaternion rot = Quaternion.LookRotation(raycastHit.normal);


        if (projectilePrefab != null)
        {
            //projectileSpawnPoint.LookAt(raycastHit.point);

            Vector3 destination = raycastHit.point;
            
            Transform projectileClone = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            projectileClone.LookAt(destination);

            
        }
        else { Instantiate(bulletImpactPrefab, impactSpawnPos, rot); }




        if (weaponSound != null) { weaponSound.Play(); }
    }


   

    public void ShootPhysicsProjectile()
    {
        if (muzzleFlash != null)
        {
            StopCoroutine("ActivateMuzzleFlash");
            StartCoroutine("ActivateMuzzleFlash");
        }

        Rigidbody projectile = Instantiate(
            physicsProjectilePrefab,
            projectileSpawnPoint.position,
            Quaternion.identity
        );


        projectile.gameObject.SetActive(true);
        projectile.angularVelocity = new Vector3(Random.Range(-10,10), Random.Range(-10, 10), Random.Range(-10, 10));
        projectile.linearVelocity = projectileSpawnPoint.forward * force;

        if (weaponSound != null) { weaponSound.Play(); }
    }


    IEnumerator ActivateMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.2f);
        muzzleFlash.SetActive(false);
    }
}
