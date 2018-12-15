using UnityEngine;

namespace RPG.Characters {

    [CreateAssetMenu(menuName = "RPG/Special Ability/Area Attack")]
    public class AreaAttackConfig : AbilityConfig {

        [Header("Specific Area Attack")]
        [SerializeField] float radius = 3f;
        [SerializeField] float damageToEachTarget = 10f;

        protected override AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo) {
             return (gameObjectToAttachTo.AddComponent<AreaAttackBehaviour>());
        }

        public float GetRadius(){
            return radius;
        }

        public float GetDamageToEachTarget(){
            return damageToEachTarget;
        }


    }
}
