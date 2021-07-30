using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    [SerializeField] int maxHp;
    int currHp;
    Rigidbody playerRigid;

    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackDelay = 2f;
    [SerializeField] int attackDmg = 10;
    [SerializeField] bool isSuper;

    float originSpeed;
    float slowSpeed = 1f;

    bool isDead;
    bool canAttack = true;
    bool isRunToBarrel;

    NavMeshAgent agent;
    Animator anim;
    Image hpBar;
    ParticleSystem particle;
    CapsuleCollider bodyCol;
    SphereCollider headCol;
    BoxCollider boxCol;

    Vector3 dest;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hpBar = GetComponentInChildren<Image>();
        particle = GetComponentInChildren<ParticleSystem>();
        playerRigid = GameObject.Find("Player").GetComponent<Rigidbody>();
        bodyCol = GetComponent<CapsuleCollider>();
        headCol = GetComponentInChildren<SphereCollider>();
        boxCol = GetComponent<BoxCollider>();

        anim.SetFloat("MoveSpeed", 1);

        originSpeed = agent.speed;
        currHp = maxHp = GameManager.instance.zombieHp;

        if (isSuper)
        {
            currHp = maxHp = GameManager.instance.zombieHp * 3;
            attackDmg *= 3;
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (!isRunToBarrel) agent.destination = playerRigid.position;
            else agent.destination = dest;


            if (DetectPlayer() && canAttack && !isRunToBarrel)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private void LateUpdate()
    {
        hpBar.transform.LookAt(playerRigid.position);
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
        bodyCol.enabled = false;
        headCol.enabled = false;
        boxCol.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Dead");

        isDead = true;
        anim.SetTrigger("Dead");
        agent.destination = transform.position;

        GameManager.instance.numOfZombieInScene--;

        Destroy(gameObject, 5f);
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

    IEnumerator Attack()
    {
        canAttack = false;
        agent.speed = originSpeed / 1.2f;
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelay / 3);

        if(DetectPlayer()) GameManager.instance.plyerHP = -attackDmg;

        yield return new WaitForSeconds(attackDelay / 3);
        agent.speed = originSpeed;

        yield return new WaitForSeconds(attackDelay / 3);
        canAttack = true;
    }

    public void RunToBarrel(Vector3 pos)
    {
        if (!isRunToBarrel)
        {
            isRunToBarrel = true;
            dest = pos;
            agent.speed = originSpeed * 1.5f;

            if (isSuper) StartCoroutine(RetrackPlayer());
        }
    }

    IEnumerator RetrackPlayer()
    {
        yield return new WaitForSeconds(2f);

        agent.enabled = true;
        isRunToBarrel = false;
    }

}