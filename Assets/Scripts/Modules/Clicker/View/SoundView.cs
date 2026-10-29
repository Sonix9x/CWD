using UnityEngine;

namespace CWD.Scripts.Modules.Clicker.View
{
    public class SoundView : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip _audioClip;

        public void PlayAudio()
        {
            _audio.PlayOneShot(_audioClip);
        }
    }
}
