using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 레이케스트 방식으로 바꾸면서 사용하지 않는 클래스
public class Bullet : MonoBehaviour
{
    [SerializeField] float power;
    [SerializeField] int dmg;

    GameObject mainCamera;
    Rigidbody rigid;

    float a;
    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
        rigid = GetComponent<Rigidbody>();
    }

    public void Fire()
    {
        rigid.velocity = Vector3.zero;
        rigid.AddForce(mainCamera.transform.forward * power, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        //.Log(rigid.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            Debug.Log("몸통에 맞음");
        }
        else if(other.gameObject.layer == 8)
        {
            Debug.Log("머리에 맞음");
        }

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    { 
        Debug.Log(collision.gameObject.name + "맞았어용!!");

        if (collision.gameObject.layer == 8)
        {
            Debug.Log("머리에 맞았음");
            collision.gameObject.GetComponentInParent<Zombie>().DecreaseHp(dmg * 2);
        }
        else if(collision.gameObject.layer == 9)
        {
            Debug.Log("몸통에 맞았음");
            collision.gameObject.GetComponent<Zombie>().DecreaseHp(dmg);
        }

        gameObject.SetActive(false);
    }
}