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

        public void AttachAbilityTo(GameObject gameObjectToAttachTo) {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(gameObjectToAttachTo); 
            behaviourComponent.SetConfiguration(this);
            behaviour = behaviourComponent;
        }

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

        abstract protected AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo);

    }
}