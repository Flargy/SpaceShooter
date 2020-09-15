using System.Collections;
using UnityEngine;

public class ShieldParticlesRotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotation = Vector3.zero;
    [SerializeField] private float rotationSpeed = 1.0f;

    private bool rotationRoutineRunning = false;
    private float shieldDuration = 0.0f;

    // Update is called once per frame
    void Start()
    {
        shieldDuration = gameObject.GetComponentInParent<PlayerBehaviour>().GetShieldDuration();
    }

    public void ActivateShield()
    {
        if(rotationRoutineRunning == false)
        {
            StartCoroutine(RotateShield());
        }
    }

    private IEnumerator RotateShield()
    {
        rotationRoutineRunning = true;
        float timer = 0.0f;
        while(timer <= shieldDuration)
        {
            transform.Rotate(rotation * Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = Quaternion.identity;
        rotationRoutineRunning = false;

    }
}
