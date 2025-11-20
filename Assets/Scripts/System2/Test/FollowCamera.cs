using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    Vector3 aimPosition;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        
    }

    private void Update()
    {
        mainCamera.transform.position = transform.position + aimPosition;
        mainCamera.transform.LookAt(transform);
    }
}
