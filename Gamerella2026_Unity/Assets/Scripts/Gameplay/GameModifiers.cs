using System.Collections.Generic;
using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    // Multiplicative with the player's velocity
    public float ExtraScalePlayerVelocity { get; set; } = 1f;
    
    // Multiplicative with the enemy's velocity
    public float ExtraScaleEnemyVelocity { get; set; } = 1f;
    
    public int ExtraPlayerDamage { get; set; }
    
    public int ExtraEnemyDamage { get; set; }
    
    public int ExtraPlayerAttacksPerSecond { get; set; }
    
    /// <summary>
    /// Apply a modifier to everything in the game
    /// </summary>
    public void ApplyModifier(ModifierType modifierType, object modifierValue)
    {
        switch (modifierType)
        {
            // ex: 1.5X -> 1.6X -> 1.7X faster
            case ModifierType.PlayerMoveFaster:
                ExtraScalePlayerVelocity += (float) modifierValue;
                break;
            
            case ModifierType.EnemyMoveFaster:
                ExtraScaleEnemyVelocity += (float) modifierValue;
                break;
            
            case ModifierType.PlayerMoreDamage:
                ExtraPlayerDamage += (int) modifierValue;
                break;
            
            case ModifierType.EnemyMoreDamage:
                ExtraEnemyDamage += (int) modifierValue;
                break;
            
            case ModifierType.PlayerAttackFaster:
                ExtraPlayerAttacksPerSecond += (int) modifierValue;
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
