using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;
    private Vector3 moveDir;

    private Rigidbody rigid;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 80.0f;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        // 전후좌우 이동 방향 벡터 계산
        moveDir = (Vector3.forward * v) + (Vector3.right * h);
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(Vector3.up * r * rotSpeed * Time.fixedDeltaTime));
    }
}
