using UnityEngine;

public class ApplePickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            
            // Verifica se já não tem a maçã guardada ou comida
            if (player != null && !player.hasAppleInInventory && !player.hasEatenApple)
            {
                player.StoreApple();
                Destroy(gameObject); // A maçã vai para o "bolso", então some da cena
            }
        }
    }
}