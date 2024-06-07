using UnityEngine;
using Utilities;

public class SFXManager : Singleton<SFXManager>
{
    const float distFalloff = 10; // distance it starts to fall off
    AudioSource source;
    Transform camT;

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;

        camT = Camera.main.transform;
    }

    public void PlaySFXClip(AudioClip clip, Vector2 pos)
    {
        source.PlayOneShot(clip, GetVolumeFromPosition(pos));
    }

    public static float GetVolumeFromPosition(Vector2 pos)
    {
        float sqrDist = (pos - (Vector2)Instance.camT.position).sqrMagnitude;
        float distFactor = Mathf.Clamp01(distFalloff * distFalloff / sqrDist);
        if (distFactor < 0.1)
            distFactor = 0;
        return distFactor;
    }
}
