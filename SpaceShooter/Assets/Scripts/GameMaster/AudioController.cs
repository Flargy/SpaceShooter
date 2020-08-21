
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController instance = null;

    public static AudioController Instance { get { return instance; } }

    public enum ClipName { Laser, Missile, Pickup, BossDestroy, PlayerDestroyed, EnemyDestroy }

    [SerializeField] private List<AudioClip> clips;
    [SerializeField] private AudioSource sourcePrefab;

    private List<AudioSource> sources;


    private void Awake()
    {
        sources = new List<AudioSource>();

        if(instance == null)
        {
            instance = this;
        }
    }

    public void GenerateAudio(ClipName type ,Vector3 location, float strength)
    {
        AudioSource audio;
        AudioClip clip = clips[(int)type];
        if(sources.Count > 0)
        {
            audio = sources[0];
            RemoveFromList(audio);
        }
        else
        {
            audio = Instantiate(sourcePrefab);
        }
        audio.gameObject.SetActive(true);
        audio.transform.position = location;
        audio.PlayOneShot(clip, strength);
    }

    private void RemoveFromList(AudioSource source)
    {
        if (sources.Contains(source))
        {
            sources.Remove(source);
        }
    }

    private void AddToList(AudioSource source)
    {
        source.gameObject.SetActive(false);
        sources.Add(source);
    }

    private IEnumerator AddToListAfterDelay(AudioSource source, AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        AddToList(source);
    }
}
