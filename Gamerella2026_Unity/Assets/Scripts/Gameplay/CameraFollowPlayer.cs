using ProphetAR;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTransform = null;
    
    private void Update()
    {
        transform.position = _playerTransform.position.WithZ(transform.position.z);
    }
}
