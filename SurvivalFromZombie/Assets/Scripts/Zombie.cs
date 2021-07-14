using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{

    [SerializeField] int hp;

    [SerializeField] Rigidbody playerPos;

    float originSpeed;
    float hitSpeed = 1.5f;

    bool isDead;

    NavMeshAgent agent;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        anim.SetFloat("MoveSpeed", 1);

        originSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            agent.destination = playerPos.position;
        }
    }

    public void DecreaseHp(int dmg)
    {
        Debug.Log(dmg.ToString() + "데미지 입힘");
        agent.speed = hitSpeed;
        StartCoroutine(IncreaseSpeed());

        if (hp - dmg <= 0)
        {
            hp = 0;
            Die();
        }
        else
        {
            hp -= dmg;
        }
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
    }
}
