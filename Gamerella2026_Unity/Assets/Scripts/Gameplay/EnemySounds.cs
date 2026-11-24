using System;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Gameplay
{
    public class EnemySounds : MonoBehaviour
    {
        [SerializeField]
        private EventReference _footstepsSoundReference = default;
        
        [SerializeField]
        private EventReference _onDamagedSoundReference = default;
        
        [SerializeField]
        private EventReference _onDeathSoundReference = default;
        
        private const float FootstepsRepeatDelay = 0.4f;
        
        private EventInstance _footstepsSoundInstance;
        private EventInstance _onDamagedSoundInstance;
        private EventInstance _onDeathSoundInstance;
        
        private Sequence _footstepsSequence;

        private void Awake()
        {
            _footstepsSoundInstance = RuntimeManager.CreateInstance(_footstepsSoundReference);
            _onDamagedSoundInstance = RuntimeManager.CreateInstance(_onDamagedSoundReference);
            _onDeathSoundInstance = RuntimeManager.CreateInstance(_onDeathSoundReference);
        }

        public void PlayOnDamaged()
        {
            _onDamagedSoundInstance.start();
        }

        public void StopOnDamaged()
        {
            _onDamagedSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        public void PlayOnDeath()
        {
            _onDeathSoundInstance.start();
        }

        public void StopOnDeath()
        {
            _onDeathSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        
        public bool TryPlayFootsteps()
        {
            // Already playing
            if (_footstepsSequence?.IsActive() ?? false)
            {
                return false;
            }

            _footstepsSoundInstance.start();
            _footstepsSequence = DOTween.Sequence()
                .AppendInterval(FootstepsRepeatDelay)
                .AppendCallback(() => _footstepsSoundInstance.start())
                .SetLoops(-1)
                .OnKill(() => _footstepsSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE));

            return true;
        }

        public bool TryStopFootsteps()
        {
            if (_footstepsSequence?.IsActive() ?? false)
            {
                _footstepsSequence.Kill();
                return true;
            }

            return false;
        }
    }
}