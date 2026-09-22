using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 5;

    void Update()
    {
        // Faz o cubo voar para frente
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destrói o cubo assim que bater em qualquer coisa (inimigo ou parede)
        if (!other.CompareTag("Player") && !other.isTrigger) 
        {
            Destroy(gameObject);
        }
    }
}