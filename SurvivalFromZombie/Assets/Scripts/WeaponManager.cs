using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isGun;
    public static bool isHand;

    [SerializeField] Camera weaponCamera;

    [SerializeField] GameObject gun;
    [SerializeField] GameObject arm;

    Animator animGun;
    Animator animArm;

    GunController gunController;

    private void Start()
    {
        isGun = true;

        animGun = gun.GetComponent<Animator>();
        animArm = arm.GetComponent<Animator>();

        gunController = gun.GetComponent<GunController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isGun && isHand)
        {
            StartCoroutine(GunIn());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isHand && isGun && gunController.CanChange())
        {
            StartCoroutine(ArmIn());
        }
    }

    IEnumerator GunIn()
    {
        animArm.SetTrigger("Out");
        isHand = false;

        yield return new WaitForSeconds(0.3f);

        weaponCamera.cullingMask = 1 << LayerMask.NameToLayer("Gun");
        
        animGun.SetTrigger("In");

        yield return new WaitForSeconds(0.3f);

        isGun = true;
    }
    
    IEnumerator ArmIn()
    {
        animGun.SetTrigger("Out");
        isGun = false;

        yield return new WaitForSeconds(0.3f);

        weaponCamera.cullingMask = 1 << LayerMask.NameToLayer("Arm");

        animArm.SetTrigger("In");

        yield return new WaitForSeconds(0.3f);

        isHand = true;
    }
}
