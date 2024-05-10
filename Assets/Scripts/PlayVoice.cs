using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayVoice : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _offsetDuration = 1f;
    public IEnumerator Play()
    {
        if(_audioSource != null)
        {
            _audioSource.Play();
            yield return new WaitForSeconds(_audioSource.clip.length + _offsetDuration);
        }
        else
        {
            Debug.LogError("NO AUDIO BLYAD");
            yield return new WaitForSeconds(_offsetDuration);
        }
    }
}
