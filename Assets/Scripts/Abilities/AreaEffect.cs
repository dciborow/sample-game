using UnityEngine;

/// <summary>
/// Effect that spawns an area at a position
/// </summary>
[CreateAssetMenu(fileName = "NewArea", menuName = "Effects/Area")]
public class AreaEffect : EffectData
{
    [Header("Area Shape")]
    public float areaRadius = 2f;
    
    [Header("Area Timing")]
    public float duration = 0.1f;
    
    [Header("Visual (Optional)")]
    public GameObject visualPrefab;
    
    public override void Dispatch(EffectContext context)
    {
        // Spawn an area GameObject that will be processed by other systems
        GameObject area = new GameObject($"Area_{effectName}");
        area.transform.position = context.position;
        
        // Add area component
        var areaComp = area.AddComponent<AreaMarker>();
        areaComp.radius = areaRadius;
        areaComp.source = context.source;
        areaComp.lifetime = duration;
        
        // Optional visual
        if (visualPrefab != null)
        {
            GameObject visual = Instantiate(visualPrefab, context.position, Quaternion.identity);
            visual.transform.SetParent(area.transform);
        }
    }
}

/// <summary>
/// Component attached to area GameObjects
/// </summary>
public class AreaMarker : MonoBehaviour
{
    public float radius;
    public GameObject source;
    public float lifetime;
    
    private float elapsedTime;
    
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
