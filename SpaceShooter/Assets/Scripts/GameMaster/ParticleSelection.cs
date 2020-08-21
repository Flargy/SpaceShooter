using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSelection : MonoBehaviour
{

    public enum Particles { Explosion}
    [SerializeField] private Particles particleType;

    public void SpawnParticles()
    {
        ParticleSpawner.Instance.SpawnParticleEffect(particleType, gameObject.transform.position);
    }

    public void Start()
    {
        SpawnParticles();
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnParticles();
    }
}
