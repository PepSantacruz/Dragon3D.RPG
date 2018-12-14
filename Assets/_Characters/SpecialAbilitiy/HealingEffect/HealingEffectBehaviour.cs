using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {
    public class HealingEffectBehaviour : MonoBehaviour, ISpecialAbility {

        HealingEffectConfig config = null;
        Player player = null;
        AudioSource audioSource = null;

        public void SetConfiguration(HealingEffectConfig configToSet){
            config = configToSet;
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityParams abilityParams) {
            HealCharacter();
            InstantiateParticleEffect(transform.position,Quaternion.identity);
            PlaySound(); //TODO move to parent class with particle effect
        }

        private void PlaySound() {
            audioSource.clip = config.GetAudioEffect();
            audioSource.Play();
        }

        private void HealCharacter() {
            player.Heal(config.GetHealingPoints());
        }

        private void InstantiateParticleEffect(Vector3 position, Quaternion quaternion) {
            GameObject effectPrefab = Instantiate(config.GetParticleEffectPrefab(), position, quaternion);
            effectPrefab.transform.parent = transform; //attach effect to the player
            ParticleSystem myParticleSystem = effectPrefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(effectPrefab, myParticleSystem.main.duration);
        }
    }
}
