using UnityEngine;

public class PowerPickup : MonoBehaviour
{
    public int bonusDamage = 40;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem entrou na área do gatilho tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            
            // Se o jogador existe e ainda não está com a luva equipada
            if (player != null && !player.hasRedGlove)
            {
                player.EquipGlove(bonusDamage, this.gameObject);
            }
        }
    }
}