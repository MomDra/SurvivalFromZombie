using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float runSpeed;
    [SerializeField]
    float jumpPower;
    [SerializeField]
    float rotateSpeed;

    float horizontal;
    float vertical;
    float mouseX;
    bool isJumping;

    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");

        Move();
        Rotate();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            Jump();
        }
    }

    void Move()
    {
        transform.Translate(new Vector3(horizontal, 0, vertical).normalized * moveSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up, mouseX * rotateSpeed);
    }

    void Jump()
    {
        isJumping = true;
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
