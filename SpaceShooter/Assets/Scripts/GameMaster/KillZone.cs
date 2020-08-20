using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile") == true && other.gameObject.activeInHierarchy)
        {


            ObjectPool.Instance.AddToList(other.gameObject);
        }
        else
            Destroy(other.gameObject);
    }
}
