using System.Collections.Generic;
using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    [SerializeField]
    private float _extraScalePlayerMoveVelocity = 1f;
    
    [SerializeField]
    private float _extraScaleEnemyMoveVelocity = 1f;

    [SerializeField]
    private int _extraPlayerDamage = 0;

    [SerializeField]
    private int _extraEnemyDamage = 0;
    
    [SerializeField]
    private int _extraPlayerAttacksPerSecond = 0;

    public List<(ModifierType ModifierType, float ModifierValue)> LastModifiers { get; private set; } = new();

    public static readonly Dictionary<ModifierType, string> GoodModifiersText = new()
    {
        [ModifierType.PlayerAttacksPerSecond] = "Increased Player Attack Speed",
        [ModifierType.PlayerDamage] = "Increased Player Damage",
        [ModifierType.PlayerMoveSpeed] = "Increased Player Move Speed",

        [ModifierType.EnemyDamage] = "Reduced Enemy Damage",
        [ModifierType.EnemyMoveSpeed] = "Reduced Enemy Move Speed",
    };
    
    public static readonly Dictionary<ModifierType, string> BadModifiersText = new()
    {
        [ModifierType.PlayerAttacksPerSecond] = "Reduced Player Attack Speed",
        [ModifierType.PlayerDamage] = "Reduced Player Damage",
        [ModifierType.PlayerMoveSpeed] = "Reduced Player Move Speed",

        [ModifierType.EnemyDamage] = "Increased Enemy Damage",
        [ModifierType.EnemyMoveSpeed] = "Increased Enemy Move Speed",
    };
    
    // Multiplicative with the player's velocity
    public float ExtraPlayerMoveVelocity
    {
        get => _extraScalePlayerMoveVelocity; 
        set => _extraScalePlayerMoveVelocity = value;
    }
    
    // Multiplicative with the enemy's velocity
    public float ExtraEnemyMoveVelocity
    {
        get => _extraScaleEnemyMoveVelocity;
        set => _extraScaleEnemyMoveVelocity = value;
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

    public int ExtraPlayerAttacksPerSecond
    {
        get => _extraPlayerAttacksPerSecond; 
        set => _extraPlayerAttacksPerSecond = value;
    }
    
    /// <summary>
    /// Apply a modifier to everything in the game
    /// </summary>
    public void ApplyModifier(ModifierType modifierType, float modifierValue)
    {
        LastModifiers.Add((modifierType, modifierValue));
        
        switch (modifierType)
        {
            // ex: 1.5X -> 1.6X -> 1.7X faster
            case ModifierType.PlayerMoveSpeed:
                ExtraPlayerMoveVelocity += modifierValue;
                break;
            
            case ModifierType.EnemyMoveSpeed:
                ExtraEnemyMoveVelocity += modifierValue;
                break;
            
            case ModifierType.PlayerDamage:
                ExtraPlayerDamage += (int) modifierValue;
                break;
            
            case ModifierType.EnemyDamage:
                ExtraEnemyDamage += (int) modifierValue;
                break;
            
            case ModifierType.PlayerAttacksPerSecond:
                ExtraPlayerAttacksPerSecond += (int) modifierValue;
                break;
        }
    }
    
    public enum ModifierType
    {
        PlayerAttacksPerSecond = 0,
        PlayerDamage = 1,
        PlayerMoveSpeed = 2,
        
        // Enemies don't have attack speed
        EnemyDamage = 11,
        EnemyMoveSpeed = 12,
    }
}
