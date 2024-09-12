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

        public void Play() => source.Play();

        public void Stop() => source.Stop();

        public void Pause() => source.Pause();

        void Update()
        {
            if (source.isPlaying)
                source.volume = SFXManager.GetVolumeFromPosition(transform.position);
        }
    }
}
