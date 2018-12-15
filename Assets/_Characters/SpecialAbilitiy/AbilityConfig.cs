using UnityEngine;
using RPG.Core;

namespace RPG.Characters {

    public struct AbilityParams{
        public IDamagable target;
        public float baseDamage;

        public AbilityParams(IDamagable target, float baseDamage){
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class AbilityConfig : ScriptableObject {
        [Header("Special Ability Section")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particleEffectPrefab;
        [SerializeField] AudioClip[] audioEffects = null;

        protected AbilityBehaviour behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityParams abilityParams){
            behaviour.Use(abilityParams);
        }

        public float GetEnergyCost(){
            return energyCost;
        }

        public GameObject GetParticleEffectPrefab() {
            return particleEffectPrefab;
        }

        public AudioClip GetRandomAudioEffect(){
            return audioEffects[Random.Range(0,audioEffects.Length)];
        }
    }
}