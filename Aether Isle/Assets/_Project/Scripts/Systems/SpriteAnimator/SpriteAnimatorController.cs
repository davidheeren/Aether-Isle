using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimatorController : MonoBehaviour
    {
        [SerializeField] float fps = 12;
        [SerializeField] bool playFirstAnimationOnAwake;
        [SerializeField] SpriteAnimation[] animations;

        private SpriteRenderer sr;
        private Sprite initialSprite;
        private Dictionary<string, SpriteAnimation> animationsDictionary = new Dictionary<string, SpriteAnimation>();

        public SpriteAnimation currentAnimation { get; private set; }
        public float currentAnimationTime { get; private set; }
        public float animationDelay { get; private set; }
        public bool isPaused { get; private set; }
        public bool isStopped { get; private set; }

        private int currentSpriteIndex;

        public event Action OnAnimationStart;
        public event Action OnAnimationDone;
        public event Action OnSpriteChanged;
        public event Action OnAnimationChanged;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            initialSprite = sr.sprite;
            animationDelay = 1 / fps;

            ProcessAnimations();
            SetupDictionary();
            SetupFirstAnimation();
        }

        private void ProcessAnimations()
        {
            List<SpriteAnimation> temp = new List<SpriteAnimation>();
            foreach (SpriteAnimation anim in animations)
            {
                if (anim != null)
                    temp.Add(anim);
            }

            if (temp.Count != animations.Length)
            {
                Debug.LogWarning("Some animations are null or duplicates: " + name);
                animations = temp.ToArray();
            }
        }

        private void SetupFirstAnimation()
        {
            if (animations.Length > 0)
            {
                if (playFirstAnimationOnAwake)
                    Play(animations[0]);
            }
        }

        void SetupDictionary()
        {
            foreach (SpriteAnimation anim in animations)
            {
                if (string.IsNullOrEmpty(anim.animationName))
                    continue;

                if (animationsDictionary.ContainsKey(anim.animationName))
                    Debug.LogError("Animator Controller contains multiple animations with the same name: " + anim.animationName);
                else
                    animationsDictionary[anim.animationName] = anim;
            }
        }

        void Update()
        {
            UpdateAnimation();
        }

        void UpdateAnimation()
        {
            if (currentAnimation == null) return;
            if (isPaused || isStopped) return;

            // Go to the next sprite
            if (currentAnimationTime >= animationDelay * (currentSpriteIndex + 1))
            {
                // Animation done
                if (currentSpriteIndex >= currentAnimation.Sprites.Length - 1)
                {
                    OnAnimationDone?.Invoke();

                    if (currentAnimation.loop)
                        Restart();
                    else
                        Stop();

                    return;
                }

                currentSpriteIndex++;

                sr.sprite = currentAnimation.Sprites[currentSpriteIndex];
                OnSpriteChanged?.Invoke();
            }

            // Tick animation if not paused
            currentAnimationTime += Time.deltaTime;
        }

        public void Play(SpriteAnimation animation)
        {
            if (animation == null) { Debug.LogWarning("Animation is null"); return; }
            if (animation.Sprites.Length == 0) { Debug.LogWarning("Animation has no sprites"); return; }

            if (currentAnimation != animation)
            {
                OnAnimationChanged?.Invoke();
                currentAnimation = animation;
            }

            Restart();
        }

        public void Play(string animationName) => Play(GetAnimation(animationName));

        public void Play(int index) => Play(GetAnimation(index));

        public void Restart()
        {
            currentAnimationTime = 0;
            currentSpriteIndex = 0;
            isPaused = false;
            isStopped = false;


            if (currentAnimation != null)
            {
                OnAnimationStart?.Invoke();

                sr.sprite = currentAnimation.Sprites[currentSpriteIndex];
                OnSpriteChanged?.Invoke();
            }
        }

        public void Stop() => Stop(currentAnimation.animationEnd);
        public void Stop(SpriteAnimation.AnimationEnd animationEnd)
        {
            isStopped = true;

            if (currentAnimation == null) return;

            // Handles the end of the animation
            // If lastSprite, we do nothing because it already is that sprite
            switch (animationEnd)
            {
                case SpriteAnimation.AnimationEnd.initialSprite:
                    sr.sprite = initialSprite;
                    break;
                case SpriteAnimation.AnimationEnd.firstSprite:
                    sr.sprite = currentAnimation.Sprites[0];
                    break;
                case SpriteAnimation.AnimationEnd.empty:
                    sr.sprite = null;
                    break;
            }
        }

        public void Pause() => isPaused = true;

        public void Resume() => isPaused = false;

        public SpriteAnimation GetAnimation(string animationName)
        {
            if (animationsDictionary.TryGetValue(animationName, out SpriteAnimation anim))
                return anim;

            Debug.LogWarning("Animation name not found: " + animationName);
            return null;
        }

        public SpriteAnimation GetAnimation(int index)
        {
            if (index >= 0 && index < animations.Length)
                return animations[index];

            Debug.LogWarning("Animation index out of range: " + index);
            return null;
        }

        public bool TryGetAnimation(string animationName, out SpriteAnimation anim)
        {
            return animationsDictionary.TryGetValue(animationName, out anim);
        }

        public bool TryGetAnimation(int index, out SpriteAnimation anim)
        {
            bool isValidIndex = index >= 0 && index < animations.Length;

            if (isValidIndex)
                anim = animations[index];
            else
                anim = null;

            return isValidIndex;
        }

        public float GetAnimationLengthSeconds(SpriteAnimation anim)
        {
            return anim.Length * animationDelay;
        }

        [ContextMenu("Set First Sprite")]
        private void SetFirstSprite()
        {
            if (TryGetAnimation(0, out SpriteAnimation anim))
            {
                if (anim.Length == 0) return;

                GetComponent<SpriteRenderer>().sprite = anim.Sprites[0];
            }
        }
    }
}
