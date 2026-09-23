using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento & Dash")]
    public float speed = 7f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing;
    private bool canDash = true;
    private Animator anim;

    [Header("Combate Básico & Luva")]
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public bool hasRedGlove = false;
    private int baseDamage;
    private GameObject currentGloveItem;

    [Header("Poder da Maçã (Veneno)")]
    public bool hasAppleInInventory = false;
    public bool hasEatenApple = false;
    public int poisonDamage = 5;
    public float poisonRange = 10f;

    public GameObject projetilPrefab;
    
    [Header("Interface (UI)")]
    public GameObject iconeMacaUI;
    public GameObject[] coracoesUI;

    [Header("Vida do Jogador")]
    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody rb;
    private Vector3 movementInput;
    // Padrão olhando para a frente (para a câmera) no início do jogo
    private Vector3 lastDirection = new Vector3(0f, 0f, -1f); 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        baseDamage = attackDamage;
        currentHealth = maxHealth;

        // Força a animação inicial a olhar para frente
        anim.SetFloat("MoveX", 0f);
        anim.SetFloat("MoveZ", -1f);
    }

    void Update()
    {
        if (isDashing) return;

        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveZ = Input.GetAxisRaw("Vertical");
        
        // Mantém o movimento físico fluido em todas as direções
        movementInput = new Vector3(moveX, 0f, moveZ).normalized;

        // Lógica Visual e de Animação
        if (movementInput != Vector3.zero)
        {
            // Se estiver andando na diagonal, dá prioridade para a animação lateral.
            // Isso evita que o Animator trave no meio da Blend Tree.
            if (Mathf.Abs(moveX) > 0)
            {
                lastDirection = new Vector3(Mathf.Sign(moveX), 0f, 0f);
            }
            else
            {
                lastDirection = new Vector3(0f, 0f, Mathf.Sign(moveZ));
            }

            // Envia apenas 1, 0 ou -1 para o Animator
            anim.SetFloat("MoveX", Mathf.Abs(lastDirection.x)); 
            anim.SetFloat("MoveZ", lastDirection.z);
        }

        // Lógica de Inversão Visual (Flip) baseada na última direção registrada
        if (lastDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (lastDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Comandos de Ação
        if (Input.GetKeyDown(KeyCode.Space) && canDash && movementInput != Vector3.zero)
            StartCoroutine(DashRoutine());

        if (Input.GetMouseButtonDown(0))
            MeleeAttack();

        if (Input.GetKeyDown(KeyCode.Q) && hasRedGlove)
            UnequipGlove();

        if (Input.GetKeyDown(KeyCode.C) && hasAppleInInventory && !hasEatenApple)
            EatApple();

        if (Input.GetMouseButtonDown(1) && hasEatenApple)
            ShootPoison();

        // TESTE: Simula o jogador tomando 1 de dano
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return; 
        rb.MovePosition(rb.position + movementInput * speed * Time.fixedDeltaTime);
    }

    // --- SISTEMA DE VIDA DO JOGADOR ---
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Jogador tomou dano! Vida restante: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Game Over! Reiniciando cena...");
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < coracoesUI.Length; i++)
        {
            if (i < currentHealth)
            {
                coracoesUI[i].SetActive(true);
            }
            else
            {
                coracoesUI[i].SetActive(false);
            }
        }
    }

    // --- FUNÇÕES DE AÇÃO ---
    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;
        rb.linearVelocity = movementInput * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void MeleeAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyHealth enemy = hitCollider.GetComponent<EnemyHealth>();
            if (enemy != null && hitCollider.gameObject != this.gameObject)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    public void EquipGlove(int extraDamage, GameObject gloveObject)
    {
        hasRedGlove = true;
        attackDamage = baseDamage + extraDamage;
        currentGloveItem = gloveObject;
        currentGloveItem.SetActive(false);
    }

    private void UnequipGlove()
    {
        hasRedGlove = false;
        attackDamage = baseDamage;
        currentGloveItem.transform.position = transform.position + new Vector3(1.5f, 0, 0);
        currentGloveItem.SetActive(true);
        currentGloveItem = null;
    }

    public void StoreApple()
    {
        hasAppleInInventory = true;
        if (iconeMacaUI != null) iconeMacaUI.SetActive(true);
    }

    private void EatApple()
    {
        hasAppleInInventory = false;
        hasEatenApple = true;
    }

    private void ShootPoison()
    {
        if (projetilPrefab != null)
        {
            Instantiate(projetilPrefab, transform.position + Vector3.up, Quaternion.LookRotation(lastDirection));
            Debug.Log("Atirou o cubo verde!");
        }
    }

    public void RemoveApplePower()
    {
        hasEatenApple = false;
        if (iconeMacaUI != null) iconeMacaUI.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        if (hasEatenApple)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, lastDirection * poisonRange);
        }
    }
}