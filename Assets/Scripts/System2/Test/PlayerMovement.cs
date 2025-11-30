using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float v = 0.0f;
    private Rigidbody rb;
    private Animator anim;

    public Camera mapCamera;
    public LayerMask groundMask;
    public float moveSpeed = 4.0f;
    public float rotSpeed = 80.0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        v = Input.GetAxis("Vertical");
        anim.SetFloat("IsWalking", v);
    }

    private void FixedUpdate()
    {
        RotateToMouse();

        Vector3 moveDir = transform.forward * v;
        rb.MovePosition(rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void RotateToMouse()
    {
        Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50f, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0;

            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
