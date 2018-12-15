using UnityEngine;

namespace RPG.Characters {

    [CreateAssetMenu(menuName = "RPG/Special Ability/Healing Effect")]
    public class HealingEffectConfig : AbilityConfig {

        [Header("Specific Healing")]
        [SerializeField] float healingPoints = 10f;

        protected override AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo) {
            return (gameObjectToAttachTo.AddComponent<HealingEffectBehaviour>());
        }

        public float GetHealingPoints(){
            return healingPoints;
        }
    }
}
