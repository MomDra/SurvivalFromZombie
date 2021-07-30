using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Barrel : MonoBehaviour
{
    Collider[] target;

    [SerializeField] float power;
    [SerializeField] float upForce;
    [SerializeField] float detectDistance;
    [SerializeField] float waitTime;
    [SerializeField] int dmgToPlayer;

    AudioSource audioSource;
    [SerializeField] AudioClip timer;
    [SerializeField] AudioClip explosion;

    MeshRenderer render;

    bool isExplosion;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        render = GetComponent<MeshRenderer>();

        transform.Find("Spot Light").gameObject.SetActive(true);
        StartCoroutine(Explosion());
    }

    private void FixedUpdate()
    {
        if(!isExplosion) Detect();
    }

    void Detect()
    {
        target = Physics.OverlapSphere(transform.position, detectDistance, 1 << 9);

        for(int i = 0; i<target.Length; i++)
        {
            target[i].GetComponent<Zombie>().RunToBarrel(transform.position);
        }
    }

    IEnumerator Explosion()
    {
        for(int i = 0; i<5; i++)
        {
            yield return new WaitForSeconds(waitTime / 5);

            audioSource.PlayOneShot(timer);
        }
        
        isExplosion = true;

        audioSource.volume = 1f;
        audioSource.PlayOneShot(explosion);

        yield return new WaitForSeconds(1f);

        target = Physics.OverlapSphere(transform.position, detectDistance, 1 << 9);

        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<NavMeshAgent>().enabled = false;
            target[i].GetComponent<Zombie>().GetComponent<Rigidbody>().AddExplosionForce(power, transform.position, detectDistance, upForce, ForceMode.Impulse);
            target[i].GetComponent<Zombie>().DecreaseHp(GameManager.instance.zombieHp);
        }

        GameObject player = GameObject.Find("Player");

        if(Vector3.Distance(transform.position, player.transform.position) <= detectDistance - 0.01f)
        {
            player.GetComponent<Rigidbody>().AddExplosionForce(power, transform.position, detectDistance, upForce, ForceMode.Impulse);
            GameManager.instance.plyerHP = -30;
        }

        Debug.Log("Barrel Æø¹ß!");

        render.enabled = false;
        Destroy(gameObject, 2f);
    }
}
