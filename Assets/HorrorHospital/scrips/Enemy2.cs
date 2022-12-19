using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    public player player;
    public float attackDistance;
    public int damage;
    public int health;
    public weapon weapon;

    private bool isAttacking;
    public bool isDead;

    public NavMeshAgent agent;
    public Animator anim;
    public bool hiding = false;

    float[] probArray = { 0.5f, 0.5f };
    int ProbValue;
    private Rigidbody rig;
    int getRangeNum = 0;
    int rangeRadomNum = 0;
    int numCount = 0;
 

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = PickRandomPosition();
        
    }

    private void Update()
    {
        
        if (isDead)
            return;
        if (CanSeePlayer())
        {
         
            if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
            {
               
                agent.isStopped = true;

                if (!isAttacking&&!hiding)
                    Attack();
                if (health < 5 && !hiding)
                {
                   

                    GetRandomSequence2(4);
                    Debug.Log(numCount.ToString() + "di" + getRangeNum);

                    if (numCount == 1 && (getRangeNum == 1 || getRangeNum == 3))
                    {
                        Debug.Log("ao");
                        
                        hiding = true;

                        agent.destination = PickHidingPlace();
                        anim.SetBool("Running", false);

                        anim.SetBool("Hurt", true);
                        

                    }

                    else if (numCount == 1 && getRangeNum == 2)
                    {

                        hiding = false;
                        anim.SetBool("Running", false);

                        anim.SetBool("thanatosis", true);
                       

                    }
                    else if(numCount == 1 && getRangeNum == 0)
                        hiding = false;
                     

                }

            }

            else if(!hiding)
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                anim.SetBool("patrol", false);
                anim.SetBool("Running", true);
               // Debug.Log(transform.position == player.transform.position);
            }
        }
        
        else
        {
            if (agent.remainingDistance < 0.5f&&!hiding)
            {
            
                agent.destination = PickRandomPosition();
               
                GetRandomSequence2(4);
              //  Debug.Log(numCount.ToString() + "di" + getRangeNum);
                if (numCount == 2 && (getRangeNum == 1|| getRangeNum == 2|| getRangeNum == 3))
                anim.SetBool("patrol", true);
                if(numCount == 2 && getRangeNum == 0)
                    anim.SetBool("Running", true);

            }
        }
        



    }
    bool CanSeePlayer()
    {
        Vector3 rayPos = transform.position;
        Vector3 rayDir = (player.transform.position - rayPos).normalized;

        RaycastHit info;
        if (Physics.Raycast(rayPos, rayDir, out info))
        {
            if (info.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
    Vector3 PickRandomPosition()
    {

        
        Vector3 directionToPlayer = (player.transform.position -
        transform.position).normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position -
        (directionToPlayer * 8.0f), out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }

    private Vector3 PickHidingPlace()
    {
        Debug.Log("lalal");
        Vector3 directionToPlayer = (player.transform.position -
        transform.position).normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position -
        (directionToPlayer * 8.0f), out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }
    void Attack()
    {
        isAttacking = true;
        anim.SetBool("Running", false);
        anim.SetTrigger("Attack");

        Invoke("TryDamage", 1.2f);
        Invoke("DisableIsAttacking", 2.5f);
    }

    void TryDamage()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
           
            player.TakeDamage(damage);
        }
    }
    void DisableIsAttacking()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damageToTake)
    {
        Debug.Log("sss");
        health -= damageToTake;
        
        if (health <= 0)
        {
            player.score+=1;
            
            UI.instance.UpdateScoreText(player.score);
            GameManager.instance.addScore(player.score);
            player.currentHP += 10;
            isDead = true;
            agent.isStopped = true;
            //disable animation
            anim.SetTrigger("Die");
        }
    }
    public int GetRandomSequence2(int a)
    {
        numCount++;
        do
        {
            rangeRadomNum = Random.Range(0, a);

        }
        while (getRangeNum == rangeRadomNum);
        getRangeNum = rangeRadomNum;

       
        return getRangeNum;
        return numCount;
    }
    
    
}
