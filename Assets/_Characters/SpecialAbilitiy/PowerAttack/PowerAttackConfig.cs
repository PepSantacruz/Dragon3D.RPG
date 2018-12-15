using UnityEngine;

namespace RPG.Characters {

    [CreateAssetMenu(menuName = "RPG/Special Ability/Power Attack")]
    public class PowerAttackConfig : AbilityConfig {

        [Header("Specific Power Attack")]
        [SerializeField] float extraDamage = 10f;

        protected override AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo) {
            return (gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>());
        }

        public float GetExtraDamage(){
            return extraDamage;
        }
    }
}
