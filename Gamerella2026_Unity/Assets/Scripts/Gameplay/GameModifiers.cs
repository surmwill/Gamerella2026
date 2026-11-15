using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    private PlayerController PlayerController => Game.Instance.PlayerController;
    
    // Maintain values of all modifiers to dynamically add to enemies
    
    /// <summary>
    /// Apply a modifier to everything in the game
    /// </summary>
    public void ApplyModifier(ModifierType modifierType)
    {
        switch (modifierType)
        {
            case ModifierType.PlayerMoveFaster:
                break;
            
            case ModifierType.EnemyMoveFaster:
                break;
        }
    }

    /// <summary>
    /// Enemies are spawned dynamically
    /// </summary>
    public void ApplyEnemyModifier()
    {
        
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
