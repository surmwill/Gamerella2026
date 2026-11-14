using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public void OnSpawned()
    {
        
    }

    public void KillEnemy()
    {
        Game.Instance.EnemyFactory.ReturnEnemyToPool(this);   
    }
}
