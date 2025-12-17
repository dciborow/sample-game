using UnityEngine;

/// <summary>
/// Effect that spawns a hitbox in space
/// </summary>
[CreateAssetMenu(fileName = "NewHitbox", menuName = "Effects/Hitbox")]
public class HitboxEffect : EffectData
{
    [Header("Hitbox Shape")]
    public HitboxShape shape = HitboxShape.Sphere;
    public Vector3 size = Vector3.one;
    public float radius = 1f;
    public float arcAngle = 90f;
    
    [Header("Hitbox Timing")]
    public float duration = 0.1f;
    
    [Header("Damage")]
    public float damage = 10f;
    
    [Header("Visual (Optional)")]
    public GameObject visualPrefab;
    
    public override void Dispatch(EffectContext context)
    {
        // Spawn a hitbox GameObject that will be processed by other systems
        GameObject hitbox = new GameObject($"Hitbox_{effectName}");
        hitbox.transform.position = context.position;
        hitbox.transform.rotation = context.rotation;
        
        // Add hitbox component
        var hitboxComp = hitbox.AddComponent<Hitbox>();
        hitboxComp.shape = shape;
        hitboxComp.size = size;
        hitboxComp.radius = radius;
        hitboxComp.arcAngle = arcAngle;
        hitboxComp.direction = context.direction;
        hitboxComp.source = context.source;
        hitboxComp.lifetime = duration;
        hitboxComp.damage = damage;
        
        // Optional visual
        if (visualPrefab != null)
        {
            GameObject visual = Instantiate(visualPrefab, context.position, context.rotation);
            visual.transform.SetParent(hitbox.transform);
        }
    }
}

public enum HitboxShape
{
    Sphere,
    Box,
    Cone
}

/// <summary>
/// Component attached to hitbox GameObjects
/// </summary>
public class Hitbox : MonoBehaviour
{
    public HitboxShape shape;
    public Vector3 size;
    public float radius;
    public float arcAngle;
    public Vector3 direction;
    public GameObject source;
    public float lifetime;
    public float damage;
    
    private float elapsedTime;
    
    void Start()
    {
        // Register with EffectResolver for processing
        EffectResolver.RegisterEffect(gameObject);
    }
    
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
