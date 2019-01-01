using UnityEngine;

namespace RPG.Characters {
    public class HealingEffectBehaviour : AbilityBehaviour {

        HealthSystem healthSystem = null;

        void Start() {
            healthSystem = GetComponent<HealthSystem>();
        }

        public override void Use(GameObject gameObject) {
            PlayAbilitySound();
            HealCharacter();
            PlayParticleEffect();
            PlayAbilityAnimation();
        }

        private void HealCharacter() {
            healthSystem.Heal((config as HealingEffectConfig).GetHealingPoints());
        }
    }
}
