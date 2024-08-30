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
    [SerializeField] private float lateralOffset = 0.5f;
    [SerializeField] private CameraSwitch cameraSwitch;

    [SerializeField] private Animator animator;

    private void Start()
    {
        if (cameraSwitch == null)
        {
            cameraSwitch = FindObjectOfType<CameraSwitch>(); // Encuentra el componente en la escena si no se asignó en el inspector
        }
    }
    private void Update()
    {
        // Verifica si el botón de ataque se presiona y si está en vista en primera persona
        if (Input.GetButtonDown("Fire1") )
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (Time.timeScale==0) return;
        if(cameraSwitch != null && cameraSwitch.isFirstPesonEnable){
        animator.SetTrigger("Attack");
        }
        // Calculate the center point of the attack area
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        
        Quaternion attackRotation = transform.rotation;
        attackPoint += transform.right * lateralOffset;
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
        attackPoint += transform.right * lateralOffset;

        Gizmos.matrix = Matrix4x4.TRS(attackPoint, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackBoxSize);
    }
}