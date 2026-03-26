using Unity.Cinemachine;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 80f;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Input")]
    [SerializeField] private InputReader inputReader;

    private Vector2 currentRotation;
    private bool initialized;

    public Vector3 forward => Quaternion.Euler(currentRotation.x,currentRotation.y,0) * Vector3.forward;

    private void Awake()
    {
        cinemachineCamera.Priority.Value = -1;
        
    }

    public void Init()
    {
        initialized = true;
        cinemachineCamera.Priority.Value = 10;
    }

    private void LateUpdate()
    {
        if (!initialized) { return; }
        float mouseX = inputReader.LookInput.x * lookSensitivity;
        float mouseY = inputReader.LookInput.y * lookSensitivity;
    
        currentRotation.x = Mathf.Clamp(currentRotation.x - mouseY , -maxLookAngle, maxLookAngle);
        currentRotation.y += mouseX;

        transform.localRotation = Quaternion.Euler(currentRotation.x, 0, 0);

    }
}
