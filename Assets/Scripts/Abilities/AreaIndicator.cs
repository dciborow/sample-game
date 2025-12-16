using UnityEngine;

/// <summary>
/// Visual indicator for ground-targeted area abilities
/// </summary>
public class AreaIndicator : MonoBehaviour
{
    [Header("Animation")]
    public float scaleSpeed = 5f;
    public float maxScale = 1f;
    
    private Vector3 targetScale;
    
    void Start()
    {
        targetScale = new Vector3(maxScale, 0.1f, maxScale);
        transform.localScale = Vector3.zero;
    }
    
    void Update()
    {
        // Animate scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }
}
