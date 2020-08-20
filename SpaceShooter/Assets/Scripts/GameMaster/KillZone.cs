using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            ObjectPool.Instance.AddToList(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Enemy") == true)
        {
            EnemySpawner.Instance.EnemyOutOfBounds();
            Destroy(other.gameObject);
        }
        else
            Destroy(other.gameObject);
    }
}
