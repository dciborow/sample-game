using UnityEngine;

/// <summary>
/// Contract between "things that deal damage" and "things that receive damage"
/// Decouples damage sources from damage receivers
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Apply damage to this entity
    /// </summary>
    /// <param name="amount">Amount of damage to apply</param>
    /// <param name="hitPoint">World position where the damage occurred</param>
    void TakeDamage(int amount, Vector3 hitPoint);
}
