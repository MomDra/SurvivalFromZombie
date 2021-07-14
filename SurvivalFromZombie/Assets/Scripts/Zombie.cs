using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    [SerializeField] int maxHp;
    int currHp;
    [SerializeField] Rigidbody playerRigid;

    float originSpeed;
    float hitSpeed = 1.5f;

    bool isDead;

    NavMeshAgent agent;
    Animator anim;
    Image hpBar;
    ParticleSystem particle;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hpBar = GetComponentInChildren<Image>();
        particle = GetComponentInChildren<ParticleSystem>();

        anim.SetFloat("MoveSpeed", 1);

        originSpeed = agent.speed;
        currHp = maxHp;
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            agent.destination = playerRigid.position;
        }
    }

    public void DecreaseHp(int dmg)
    {
        Debug.Log(dmg.ToString() + "������ ����");
        agent.speed = hitSpeed;
        StartCoroutine(IncreaseSpeed());

        if (currHp - dmg <= 0)
        {
            currHp = 0;
            Die();
        }
        else
        {
            currHp -= dmg;
        }

        hpBar.fillAmount = (float)currHp / maxHp;

        particle.Play();
    }

    IEnumerator IncreaseSpeed()
    {
        yield return new WaitForSeconds(1f);

        agent.speed = originSpeed;
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Dead");
        agent.destination = transform.position;
    }
}
