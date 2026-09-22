using UnityEngine;

public class Toilet : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        // Usa OnTriggerStay para verificar continuamente enquanto o jogador está encostado
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            
            // Se o jogador estiver na área e apertar a tecla "E" (Interagir)
            if (player != null && Input.GetKeyDown(KeyCode.E))
            {
                if (player.hasEatenApple)
                {
                    player.RemoveApplePower();
                }
                else
                {
                    Debug.Log("Você não precisa usar o banheiro agora.");
                }
            }
        }
    }
}