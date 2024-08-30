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

    private Animator animator;
    [Header("Enemy Weapon")]
    public GameObject weapon;
    public bool isDead = false;

    void Start()
    {
        agent.destination = destinations[index].position;
        //Search the player, reference
        player = FindObjectOfType<PlayerControllerMF>().gameObject;
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (isDead)
        {
            // Detener todo comportamiento y activar animación de muerte
            Die();
            return;
        }
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

        // Set walking animation when the enemy is moving along the path
        animator.SetBool("isWalking", true);
        animator.SetBool("isAiming", false);

         // Desactivate the weapon when is walking
        if (weapon != null)
        {
            weapon.SetActive(false);
        }
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
        // Set aiming animation when the enemy is following the player
        animator.SetBool("isAiming", true);
        animator.SetBool("isWalking", false);
        //Activate the weapon when is following player
        if (weapon != null)
        {
            weapon.SetActive(true); 
        }
    }
 public void Die()
    {
        animator.SetBool("isDead", true);
        agent.isStopped = true;
        Destroy(gameObject, 2f); 
    }
}
