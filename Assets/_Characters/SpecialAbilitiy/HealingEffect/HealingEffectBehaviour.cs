using UnityEngine;

namespace RPG.Characters {
    public class HealingEffectBehaviour : AbilityBehaviour {

        HealthSystem healthSystem = null;

        void Start() {
            healthSystem = GetComponent<HealthSystem>();
        }

        public override void Use(AbilityParams abilityParams) {
            PlayAbilitySound();
            HealCharacter();
            PlayParticleEffect();
        }

        private void HealCharacter() {
            healthSystem.Heal((config as HealingEffectConfig).GetHealingPoints());
        }
    }
}
