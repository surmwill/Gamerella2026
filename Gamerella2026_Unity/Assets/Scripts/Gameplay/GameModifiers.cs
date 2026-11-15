using System.Collections.Generic;
using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    [SerializeField]
    private float _extraScalePlayerVelocity = 1f;
    
    [SerializeField]
    private float _extraScaleEnemyVelocity = 1f;

    [SerializeField]
    private int _extraPlayerDamage = 0;

    [SerializeField]
    private int _extraEnemyDamage = 0;
    
    [SerializeField]
    private int _extraPlayerAttacksPerSecond = 0;
    
    // Multiplicative with the player's velocity
    public float ExtraScalePlayerVelocity
    {
        get => _extraScalePlayerVelocity; 
        set => _extraScalePlayerVelocity = value;
    }
    
    // Multiplicative with the enemy's velocity
    public float ExtraScaleEnemyVelocity
    {
        get => _extraScaleEnemyVelocity;
        set => _extraScaleEnemyVelocity = value;
    }

    public int ExtraPlayerDamage
    {
        get => _extraPlayerDamage;
        set => _extraPlayerDamage = value;
    }

    public int ExtraEnemyDamage
    {
        get => _extraEnemyDamage; 
        set => _extraEnemyDamage = value;
    }
    
    public int ExtraPlayerAttacksPerSecond { get; set; }
    
    /// <summary>
    /// Apply a modifier to everything in the game
    /// </summary>
    public void ApplyModifier(ModifierType modifierType, float modifierValue)
    {
        switch (modifierType)
        {
            // ex: 1.5X -> 1.6X -> 1.7X faster
            case ModifierType.PlayerMoveSpeed:
                ExtraScalePlayerVelocity += modifierValue;
                break;
            
            case ModifierType.EnemyMoveSpeed:
                ExtraScaleEnemyVelocity += modifierValue;
                break;
            
            case ModifierType.PlayerDamage:
                ExtraPlayerDamage += (int) modifierValue;
                break;
            
            case ModifierType.EnemyDamage:
                ExtraEnemyDamage += (int) modifierValue;
                break;
            
            case ModifierType.PlayerAttackSpeed:
                ExtraPlayerAttacksPerSecond += (int) modifierValue;
                break;
        }
    }
    
    public enum ModifierType
    {
        PlayerAttackSpeed = 0,
        PlayerDamage = 1,
        PlayerMoveSpeed = 2,
        
        // Enemies don't have attack speed
        EnemyDamage = 11,
        EnemyMoveSpeed = 12,
    }
}
