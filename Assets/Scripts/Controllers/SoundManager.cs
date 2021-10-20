using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip rewindSound;
    [SerializeField] private AudioClip shotSound;
    
    private void OnEnable()
    {
        GameManager.RewindStarted += PlayRewindSound;
        InputManager.OnRelease    += PlayShotSound;
    }

    private void OnDisable()
    {
        GameManager.RewindStarted -= PlayRewindSound;
        InputManager.OnRelease    -= PlayShotSound;
    }

    private void PlayRewindSound()
    {
        AudioSource.PlayClipAtPoint(rewindSound, Vector3.zero);
    }
    
    private void PlayShotSound()
    {
        AudioSource.PlayClipAtPoint(shotSound, Vector3.zero);
    }
}
