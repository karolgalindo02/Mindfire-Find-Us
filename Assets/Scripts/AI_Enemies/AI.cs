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
        if (destinations.Length == 0)
        {
            Debug.LogError("No destinations set for AI.");
            return;
        }
        agent.destination = destinations[index].position;
        //Search the player, reference
        player = FindObjectOfType<PlayerControllerMF>().gameObject;
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        if (!agent)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (isDead)
        {
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
        SetAnimatorState(isWalking: true, isAiming: false, isDead: false);

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
        agent.ResetPath(); 
        agent.destination = player.transform.position;
        // Set aiming animation when the enemy is following the player
        SetAnimatorState(isWalking: false, isAiming: true, isDead: false);
        //Activate the weapon when is following player
        if (weapon != null)
        {
            weapon.SetActive(true); 
        }
    }
    public void Die()
    {
        SetAnimatorState(isWalking: false, isAiming: false, isDead: true);
        agent.isStopped = true;
    }    private void SetAnimatorState(bool isWalking, bool isAiming, bool isDead)
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isAiming", isAiming);
        animator.SetBool("isDead", isDead);
    }
}
