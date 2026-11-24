using System;
using System.Collections.Generic;
using UnityEngine;

using static GameModifiers;
using static MenuNames;

[CreateAssetMenu(menuName = GameAssetMenu + "LootBox Configuration")]
public class LootBoxConfigurationSO : ScriptableObject
{
    [SerializeField]
    private float _dropChance = 0.05f;
    
    [SerializeField]
    private List<ModifierValue> _modifiers = null;
    
    public float DropChance => _dropChance;
    
    public List<ModifierValue> Modifiers => _modifiers;
    
    [Serializable]
    public class ModifierValue
    {
        [SerializeField]
        private ModifierType _type = default;
        
        [SerializeField]
        private float _value = 0f;
        
        public ModifierType Type => _type;
        
        public float Value => _value;
    }
}
