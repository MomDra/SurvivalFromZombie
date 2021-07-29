using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [SerializeField] int currentBulletCount = 5;
    [SerializeField] int bulletInBagCount = 300;
    int maxBulletCount = 30;

    [SerializeField] float fireTime;
    [SerializeField] float reloadTime;

    [SerializeField] float fireDistance;
    [SerializeField] int dmg;

    float forwardBackRecoil = -0.07f;
    [SerializeField] float upRecoil;
    [SerializeField] float leftRightRecoil;

    bool isReady;
    bool isloading;

    AudioSource audioSource;
    Animator anim;
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] PlayerController playerController;

    [SerializeField] Text currentBullet;
    [SerializeField] Text maxBullet;

    [SerializeField] Transform mainCamera;
    [SerializeField] Transform changableHolder;
    RaycastHit hit;

    [SerializeField] CrossHair crossHair;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        isReady = true;
        isloading = false;

        currentBullet.text = currentBulletCount.ToString();
        maxBullet.text = bulletInBagCount.ToString();
    }

    private void Update()
    {
        if (WeaponManager.isGun)
        {
            if (Input.GetButton("Fire1") && isReady && !isloading)
            {
                if (currentBulletCount > 0)
                {
                    Fire();
                }
                else
                {
                    if (bulletInBagCount > 0)
                    {
                        Reload();
                    }
                    else
                    {
                        Debug.Log("총알이 없습니다.");
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && !isloading && currentBulletCount != maxBulletCount && bulletInBagCount != 0)
            {
                Reload();
            }
        }

        Debug.DrawRay(mainCamera.position, mainCamera.forward * fireDistance, Color.blue);
    }

    void Fire()
    {
        Debug.Log("발사!");

        isReady = false;
        audioSource.Play();
        StartCoroutine(FireReady());
        StartCoroutine(ForwardBackRecoil());
        fireParticle.Play();
        currentBulletCount--;

        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, fireDistance))
        {
            if (hit.transform.gameObject.layer == 8)
            {
                Debug.Log("머리에 맞았음");
                hit.transform.gameObject.GetComponentInParent<Zombie>().DecreaseHp(dmg * 2);
                crossHair.HitHead();
            }
            else if (hit.transform.gameObject.layer == 9)
            {
                Debug.Log("몸통에 맞았음");
                hit.transform.gameObject.GetComponentInParent<Zombie>().DecreaseHp(dmg);
                crossHair.HitBody();
            }
        }

        UpDownLeftRigtRecoil();

        playerController.Walk();

        currentBullet.text = currentBulletCount.ToString();
    }

    IEnumerator FireReady()
    {
        yield return new WaitForSeconds(fireTime);
        isReady = true;
    }

    void Reload()
    {
        isloading = true;
        StartCoroutine(ReloadReady());

        anim.SetTrigger("Reload");

        bulletInBagCount += currentBulletCount;
        if(bulletInBagCount >= maxBulletCount)
        {
            currentBulletCount = maxBulletCount;
            bulletInBagCount -= maxBulletCount;
        }
        else
        {
            currentBulletCount = bulletInBagCount;
            bulletInBagCount = 0;
        }
    }

    IEnumerator ReloadReady()
    {
        yield return new WaitForSeconds(reloadTime);

        currentBullet.text = currentBulletCount.ToString();
        maxBullet.text = bulletInBagCount.ToString();

        isloading = false;
    }

    IEnumerator ForwardBackRecoil()
    {
        for (int i = 0; i < 30; i++)
        {
            float k = Mathf.Lerp(transform.localPosition.z, forwardBackRecoil, 0.4f);
            yield return new WaitForEndOfFrame();
            transform.localPosition = new Vector3(0, 0, k);
        }

        for(int i = 0; i < 50; i++)
        {
            float k = Mathf.Lerp(transform.localPosition.z, 0, 0.1f);
            yield return new WaitForEndOfFrame();
            transform.localPosition = new Vector3(0, 0, k);
        }
    }

    void UpDownLeftRigtRecoil()
    {
        float random = Random.Range(-leftRightRecoil, leftRightRecoil);
        Vector3 rotateVal = new Vector3(-upRecoil, random, 0);

        if (mainCamera.rotation.eulerAngles.x >= 270 && mainCamera.rotation.eulerAngles.x <= 275)
        {
            rotateVal.x = 0;
            Debug.Log("상하반동 불가");
        }

        mainCamera.Rotate(rotateVal);
        changableHolder.Rotate(rotateVal);
    }

    public void IncreaseBullet(int num)
    {
        bulletInBagCount += num;
        maxBullet.text = bulletInBagCount.ToString();
    }

    public bool CanChange()
    {
        if (!isReady) return false;
        if (isloading) return false;

        return true;
    }
}
