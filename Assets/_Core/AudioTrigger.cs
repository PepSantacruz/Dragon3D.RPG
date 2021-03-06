﻿using UnityEngine;

namespace RPG.Core {
    public class AudioTrigger : MonoBehaviour {
        [SerializeField] AudioClip clip;
        [SerializeField] float distanceToPlayerThreshold = 5f;
        [SerializeField] bool isOneTimeOnly = true;

        bool hasPlayed = false;
        AudioSource audioSource;
        GameObject player;

        void Start() {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = clip;

            player = GameObject.FindWithTag(Constants.PLAYER_TAG);
        }

        void Update() {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < distanceToPlayerThreshold)
                RequestPlayAudioClip();
        }


        void RequestPlayAudioClip() {
            if (isOneTimeOnly && hasPlayed) {
                return;
            }
            else if (audioSource.isPlaying == false) {
                audioSource.Play();
                hasPlayed = true;
            }
        }

        void OnDrawGizmos() {
            Gizmos.color = new Color(0, 255f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, distanceToPlayerThreshold);
        }
    }
} 