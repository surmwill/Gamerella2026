using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudio : MonoBehaviour
{
    [SerializeField]
    private Button _button = null;
    
    [SerializeField]
    private EventReference _event = default;

    private void Start()
    {
        _button.onClick.AddListener(PlayOneShot);
    }

    private void PlayOneShot()
    {
        RuntimeManager.PlayOneShot(_event);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(PlayOneShot);
    }

    private void OnValidate()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
        }
    }
}