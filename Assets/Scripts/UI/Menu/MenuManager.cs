using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject painelPrincipal; // Controla os botões iniciais
    public GameObject painelOpcoes;    // Controla a tela de opções

    public void IniciarJogo()
    {
        SceneManager.LoadScene("Gameplay"); 
    }

    public void AbrirOpcoes()
    {
        if (painelOpcoes != null && painelPrincipal != null)
        {
            painelPrincipal.SetActive(false); // Desliga os botões do menu
            painelOpcoes.SetActive(true);     // Liga a tela de opções
        }
    }

    public void FecharOpcoes()
    {
        if (painelOpcoes != null && painelPrincipal != null)
        {
            painelOpcoes.SetActive(false);    // Desliga a tela de opções
            painelPrincipal.SetActive(true);  // Liga os botões do menu de volta
        }
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do Jogo...");
        Application.Quit();
    }
}