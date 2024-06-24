using UnityEngine;

namespace Game
{
    public class SFXLoop : MonoBehaviour
    {
        [SerializeField] AudioClip SFX;

        AudioSource source;

        void Awake()
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = SFX;
            source.loop = true;
            source.playOnAwake = false;
        }

        public void Play()
        {
            source.Play();
            source.volume = SFXManager.GetVolumeFromPosition(transform.position);
        }

        public void Stop()
        {
            source.Stop();
        }

        void Update()
        {
            if (source.isPlaying)
            {
                source.volume = SFXManager.GetVolumeFromPosition(transform.position);
            }
        }
    }
}
