using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gameplay
{
    public class HitTextPool : MonoBehaviour
    {
        [SerializeField]
        private Transform _hitTextPool = null;

        [SerializeField]
        private TMP_Text _hitTextPrefab = null;

        [SerializeField]
        private int _numInPool = 10;
        
        public TMP_Text FetchTextFromPool()
        {
            TMP_Text text = null;
            if (_hitTextPool.childCount > 0)
            {
                text = _hitTextPool.GetChild(0).GetComponent<TMP_Text>();
            }
            else
            {
                Debug.LogWarning("Text pool empty, using fresh instantiation");
                text = Instantiate(_hitTextPrefab, _hitTextPool);
            }

            text.gameObject.SetActive(true);
            return text;
        }
        
        public void ReturnTextToPool(TMP_Text hitText)
        {   
            hitText.transform.SetParent(_hitTextPool);
            hitText.gameObject.SetActive(false);
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            _numInPool = Mathf.Max(_numInPool, 0);
        
            if (_hitTextPool == null || _hitTextPrefab == null)
            {
                return;
            }
        
            for (int i = _hitTextPool.childCount; i < _numInPool; i++)
            {
                TMP_Text hitText = (TMP_Text) PrefabUtility.InstantiatePrefab(_hitTextPrefab);
                hitText.transform.SetParent(_hitTextPool);
                hitText.gameObject.SetActive(false);
            }

            EditorApplication.delayCall += () =>
            {
                for (int i = _hitTextPool.childCount; i > _numInPool; i--)
                {
                    DestroyImmediate(_hitTextPool.GetChild(0).gameObject);
                }
            };
        }
        #endif
    }
}