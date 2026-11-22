using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class PlayerStats
    {
        [SerializeField]
        private int _health = 0;

        [SerializeField]
        private float _attacksPerSecond = 0;

        [SerializeField]
        private int _attackDamage = 0;

        [SerializeField]
        private float _moveVelocity = 0f;

        [SerializeField]
        private float _knockbackForce = 0f;
        
        [SerializeField]
        private float _knockbackDuration = 0f;

        public void Init(PlayerConfigurationSO playerConfiguration)
        {
            Health = playerConfiguration.BaseHealth;
            
            AttacksPerSecond = playerConfiguration.BaseAttacksPerSecond;
            AttackDamage = playerConfiguration.BaseAttackDamage;
            
            MoveVelocity = playerConfiguration.BaseMoveVelocity;

            KnockbackForce = playerConfiguration.KnockbackForce;
            KnockbackDuration = playerConfiguration.KnockbackDuration;
        }

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public float AttacksPerSecond
        {
            get => _attacksPerSecond;
            set => _attacksPerSecond = value;
        }
        
        public float AttackCooldownSeconds => 1f / AttacksPerSecond;

        public int AttackDamage
        {
            get => _attackDamage;
            set => _attackDamage = value;
        }

        public float MoveVelocity
        {
            get => _moveVelocity;
            set => _moveVelocity = value;
        }

        public float KnockbackForce
        {
            get => _knockbackForce;
            set => _knockbackForce = value;
        }

        public float KnockbackDuration
        {
            get => _knockbackDuration;
            set => _knockbackDuration = value;
        }

        #region Modified

        public float ModifiedAttacksPerSecond => AttacksPerSecond + Game.Instance.GameModifiers.ExtraPlayerAttacksPerSecond;

        public float ModifiedAttackCooldownSeconds => 1f / Mathf.Max(ModifiedAttacksPerSecond, 1);
        
        public int ModifiedAttackDamage => Mathf.Max(AttackDamage + Game.Instance.GameModifiers.ExtraPlayerDamage, 1); 
        
        public float ModifiedMoveVelocity => MoveVelocity * Game.Instance.GameModifiers.ExtraScalePlayerVelocity;
        
        #endregion
    }
}