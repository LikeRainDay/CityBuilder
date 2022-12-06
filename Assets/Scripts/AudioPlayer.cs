using System;
using UnityEngine;

namespace SVS
{
    public class AudioPlayer: MonoBehaviour
    {
        public AudioClip placementSound;
        public AudioSource audioSource;

        private static AudioPlayer _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void PlayPlacementSound()
        {
            if (placementSound != null)
            {
               audioSource.PlayOneShot(placementSound); 
            }
        }
    }
}