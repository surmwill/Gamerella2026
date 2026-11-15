using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    // Multiplicative with the player's velocity
    public float ExtraScalePlayerVelocity { get; set; }
    
    // Multiplicative with the enemy's velocity
    public float ExtraScaleEnemyVelocity { get; set; }
    
    public float ExtraPlayerDamage { get; set; }
    
    public float ExtraEnemyDamage { get; set; }
    
    public float ExtraPlayerAttackSpeed { get; set; }
    
    /// <summary>
    /// Apply a modifier to everything in the game
    /// </summary>
    public void ApplyModifier(ModifierType modifierType, float modifierValue)
    {
        switch (modifierType)
        {
            case ModifierType.PlayerMoveFaster:
                ExtraScalePlayerVelocity += modifierValue;
                break;
            
            case ModifierType.EnemyMoveFaster:
                ExtraScaleEnemyVelocity += modifierValue;
                break;
            
            case ModifierType.PlayerMoreDamage:
                break;
            
            case ModifierType.EnemyMoreDamage:
                break;
            
            case ModifierType.PlayerAttackFaster:
                break;
        }
    }
    
    public enum ModifierType
    {
        PlayerAttackFaster = 0,
        PlayerMoreDamage = 1,
        PlayerMoveFaster = 2,
        
        EnemyAttackFaster = 10,
        EnemyMoreDamage = 11,
        EnemyMoveFaster = 12,
    }
}
