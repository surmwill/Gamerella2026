using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private EventReference _fmodEventReference;
    
    private EventInstance _eventInstance;

    private bool _isStarted = false;

    private void OnEnable()
    {
        if (_isStarted)
        {
            return;
        }
        
        _eventInstance = RuntimeManager.CreateInstance(_fmodEventReference);
        _eventInstance.start();
        _isStarted = true;
    }

    public void StopMenuMusic()
    {
        _isStarted = false;
        _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
