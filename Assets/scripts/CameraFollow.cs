using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    
    [Header("Camera Position")]
    public float fixedX = -2.2f;
    public float cameraZ = -10f;
    
    [Header("Follow Settings")]
    public float smoothSpeed = 0.125f;
    public float yOffset = 0f;
    
    [Header("Bounds")]
    public bool useBounds = true;
    public float minY = -5f;
    public float maxY = 30f;
    
    [Header("Orthographic Size")]
    public bool adjustSizeForMobile = true;
    public float pcOrthographicSize = 2.03366f;
    public float mobileOrthographicSize = 1.7f;  // Más zoom (número menor = más cerca)
    
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        
        // Ajustar el tamaño ortográfico según la plataforma
        if (adjustSizeForMobile && cam != null && cam.orthographic)
        {
            if (IsMobileDevice())
            {
                cam.orthographicSize = mobileOrthographicSize;
            }
            else
            {
                cam.orthographicSize = pcOrthographicSize;
            }
        }
    }
    
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
    
    private bool IsMobileDevice()
    {
        #if UNITY_EDITOR
        // En el editor, detectar por aspect ratio del Game View
        float aspectRatio = (float)Screen.width / Screen.height;
        return aspectRatio > 1.7f && aspectRatio < 2.5f; // Aspect ratio típico de móvil en landscape
        #else
        return Application.platform == RuntimePlatform.Android ||
               Application.platform == RuntimePlatform.IPhonePlayer;
        #endif
    }
}
