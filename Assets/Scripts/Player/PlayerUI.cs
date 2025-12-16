using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple UI for player health and cooldowns
/// </summary>
public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public PlayerController playerController;
    public AbilitySystem abilitySystem;
    
    [Header("UI Elements")]
    public Slider healthBar;
    public Image dodgeCooldownImage;
    public Image ability1CooldownImage;
    public Image ability2CooldownImage;
    public Image ability3CooldownImage;
    
    void Update()
    {
        if (playerHealth != null && healthBar != null)
        {
            healthBar.value = playerHealth.GetHealthPercent();
        }
        
        if (playerController != null && dodgeCooldownImage != null)
        {
            float cooldownPercent = playerController.GetDodgeCooldownPercent();
            dodgeCooldownImage.fillAmount = cooldownPercent;
        }
        
        if (abilitySystem != null)
        {
            if (ability1CooldownImage != null)
            {
                ability1CooldownImage.fillAmount = abilitySystem.GetCooldownPercent(0);
            }
            if (ability2CooldownImage != null)
            {
                ability2CooldownImage.fillAmount = abilitySystem.GetCooldownPercent(1);
            }
            if (ability3CooldownImage != null)
            {
                ability3CooldownImage.fillAmount = abilitySystem.GetCooldownPercent(2);
            }
        }
    }
}
