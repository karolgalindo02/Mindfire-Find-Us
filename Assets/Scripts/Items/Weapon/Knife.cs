using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private AudioSource knifeHit;
    [SerializeField] private int damage = 10;
    [SerializeField] private Vector3 attackBoxSize = new Vector3(1.5f, 1.0f, 1.5f);
    [SerializeField] private float attackRange = 1.5f;

    [SerializeField] private Animator animator;


    private void Update()
    {
        // Check if player presses the attack button and can attack

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        // Calculate the center point of the attack area
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Quaternion attackRotation = transform.rotation;

        // Define the collision area using a BoxCollider
        Collider[] hitEnemies = Physics.OverlapBox(attackPoint, attackBoxSize / 2, transform.rotation);

        // Deal damage to enemies in the attack area
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                knifeHit.Play();
                enemy.GetComponent<Enemy>().TakeDamage(damage);
                Debug.Log("You hit your enemy");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPoint = transform.position + transform.forward * attackRange;

        Gizmos.matrix = Matrix4x4.TRS(attackPoint, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackBoxSize);
    }
}