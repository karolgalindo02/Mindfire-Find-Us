using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    //Reference Nav Mesh Agent
    public NavMeshAgent agent;

    //Array with the points
    [SerializeField] private Transform[] destinations;

    //For modified the route of the enemy
    private int index = 0;


    [Header("-AI is searching the player?")]
    public bool isFollowingPlayer;

    private float distanceToPlayer;

    public float distanceToFollowPlayer = 10;

    //The correct way to conect with other GameObject, because sometimes you can forget to attach the element.
    private GameObject player;

    public float distanceToFollowPath = 2;


    void Start()
    {
        agent.destination = destinations[index].position;
        //Search the player, reference
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    void Update()
    {
        //Vector3.Distance() need 2 parameters for the calculation, in this case the position of the enemy and the position of the player
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer < distanceToFollowPlayer && isFollowingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            EnemyPath();
        }
    }

    public void EnemyPath()
    {
        agent.destination = destinations[index].position;

        if(Vector3.Distance(transform.position, destinations[index].position) <= distanceToFollowPath)
        {
            if (destinations[index] != destinations[destinations.Length - 1])
            {
                index++;
            }
            else
            {
                index = 0;
            }
        }
    }

    public void FollowPlayer()
    {
        agent.destination = player.transform.position;
    }

}
