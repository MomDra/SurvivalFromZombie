using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed;
    float mouseY;
    float currentCameraRotationX;

    void Update()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * rotateSpeed;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -80f, 80f);

        transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
    }
}
