﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {

    public abstract class AbilityBehaviour : MonoBehaviour {

        protected AbilityConfig config;

        const float PARTICLE_CLEAN_UP_DELAY = 10.0f;

        public void SetConfiguration(AbilityConfig configToSet) {
            config = configToSet;
        }

        public abstract void Use(AbilityParams abilityParams);

        protected void PlayParticleEffect() {
            GameObject particlePrefab = config.GetParticleEffectPrefab();
            GameObject effectPrefab = Instantiate(  //the particle effect configures the local or world coordinates
                particlePrefab, 
                transform.position,
                particlePrefab.transform.rotation);
            effectPrefab.transform.parent = transform;

            effectPrefab.GetComponent<ParticleSystem>().Play();
            Destroy(effectPrefab, PARTICLE_CLEAN_UP_DELAY);
        }

        protected void PlayAbilitySound(){
            var audioClip = config.GetRandomAudioEffect();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClip);
        }
    }
}