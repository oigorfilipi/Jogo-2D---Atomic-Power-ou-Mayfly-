using UnityEngine;
using UnityEngine.UI; // Necessário para controlar o Slider de Vida

public class EnemyHealth : MonoBehaviour
{
    [Header("Atributos")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Interface (UI)")]
    public Slider healthSlider; // Puxa o componente Slider que acabamos de criar

    void Start()
    {
        currentHealth = maxHealth;

        // Configura a barra de vida inicial
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Inimigo tomou " + damage + " de dano! Vida restante: " + currentHealth);

        // Atualiza a barrinha visual toda vez que tomar dano
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Inimigo Caiu! Vai voltar em 3 segundos...");
        gameObject.SetActive(false);
        Invoke(nameof(Respawn), 3f);
    }

    void Respawn()
    {
        // Restaura a vida e liga ele de novo no mesmo lugar
        currentHealth = maxHealth;
        if (healthSlider != null) healthSlider.value = currentHealth;
        
        gameObject.SetActive(true);
        Debug.Log("Inimigo Voltou!");
    }
}