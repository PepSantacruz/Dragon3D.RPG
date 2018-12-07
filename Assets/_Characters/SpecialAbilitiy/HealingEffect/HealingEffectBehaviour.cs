using UnityEngine;
using RPG.Core;

namespace RPG.Characters {
    public class HealingEffectBehaviour : MonoBehaviour, ISpecialAbility {

        HealingEffectConfig config;

        public void SetConfiguration(HealingEffectConfig configToSet){
            config = configToSet;
        }

        public void Use(AbilityParams abilityParams){
            IDamagable damagable = gameObject.GetComponent<IDamagable>();
            damagable.TakeDamage(-config.GetHealingPoints());
        }
    }
}
