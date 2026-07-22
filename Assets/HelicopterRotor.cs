using UnityEngine;

public class HelicopterRotor : MonoBehaviour
{
    [Tooltip("Rotation speed in degrees per second.")]
    public float rotationSpeed = 1500f;

    [Tooltip("The local axis the rotor spins on. (0,1,0) for main rotor, (1,0,0) for tail rotor.")]
    public Vector3 rotationAxis = Vector3.up;

    // We cache the Transform reference to avoid native-to-managed bridge overhead
    private Transform _transform;

    void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        // Lowest impact rotation: simple multiplication, applied in Local Space
        _transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
    }
}