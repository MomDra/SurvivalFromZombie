using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int numOfBullet;
    [SerializeField] int numOfBarrel;
    [SerializeField] int gunDmg;

    [SerializeField] float UITime;

    bool canHp;
    bool canBullet;
    bool canBarrel;
    bool canDmg;

    GunController gunController;
    ArmController armController;
    Rigidbody rigid;

    GameObject parentUI;
    GameObject HpUI;
    GameObject bulletUI;
    GameObject barrelUI;
    GameObject dmgUI;

    private void Start()
    {
        gunController = FindObjectOfType<GunController>();
        armController = FindObjectOfType<ArmController>();
        rigid = GetComponent<Rigidbody>();

        parentUI = GameObject.Find("GetRandomBox");

        HpUI = parentUI.transform.Find("GetHp").gameObject;
        bulletUI = parentUI.transform.Find("GetBullet").gameObject;
        barrelUI = parentUI.transform.Find("GetBarrel").gameObject;
        dmgUI = parentUI.transform.Find("IncreaseDmg").gameObject;

        int random = Random.Range(0, 10);
        if (random > 3) canHp = true; // 60%

        random = Random.Range(0, 10);
        if (random > 1) canBullet = true; // 80%

        random = Random.Range(0, 10);
        if (random > 5) canBarrel = true; // 40%

        random = Random.Range(0, 15);
        if (random == 0) canDmg = true; // 6.66%

    }

    private void FixedUpdate()
    {
        rigid.velocity = Vector3.down * 2;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if (canHp)
            {
                canHp = false;

                GameManager.instance.plyerHP = hp;

                HpUI.SetActive(true);
                StartCoroutine(HideUI(HpUI));
            }

            if (canBullet)
            {
                canBullet = false;

                gunController.IncreaseBullet(numOfBullet);

                bulletUI.SetActive(true);
                StartCoroutine(HideUI(bulletUI));
            }

            if (canBarrel)
            {
                canBarrel = false;

                armController.IncreaseBarrel(numOfBarrel);

                barrelUI.SetActive(true);
                StartCoroutine(HideUI(barrelUI));
            }

            if (canDmg)
            {
                canDmg = false;

                gunController.IncreaseDmg(gunDmg);

                dmgUI.SetActive(true);
                StartCoroutine(HideUI(dmgUI));
            }

            Destroy(gameObject, UITime + 0.2f);
        }
    }

    IEnumerator HideUI(GameObject UI)
    {
        yield return new WaitForSeconds(UITime);

        UI.SetActive(false);
    }
}
