using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    private PlayerController PlayerController => Game.Instance.PlayerController;
    
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
