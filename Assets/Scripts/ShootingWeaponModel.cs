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
    public Transform trajectoryBarrel;

    [Header("Weapon Wheels Stuff")]
    public Transform leftWheel;
    public Transform rightWheel;

    [Tooltip("Base visual rotation speed when moving forward/backward.")]
    public float baseRotationSpeed = 300f;

    [Tooltip("How much faster the wheel spins when turning in its direction.")]
    public float turnSpeedBoost = 200f;

    // Define the local axis your wheels spin on (usually right or forward depending on the 3D model)
    private Vector3 rotationAxis = Vector3.right;


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

        // --- BARREL X-AXIS ROTATION LOGIC ---

        // 1. Convert your 5 to 20 force range into a 0.0 to 1.0 percentage
        float forcePercentage = Mathf.InverseLerp(5f, 20f, force);

        // 2. Map that percentage to your angle. 
        // force = 5 results in -15, force = 20 results in +15.
        float xAngle = Mathf.Lerp(-15f, 15f, forcePercentage);

        // (Remember: if it tilts the wrong way in Unity, swap to Mathf.Lerp(15f, -15f, forcePercentage))

        // 3. Apply the angle to the barrel in Local Space, leaving Y and Z alone.
        Vector3 currentLocalEuler = trajectoryBarrel.localEulerAngles;
        trajectoryBarrel.localEulerAngles = new Vector3(-xAngle, currentLocalEuler.y, currentLocalEuler.z);
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

        Vector3 destination = raycastHit.point;

        // Rotate decal to match surface
        Quaternion rot = Quaternion.LookRotation(raycastHit.normal);


        if (projectilePrefab != null)
        {
            //projectileSpawnPoint.LookAt(raycastHit.point);
           
            if(raycastHit.point!=null)
            {
                 destination = raycastHit.point;
            }
            else
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                destination = ray.GetPoint(2000);

            }

           
            
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

    //Rotate Wheel Cosmetic
    public void RotateWheelsCosmetic(Vector3 dir)
    {
        // 1. Extract inputs based on your formula
        float forwardInput = -dir.x; // W/S or Forward/Back
        float turnInput = dir.z;     // A/D or Left/Right

        // 2. Start with the base speed for both wheels
        float leftSpeed = forwardInput * baseRotationSpeed;
        float rightSpeed = forwardInput * baseRotationSpeed;

        // 3. Apply the "turn boost" based on left/right input
        if (turnInput < 0) // Turning Left
        {
            // Make the left wheel spin much faster (Mathf.Abs ensures we ADD speed regardless of forward/back direction)
            leftSpeed += Mathf.Abs(turnInput) * turnSpeedBoost * Mathf.Sign(forwardInput != 0 ? forwardInput : 1);
        }
        else if (turnInput > 0) // Turning Right
        {
            // Make the right wheel spin much faster
            rightSpeed += Mathf.Abs(turnInput) * turnSpeedBoost * Mathf.Sign(forwardInput != 0 ? forwardInput : 1);
        }

        // 4. Apply the rotations locally on their pivots
        if (leftWheel != null)
            leftWheel.Rotate(rotationAxis, leftSpeed * Time.deltaTime, Space.Self);

        if (rightWheel != null)
            rightWheel.Rotate(rotationAxis, rightSpeed * Time.deltaTime, Space.Self);
    }
}
