
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
    private float laserAudioDelay;
    private float missileAudioDelay;


    private void Awake()
    {
        sources = new List<AudioSource>();

        if(instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        laserAudioDelay += GameVariables.GameTime;
        missileAudioDelay += GameVariables.GameTime;
    }

    public void GenerateAudio(ClipName type ,Vector3 location, float strength)
    {
        if (strength == 0)
        {
                if ((type == ClipName.Laser) && laserAudioDelay < 0.1f)
            {
                return;
            }
            if (type == ClipName.Missile && missileAudioDelay < 0.1f)
            {
                return;
            }
        
                return;
        }
        laserAudioDelay = laserAudioDelay > 0.1f ? 0.0f : laserAudioDelay;
        AudioSource audio;
        AudioClip clip = clips[(int)type];
        if(sources.Count > 0)
        {
            audio = sources[0];
        }
        else
        {
            audio = Instantiate(sourcePrefab);
        }
        RemoveFromList(audio, clip.length);
        audio.gameObject.SetActive(true);
        audio.transform.position = location;
        audio.PlayOneShot(clip, strength);
    }

    private void RemoveFromList(AudioSource source, float duration)
    {
        if (sources.Contains(source))
        {
            sources.Remove(source);
        }
        StartCoroutine(AddToListAfterDelay(source, duration));
    }

    private void AddToList(AudioSource source)
    {
        source.gameObject.SetActive(false);
        sources.Add(source);
    }

    private IEnumerator AddToListAfterDelay(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        AddToList(source);
    }
}
