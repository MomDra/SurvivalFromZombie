using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int numOfBullet;

    bool isGround;

    GunController gunController;
    Rigidbody rigid;

    private void Start()
    {
        gunController = FindObjectOfType<GunController>();
        rigid = GetComponent<Rigidbody>();

        rigid.velocity = Vector3.down * 2;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rigid.velocity = Vector3.zero;
        }
        else if (collision.gameObject.layer == 7)
        {
            // 체력 회복 코드

            gunController.IncreaseBullet(numOfBullet);

            Destroy(gameObject);
        }
    }
}
