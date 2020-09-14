using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy") == true)
        {
            EnemySpawner.Instance.EnemyOutOfBounds();
            other.GetComponent<EnemyBase>().removeFromList();
        }

    }
}
