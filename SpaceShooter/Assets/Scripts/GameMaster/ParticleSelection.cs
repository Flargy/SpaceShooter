using UnityEngine;

public class ParticleSelection : MonoBehaviour
{

    public enum Particles { Explosion}
    [SerializeField] private Particles particleType;

    public void SpawnParticles()
    {
        ParticleSpawner.Instance.SpawnParticleEffect(particleType, gameObject.transform.position);
    }

}
