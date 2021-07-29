using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isGun;
    public static bool isArm;

    [SerializeField] Camera weaponCamera;

    [SerializeField] GameObject gun;
    [SerializeField] GameObject arm;

    [SerializeField] GameObject bulletUI;
    [SerializeField] GameObject barrelUI;

    Animator animGun;
    Animator animArm;

    GunController gunController;
    ArmController armController;

    private void Start()
    {
        isGun = true;

        animGun = gun.GetComponent<Animator>();
        animArm = arm.GetComponent<Animator>();

        gunController = gun.GetComponent<GunController>();
        armController = arm.GetComponent<ArmController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isGun && isArm && armController.CanChange())
        {
            StartCoroutine(GunIn());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isArm && isGun && gunController.CanChange())
        {
            StartCoroutine(ArmIn());
        }
    }

    IEnumerator GunIn()
    {
        animArm.SetTrigger("Out");
        isArm = false;

        yield return new WaitForSeconds(0.3f);

        weaponCamera.cullingMask = 1 << LayerMask.NameToLayer("Gun");

        bulletUI.SetActive(true);
        barrelUI.SetActive(false);
        
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

        bulletUI.SetActive(false);
        barrelUI.SetActive(true);

        animArm.SetTrigger("In");

        yield return new WaitForSeconds(0.3f);

        isArm = true;
    }
}
