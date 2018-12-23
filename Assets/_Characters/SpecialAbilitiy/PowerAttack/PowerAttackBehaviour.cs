using UnityEngine;

namespace RPG.Characters {
    public class PowerAttackBehaviour : AbilityBehaviour {

        public override void Use(GameObject target) {
            PlayAbilitySound();
            DealDamageOnTarget(target);
            PlayParticleEffect();
        }

        private void DealDamageOnTarget(GameObject target) {
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }
    }
}
