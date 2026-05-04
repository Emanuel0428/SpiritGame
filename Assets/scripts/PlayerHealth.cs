using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 6; // 3 corazones × 2 mitades = 6 puntos de vida
    private int currentHealth;

    [Header("Events")]
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent OnPlayerDeath;

    [Header("Damage Settings")]
    [SerializeField] private float invulnerabilityTime = 1.5f; // Tiempo de invulnerabilidad después de recibir daño
    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Update()
    {
        // Manejar el temporizador de invulnerabilidad
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0f)
            {
                isInvulnerable = false;
            }
        }
    }

    public void TakeDamage(int damage = 1)
    {
        if (isInvulnerable || currentHealth <= 0)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        // Activar invulnerabilidad
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityTime;

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount = 1)
    {
        if (currentHealth >= maxHealth)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
        Debug.Log("Player has died!");
        // Aquí puedes agregar lógica adicional como reiniciar el nivel
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    // Método para testing - presiona H para recibir daño
    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.H)
        {
            TakeDamage(1);
        }
    }
}
