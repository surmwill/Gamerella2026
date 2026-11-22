using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static EnemyFactory;
using static MenuNames;

[CreateAssetMenu(menuName = GameAssetMenu + "Enemy Configurations")]
public class EnemyConfigurationsSO : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Global")]
    [Tooltip("Resistance to being knocked back")]
    [SerializeField]
    private float _knockbackLinearDampening = 3f;

    [Tooltip("Spin speed while being knocked back")]
    [SerializeField]
    private float _knockbackSpinsPerSecond = 1f;

    [Tooltip("Offset for damage text")]
    [SerializeField]
    private float _hitTextOffsetY = 0f;

    [Tooltip("Time the damage text is shown")]
    [SerializeField]
    private float _hitTextDuration = 1f;
    
    [Header("Per Enemy")]
    [SerializeField]
    private List<EnemyConfiguration> _enemyConfigurations = null;
    
    public float KnockbackLinearDampening => _knockbackLinearDampening;
    
    public float KnockbackSpinsPerSecond => _knockbackSpinsPerSecond;
    
    public float HitTextOffsetY => _hitTextOffsetY;
    
    public float HitTextDuration => _hitTextDuration;
    
    public Dictionary<EnemyType, EnemyConfiguration> EnemyConfigs { get; private set; }
    
    public void OnBeforeSerialize()
    {
        // Empty
    }

    public void OnAfterDeserialize()
    {
        EnemyConfigs = _enemyConfigurations.ToDictionary(c => c.EnemyType, c => c);
    }
    
    [Serializable]
    public class EnemyConfiguration
    {
        [SerializeField]
        private EnemyType _enemyType = default;
        
        [SerializeField]
        private EnemyPrefabConfiguration _prefabConfig;
        
        [SerializeField]
        private EnemyBaseStats _baseStats;

        public EnemyType EnemyType => _enemyType;

        public EnemyPrefabConfiguration PrefabConfig => _prefabConfig;

        public EnemyBaseStats BaseStats => _baseStats;
    }
    
    [Serializable]
    public class EnemyPrefabConfiguration
    {
        [SerializeField]
        private Material _material = null;
        
        public Material Material => _material;
    }
    
    [Serializable]
    public class EnemyBaseStats
    {
        [SerializeField]
        private int _baseHealth = 100;

        [SerializeField]
        private float _baseMoveVelocity = 10f;

        [SerializeField]
        private int _baseAttackDamage = 10;

        [SerializeField]
        private int _tokensDropped = 1;

        public int BaseHealth => _baseHealth;
        
        public float BaseMoveVelocity => _baseMoveVelocity;

        public int BaseAttackDamage => _baseAttackDamage;
        
        public int TokensDropped => _tokensDropped;
    }
}