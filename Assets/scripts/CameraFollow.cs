using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    
    [Header("Camera Position")]
    public float fixedX = -2.199f;
    public float cameraZ = -10f;
    
    [Header("Follow Settings")]
    public float smoothSpeed = 0.125f;
    public float yOffset = 0f;
    
    [Header("Bounds")]
    public bool useBounds = true;
    public float minY = -5f;
    public float maxY = 30f;
    
    void LateUpdate()
    {
        if (target == null)
            return;
        
        // Solo seguir en el eje Y, mantener X FIJA
        float desiredY = target.position.y + yOffset;
        
        // Aplicar límites verticales si están habilitados
        if (useBounds)
        {
            desiredY = Mathf.Clamp(desiredY, minY, maxY);
        }
        
        // Suavizar el movimiento vertical
        float smoothedY = Mathf.Lerp(transform.position.y, desiredY, smoothSpeed);
        
        // SIEMPRE mantener X en el valor fijo y Z en la profundidad de cámara
        transform.position = new Vector3(fixedX, smoothedY, cameraZ);
    }
}
