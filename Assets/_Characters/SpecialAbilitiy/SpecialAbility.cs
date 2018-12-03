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

    public abstract class SpecialAbility : ScriptableObject {
        [Header("Special Ability Section")]
        [SerializeField] float energyCost = 10f;

        protected ISpecialAbility behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityParams abilityParams){
            behaviour.Use(abilityParams);
        }

        public float GetEnergyCost(){
            return energyCost;
        }
    }

    public interface ISpecialAbility {
        void Use(AbilityParams abilityParams);
    }
}