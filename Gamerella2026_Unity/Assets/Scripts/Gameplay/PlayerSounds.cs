using System;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private EventReference _onAttackSoundReference;
    
    [SerializeField]
    private EventReference _onDeathSoundReference;

    [SerializeField]
    private EventReference _onDamagedSoundReference;
    
    [SerializeField] 
    private EventReference _footstepsSoundReference;

    private EventInstance _onAttackSoundInstance;
    private EventInstance _onDeathSoundInstance;
    private EventInstance _onDamagedSoundInstance;
    private EventInstance _footstepsSoundInstance;
    
    private const float FootstepsRepeatDelay = 0.4f;
    
    private Sequence _footstepsSequence;

    private void Awake()
    {
        _footstepsSoundInstance =  RuntimeManager.CreateInstance(_footstepsSoundReference);
        _onAttackSoundInstance = RuntimeManager.CreateInstance(_onAttackSoundReference);
        _onDamagedSoundInstance = RuntimeManager.CreateInstance(_onDamagedSoundReference);
        _onDeathSoundInstance = RuntimeManager.CreateInstance(_onDeathSoundReference);
    }

    public void PlayOnAttack()
    {
        _onAttackSoundInstance.start();
    }

    public void StopOnAttack()
    {
        _onAttackSoundInstance.stop(STOP_MODE.IMMEDIATE);
    }

    public void PlayOnDamaged()
    {
        _onDamagedSoundInstance.start();
    }

    public void StopOnDamaged()
    {
        _onDamagedSoundInstance.stop(STOP_MODE.IMMEDIATE);
    }

    public void PlayOnDeath()
    {
        _onDeathSoundInstance.start();
    }

    public void StopOnDeath()
    {
        _onDeathSoundInstance.stop(STOP_MODE.IMMEDIATE);
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
            .OnKill(() => _footstepsSoundInstance.stop(STOP_MODE.IMMEDIATE));

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
