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

    bool isExplosion;

    private void Start()
    {
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
        yield return new WaitForSeconds(waitTime);

        isExplosion = true;

        yield return new WaitForFixedUpdate();

        target = Physics.OverlapSphere(transform.position, detectDistance, 1 << 9);

        for (int i = 0; i < target.Length; i++)
        {
            target[i].GetComponent<NavMeshAgent>().enabled = false;
            target[i].GetComponent<Zombie>().GetComponent<Rigidbody>().AddExplosionForce(power, transform.position, detectDistance, upForce, ForceMode.Impulse);
            target[i].GetComponent<Zombie>().DecreaseHp(100);
        }

        GameObject player = GameObject.Find("Player");

        if(Vector3.Distance(transform.position, player.transform.position) <= detectDistance - 0.01f)
        {
            player.GetComponent<Rigidbody>().AddExplosionForce(power, transform.position, detectDistance, upForce, ForceMode.Impulse);
            GameManager.instance.plyerHP = -30;
        }

        Debug.Log("Barrel Æø¹ß!");

        Destroy(gameObject);
    }
}
