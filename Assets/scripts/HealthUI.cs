using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Heart Sprites")]
    [SerializeField] private Sprite heartFull;      // Corazón completo (life_0)
    [SerializeField] private Sprite heartHalf;      // Corazón medio (life_1)
    [SerializeField] private Sprite heartEmpty;     // Corazón vacío (life_2)

    [Header("UI Settings")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartsContainer;
    [SerializeField] private float heartSpacing = 50f;
    [SerializeField] private float heartSize = 45f;

    private Image[] heartImages;
    private PlayerHealth playerHealth;

    private void Start()
    {
        // Buscar el PlayerHealth en la escena
        playerHealth = FindObjectOfType<PlayerHealth>();
        
        if (playerHealth == null)
        {
            Debug.LogError("No se encontró PlayerHealth en la escena!");
            return;
        }

        // Suscribirse al evento de cambio de vida
        playerHealth.OnHealthChanged.AddListener(UpdateHearts);

        // Crear los corazones en la UI
        CreateHearts();

        // Actualizar la visualización inicial
        UpdateHearts(playerHealth.GetCurrentHealth());
    }

    private void CreateHearts()
    {
        int maxHealth = playerHealth.GetMaxHealth();
        int numberOfHearts = Mathf.CeilToInt(maxHealth / 2f); // 3 corazones para 6 puntos de vida

        heartImages = new Image[numberOfHearts];

        for (int i = 0; i < numberOfHearts; i++)
        {
            GameObject heart;
            
            if (heartPrefab != null)
            {
                heart = Instantiate(heartPrefab, heartsContainer);
            }
            else
            {
                // Crear un GameObject simple con Image si no hay prefab
                heart = new GameObject($"Heart_{i}");
                heart.transform.SetParent(heartsContainer);
                heart.AddComponent<Image>();
            }

            // Configurar la posición y tamaño
            RectTransform rectTransform = heart.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(i * heartSpacing, 0);
            rectTransform.sizeDelta = new Vector2(heartSize, heartSize);

            // Guardar la referencia al Image
            heartImages[i] = heart.GetComponent<Image>();
            heartImages[i].sprite = heartFull;
            heartImages[i].preserveAspect = true;
        }
    }

    private void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            // Calcular cuánta vida representa este corazón
            int healthForThisHeart = currentHealth - (i * 2);

            if (healthForThisHeart >= 2)
            {
                // Corazón completo
                heartImages[i].sprite = heartFull;
            }
            else if (healthForThisHeart == 1)
            {
                // Corazón medio
                heartImages[i].sprite = heartHalf;
            }
            else
            {
                // Corazón vacío
                heartImages[i].sprite = heartEmpty;
            }
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged.RemoveListener(UpdateHearts);
        }
    }
}
