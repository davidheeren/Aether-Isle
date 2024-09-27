using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Game
{
    public class Test : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer.color = spriteRenderer.color.SetAlpha(0.5f);
        }
    }
}
