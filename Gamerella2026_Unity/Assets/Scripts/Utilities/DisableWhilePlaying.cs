using System;
using UnityEngine;

public class DisableWhilePlaying : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
