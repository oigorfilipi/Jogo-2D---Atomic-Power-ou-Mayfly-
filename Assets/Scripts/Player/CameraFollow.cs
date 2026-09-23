using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configurações")]
    public Transform alvo; // O personagem que será seguido
    public float suavidade = 5f; // Velocidade com que a câmera acompanha

    private Vector3 distanciaOffset;

    void Start()
    {
        // Se o alvo estiver conectado, calcula automaticamente a distância 
        // exata que a câmera já está do personagem lá na cena da Unity.
        if (alvo != null)
        {
            distanciaOffset = transform.position - alvo.position;
        }
    }

    // O LateUpdate roda sempre DEPOIS do Update normal e da física.
    // Isso garante que a câmera só se mova quando o jogador já terminou de dar o passo.
    void LateUpdate()
    {
        if (alvo == null) return;

        // Descobre onde a câmera deveria estar agora
        Vector3 posicaoDesejada = alvo.position + distanciaOffset;
        
        // Move a câmera suavemente da posição atual para a posição desejada
        transform.position = Vector3.Lerp(transform.position, posicaoDesejada, suavidade * Time.deltaTime);
    }
}