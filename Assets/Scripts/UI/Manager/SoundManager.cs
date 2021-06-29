using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
        private AudioSource _audioSource;
        public AudioClip[] clips;
        private static SoundManager instance = null;
        public static SoundManager Instance
        { get
                {
                        if (instance == null)
                                return null;

                        else return instance;
                }
        }

        private void Awake()
        {
                if (instance == null)
                {
                        instance = this;
                        DontDestroyOnLoad(this.gameObject);
                }
                else
                        Destroy(this.gameObject);

                _audioSource = GetComponent<AudioSource>();
        }
}

