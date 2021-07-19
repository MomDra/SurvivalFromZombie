using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [SerializeField] int currentBulletCount = 5;
    [SerializeField] int bulletInBagCount = 300;
    int maxBulletCount = 30;

    [SerializeField]
    float fireTime;
    [SerializeField]
    float reloadTime;

    float recoil = -0.07f;

    bool isReady;
    bool isloading;

    AudioSource audioSource;
    Animator anim;
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] PlayerController playerController;

    [SerializeField] Text currentBullet;
    [SerializeField] Text maxBullet;
    

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
        if (Input.GetButton("Fire1") && isReady && !isloading)
        {
            if(currentBulletCount > 0)
            {
                Fire();
            }
            else
            {
                if(bulletInBagCount > 0)
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

    void Fire()
    {
        isReady = false;
        audioSource.Play();
        StartCoroutine(FireReady());
        StartCoroutine(Recoil());
        fireParticle.Play();
        currentBulletCount--;
        BulletManager.instance.Fire();
        Debug.Log("발사!");

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

        currentBullet.text = currentBulletCount.ToString();
        maxBullet.text = bulletInBagCount.ToString();
    }

    IEnumerator ReloadReady()
    {
        yield return new WaitForSeconds(reloadTime);
        isloading = false;
    }

    IEnumerator Recoil()
    {
        for (int i = 0; i < 30; i++)
        {
            float k = Mathf.Lerp(transform.localPosition.z, recoil, 0.4f);
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

    public void IncreaseBullet(int num)
    {
        bulletInBagCount += num;
        maxBullet.text = bulletInBagCount.ToString();
    }
}
