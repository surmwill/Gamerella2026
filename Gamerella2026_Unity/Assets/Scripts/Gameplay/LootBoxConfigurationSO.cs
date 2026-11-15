using System.Collections.Generic;
using UnityEngine;
using static GameModifiers;

[CreateAssetMenu(menuName = "LootBox Configuration")]
public class LootBoxConfigurationSO : ScriptableObject
{
    [SerializeField]
    private List<ModifierType> _modifiers = null;
    
    
    
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
