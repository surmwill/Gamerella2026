using System.Collections.Generic;
using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    // Multiplicative with the player's velocity
    public float ExtraScalePlayerVelocity { get; set; }
    
    // Multiplicative with the enemy's velocity
    public float ExtraScaleEnemyVelocity { get; set; }
    
    public float ExtraPlayerDamage { get; set; }
    
    public float ExtraEnemyDamage { get; set; }
    
    public float ExtraPlayerAttacksPerSecond { get; set; }
    
    /// <summary>
    /// Apply a modifier to everything in the game
    /// </summary>
    public void ApplyModifier(ModifierType modifierType, float modifierValue)
    {
        switch (modifierType)
        {
            // ex: 1.5X -> 1.6X -> 1.7X faster
            case ModifierType.PlayerMoveFaster:
                ExtraScalePlayerVelocity += modifierValue;
                break;
            
            case ModifierType.EnemyMoveFaster:
                ExtraScaleEnemyVelocity += modifierValue;
                break;
            
            case ModifierType.PlayerMoreDamage:
                ExtraPlayerDamage += modifierValue;
                break;
            
            case ModifierType.EnemyMoreDamage:
                ExtraEnemyDamage += modifierValue;
                break;
            
            case ModifierType.PlayerAttackFaster:
                ExtraPlayerAttacksPerSecond += modifierValue;
                break;
        }
    }
    
    public enum ModifierType
    {
        PlayerAttackFaster = 0,
        PlayerMoreDamage = 1,
        PlayerMoveFaster = 2,
        
        // EnemyAttackFaster = 10, // Enemies don't attack, just run into you
        EnemyMoreDamage = 11,
        EnemyMoveFaster = 12,
    }
}
