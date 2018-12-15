using UnityEngine;

namespace RPG.Characters {
    public class HealingEffectBehaviour : AbilityBehaviour {

        Player player = null;

        void Start() {
            player = GetComponent<Player>();
        }

        public override void Use(AbilityParams abilityParams) {
            PlayAbilitySound();
            HealCharacter();
            PlayParticleEffect();
        }

        private void HealCharacter() {
            player.Heal((config as HealingEffectConfig).GetHealingPoints());
        }
    }
}
