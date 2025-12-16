using UnityEngine;

/// <summary>
/// Base class for ability effects - pure data with no logic
/// </summary>
public abstract class EffectData : ScriptableObject
{
    public string effectName;
    
    /// <summary>
    /// Called when the effect is dispatched
    /// </summary>
    public abstract void Dispatch(EffectContext context);
}

/// <summary>
/// Context information for effect execution
/// </summary>
public class EffectContext
{
    public GameObject source;
    public Vector3 position;
    public Vector3 direction;
    public Quaternion rotation;
    public float timestamp;
}
