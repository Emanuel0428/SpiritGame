using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
public class SetupSpikeTrap : MonoBehaviour
{
    [ContextMenu("Setup Spike")]
    public void Setup()
    {
        string spritePath = "Assets/Trap and Weapon/00-All_315.asset";
        Sprite spikeSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        
        if (spikeSprite != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = spikeSprite;
                sr.sortingOrder = 1;
                Debug.Log("Sprite asignado correctamente");
            }
        }
        else
        {
            Debug.LogError("No se pudo cargar el sprite");
        }
    }
}
#endif
