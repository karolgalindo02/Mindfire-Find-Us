using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{

    [Header("Settings")]

    [SerializeField] private int damage = 10;

    [SerializeField] private float attackRange = 1.5f;

    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private Animator animator;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        //Spot enemies in attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayers);

        //Cause damage to enemy
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
            Debug.Log("You hit your enemy");
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
