using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed;
    float mouseY;

    void Update()
    {
        mouseY = Input.GetAxis("Mouse Y");

        transform.eulerAngles -= new Vector3(mouseY, 0, 0);
    }
}
