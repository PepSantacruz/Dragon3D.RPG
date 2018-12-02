using UnityEngine;

namespace RPG.Characters {
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility {

        PowerAttackConfig config;

        public void SetConfiguration(PowerAttackConfig configToSet){
            config = configToSet;
        }

        public void Use(AbilityParams abilityParams){
            float damageToDeal = abilityParams.baseDamage + config.GetExtraDamage();
            abilityParams.target.TakeDamage(damageToDeal);
        }
    }
}
