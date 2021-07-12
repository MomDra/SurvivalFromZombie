using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    float applySpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float rotateSpeed;

    float horizontal;
    float vertical;
    float mouseX;
    
    bool isJumping;
    bool isRunning;
    bool isWalking;

    Rigidbody rigid;
    [SerializeField] Animator gunAnim;

    Vector3 lastPos;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        mouseX = Input.GetAxisRaw("Mouse X");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Run();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Walk();
        }

        Rotate();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
        MoveCheck();
    }

    void Move()
    {
        Vector3 moveHorizontal = transform.right * horizontal;
        Vector3 moveVertical = transform.forward * vertical;
        Vector3 dirVec = (moveHorizontal + moveVertical).normalized * applySpeed;

        rigid.MovePosition(rigid.position + dirVec * Time.deltaTime);
    }

    public void Walk()
    {
        isRunning = false;
        applySpeed = walkSpeed;
        gunAnim.SetBool("isRunning", isRunning);
    }

    void Run()
    {
        isRunning = true;
        applySpeed = runSpeed;
        gunAnim.SetBool("isRunning", isRunning);
    }

    void MoveCheck()
    {
        if (Vector3.Distance(lastPos, rigid.position) <= 0.01f)
            isWalking = false;
        else
            isWalking = true;

        gunAnim.SetBool("isWalking", isWalking);

        lastPos = rigid.position;
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
