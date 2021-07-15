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

    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackDelay = 2f;

    float originSpeed;
    float slowSpeed = 1.5f;

    bool isDead;
    bool canAttack = true;

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


            if (DetectPlayer() && canAttack)
            {
                Attack();
            }
        }
    }

    public void DecreaseHp(int dmg)
    {
        Debug.Log(dmg.ToString() + "데미지 입힘");
        agent.speed = slowSpeed;
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

    bool DetectPlayer()
    {
        if(Vector3.Distance(transform.position, playerRigid.position) <= attackRange)
        {
            Debug.Log("플레이어가 근처에 있음");
            return true;
        }

        return false;
    }

    void Attack()
    {
        canAttack = false;
        agent.speed = 0;
        StartCoroutine(ResetAttack());
        anim.SetTrigger("Attack");
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackDelay / 2);
        agent.speed = originSpeed;
        yield return new WaitForSeconds(attackDelay / 2);
        canAttack = true;
    }
}
