#nullable enable
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Assertions;

namespace Mochineko.LLMAgent.Speech
{
    public sealed class SpeechQueue : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        private readonly ConcurrentQueue<AudioClip> queue = new();

        private AudioClip? currentClip;

        private void Awake()
        {
            Assert.IsNotNull(audioSource);

            audioSource.loop = false;
        }

        private void OnDestroy()
        {
            if (currentClip != null)
            {
                Destroy(currentClip);
                currentClip = null;
            }
        }

        private void Update()
        {
            if (audioSource.isPlaying)
            {
                return;
            }
            
            if (queue.TryDequeue(out var audioClip))
            {
                if (currentClip != null)
                {
                    Destroy(currentClip);
                }

                currentClip = audioClip;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
        
        public void Enqueue(AudioClip audioClip)
        {
            queue.Enqueue(audioClip);
        }
    }
}