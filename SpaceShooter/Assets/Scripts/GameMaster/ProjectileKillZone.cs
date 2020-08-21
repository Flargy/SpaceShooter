using UnityEngine;

public class ProjectileKillZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            ObjectPool.Instance.AddToList(other.gameObject);
        }
        else if (other.gameObject.CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject);
        }
        
    }
}
