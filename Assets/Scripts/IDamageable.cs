/// <summary>
/// Contract between "things that deal damage" and "things that receive damage"
/// Decouples damage sources from damage receivers
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Apply damage to this entity
    /// </summary>
    /// <param name="damage">Amount of damage to apply</param>
    void TakeDamage(float damage);
}
