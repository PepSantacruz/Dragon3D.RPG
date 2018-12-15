using UnityEngine;

namespace RPG.Characters {
    public class PowerAttackBehaviour : AbilityBehaviour {

        public override void Use(AbilityParams abilityParams) {
            PlayAbilitySound();
            DealDamageOnTarget(abilityParams);
            PlayParticleEffect();
        }

        private void DealDamageOnTarget(AbilityParams abilityParams) {
            float damageToDeal = abilityParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage();
            abilityParams.target.TakeDamage(damageToDeal);
        }
    }
}
