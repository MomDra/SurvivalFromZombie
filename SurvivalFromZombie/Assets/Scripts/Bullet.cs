using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float power;

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


    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
        Debug.Log("Ãæµ¹ÇÔ");
    }
}

