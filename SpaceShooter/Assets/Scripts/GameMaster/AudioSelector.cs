using UnityEngine;

public class AudioSelector : MonoBehaviour
{
    [SerializeField] private AudioController.ClipName clip;

    private void Start()
    {
        AudioController.Instance.GenerateAudio(clip, transform.position, 0.5f);
    }
}
