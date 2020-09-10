using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    static private ParticleSpawner instance = null;

    static public ParticleSpawner Instance { get { return instance; } }

    public enum Particles { Explosion}

    [SerializeField] private Dictionary<Particles, List<ParticleSystem>> particleCollection = new Dictionary<Particles, List<ParticleSystem>>();
    [SerializeField] private List<ParticleSystem> particles = new List<ParticleSystem>();

    [SerializeField] private int cachedParticles = 15;
    private Dictionary<Particles, ParticleSystem> particleTypes = new Dictionary<Particles, ParticleSystem>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        particleTypes = new Dictionary<Particles, ParticleSystem>();
        particleCollection = new Dictionary<Particles, List<ParticleSystem>>();
        int index = 0;

        foreach (Particles type in Enum.GetValues(typeof(Particles)))
        {
            particleTypes.Add(type, particles[index]);
            particleCollection.Add(type, new List<ParticleSystem>());
            particleCollection.TryGetValue(type, out List<ParticleSystem> value);
            for (int i = 0; i < cachedParticles; i++)
            {
                ParticleSystem particleSystem = Instantiate(particles[index]);
                value.Add(particleSystem);
                particleSystem.Stop();
                particleSystem.gameObject.SetActive(false);
            }
            index++;
        }
    }


    public void SpawnParticleEffect(Particles type, Vector3 location)
    {
        particleCollection.TryGetValue(type, out List<ParticleSystem> value);

        ParticleSystem currentParticle;
        if (value.Count >= 1)
        {
            currentParticle = value[0];
            currentParticle.transform.position = location;
            currentParticle.gameObject.SetActive(true);
        }
        else
        {

            particleTypes.TryGetValue(type, out ParticleSystem particaleSys);
            currentParticle = Instantiate(particaleSys, location, Quaternion.identity);
        }
            RemoveFromList(value, currentParticle);
    }

    public void RemoveFromList(List<ParticleSystem> list, ParticleSystem obj)
    {
        if (list.Contains(obj))
        {
            list.Remove(obj);
        }
        StartCoroutine(ParticleDelay(list, obj, obj.main.duration));
    }

    

    private IEnumerator ParticleDelay(List<ParticleSystem> list, ParticleSystem obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        list.Add(obj);
        obj.gameObject.SetActive(false);
    }

    
}
