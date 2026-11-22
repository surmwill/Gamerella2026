using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = MenuNames.GameAssetMenu + "Player Configuration")]
public class PlayerConfigurationSO : ScriptableObject
{
    [FormerlySerializedAs("_baseScaleVelocity")]
    [SerializeField]
    private float _baseMoveVelocity = 30f;
    
    [SerializeField]
    private int _baseAttackDamage = 10;

    [SerializeField]
    private float _baseAttacksPerSecond = 10f;

    [SerializeField]
    private int _baseHealth = 100;

    [Tooltip("How hard to knock enemies back")]
    [SerializeField]
    private float _knockbackForce = 25f;
    
    [Tooltip("How long the enemies should be knocked back for in seconds")]
    [SerializeField]
    private float _knockbackDuration = 1f;

    public float BaseMoveVelocity => _baseMoveVelocity;

    public int BaseAttackDamage => _baseAttackDamage;
    
    public float BaseAttacksPerSecond => _baseAttacksPerSecond;
    
    public int BaseHealth => _baseHealth;
    
    public float KnockbackForce => _knockbackForce;
    
    public float KnockbackDuration => _knockbackDuration;
}
