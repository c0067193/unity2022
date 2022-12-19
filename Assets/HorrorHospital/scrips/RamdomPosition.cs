using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RamdomPosition : MonoBehaviour
{
    private float speed = 5f;
    //public Vector3 nextPos;
    NavMeshAgent agent;
    public player player;
    bool hiding = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
       

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
         Vector3 destination = transform.position;
         Vector2 randomDirection = UnityEngine.Random.insideUnitCircle * 8.0f;
         destination.x += randomDirection.x;
         destination.z += randomDirection.y;
        
         NavMeshHit navHit;
         NavMesh.SamplePosition(destination, out navHit, 8.0f, NavMesh.AllAreas);
        
         return navHit.position;
     }
    Vector3 PickHidingPlace()
 {
        Vector3 directionToPlayer = (player.transform.position -
        transform.position ). normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position -
        (directionToPlayer* 8.0f), out navHit , 8.0f, NavMesh.AllAreas );

        return navHit.position;
 }

    // Update is called once per frame
    void Update()
    {
        if (!hiding || agent.remainingDistance < 0.05f)
        {
            hiding = true;
            agent.destination = PickHidingPlace();
        }
    }
}
