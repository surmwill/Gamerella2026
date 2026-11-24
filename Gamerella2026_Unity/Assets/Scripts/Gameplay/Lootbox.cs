using System;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

using static LootBoxConfigurationSO;

public class Lootbox : MonoBehaviour
{
    [SerializeField]
    private LootBoxConfigurationSO _lootBoxConfiguration = null;

    [SerializeField]
    private EventReference _onPickupSoundReference = default;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != Game.Instance.Player.gameObject)
        {
            Debug.Log("Lootbox expected collision with player");
            return;
        }
        
        bool isPositiveEffect = Random.value <= Game.Instance.Casino.lootBoxChance;
        Game.Instance.Casino.ResetLootBoxChance();

        ModifierValue modifier = _lootBoxConfiguration.Modifiers[Random.Range(0, _lootBoxConfiguration.Modifiers.Count)];
        Game.Instance.GameModifiers.ApplyModifier(modifier.Type, modifier.Value * (isPositiveEffect ? 1f : -1f));

        if (isPositiveEffect)
        {
            RuntimeManager.PlayOneShot(_onPickupSoundReference);
        }
        
        Destroy(gameObject);
    }
}
