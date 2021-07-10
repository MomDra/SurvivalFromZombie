using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float rotateSpeed;

    float horizontal;
    float vertical;
    float mouseX;
    
    bool isJumping;
    bool isRunning;

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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }
        else
        {
            Move();
        }
        
        Rotate();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            Jump();
            
        }
    }

    void Move()
    {
        if (isRunning) isRunning = false;
        transform.Translate(new Vector3(horizontal, 0, vertical).normalized * walkSpeed * Time.deltaTime);
    }

    void Run()
    {
        if (!isRunning) isRunning = true;
        transform.Translate(new Vector3(horizontal, 0, vertical).normalized * runSpeed * Time.deltaTime);
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
