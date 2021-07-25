using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 레이케스트 방식으로 바꾸면서 사용하지 않는 클래스

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    GameObject[] bullets;
    int currBullet;

    [SerializeField] int bulletCount;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject mainCamera;

    private void Awake()
    {
        if (!instance) instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bullets = new GameObject[bulletCount];
        for (int i = 0; i < bulletCount; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.Euler(90f, 0f, 0f));
            bullets[i].SetActive(false);
        }
    }

    public void Fire()
    {
        if (currBullet >= bulletCount) currBullet = 0;
        GameObject bullet = bullets[currBullet];

        bullet.SetActive(true);
        bullet.transform.position = mainCamera.transform.position;
        bullet.GetComponent<Bullet>().Fire();

        currBullet++;
    }
}
