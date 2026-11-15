using System;
using UnityEngine;
using Random = UnityEngine.Random;

using static LootBoxConfigurationSO;

public class Lootbox : MonoBehaviour
{
    public const float LootboxDropChance = 0.05f;
    
    [SerializeField]
    private LootBoxConfigurationSO _lootBoxConfiguration = null;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != Game.Instance.Player.gameObject)
        {
            Debug.Log("Lootbox expected collision with player");
            return;
        }
        
        // Check positive or negative effect
        bool isPositiveEffect = true;

        ModifierValue modifier = _lootBoxConfiguration.Modifiers[Random.Range(0, _lootBoxConfiguration.Modifiers.Count)];
        Game.Instance.GameModifiers.ApplyModifier(modifier.Type, modifier.Value * (isPositiveEffect ? 1f : -1f));
        
        Destroy(gameObject);
    }
}
