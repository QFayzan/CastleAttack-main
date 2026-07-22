using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    [Header("References")]
    public Transform throwPoint;
    public Rigidbody grenadePrefab;
    public LineRenderer lineRenderer;

    [Header("Throw Settings")]
    public float throwForce = 15f;

    [Header("Trajectory")]
    public int points = 30;
    public float timeBetweenPoints = 0.1f;
    public LayerMask collisionMask;

    void Update()
    {
        DrawTrajectory();

        if (Input.GetMouseButtonDown(0))
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        Rigidbody grenade = Instantiate(
            grenadePrefab,
            throwPoint.position,
            Quaternion.identity
        );

        grenade.gameObject.SetActive( true );  
        grenade.linearVelocity = throwPoint.forward * throwForce;
    }

    void DrawTrajectory()
    {
        lineRenderer.positionCount = points;

        Vector3 startPosition = throwPoint.position;
        Vector3 startVelocity = throwPoint.forward * throwForce;

        for (int i = 0; i < points; i++)
        {
            float time = i * timeBetweenPoints;

            Vector3 point =
                startPosition +
                startVelocity * time +
                0.5f * Physics.gravity * time * time;

            lineRenderer.SetPosition(i, point);

            // Stop line when hitting something
            if (i > 0)
            {
                Vector3 previousPoint = lineRenderer.GetPosition(i - 1);

                if (Physics.Linecast(previousPoint, point, out RaycastHit hit, collisionMask))
                {
                    lineRenderer.SetPosition(i, hit.point);
                    lineRenderer.positionCount = i + 1;
                    break;
                }
            }
        }
    }
}