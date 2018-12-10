using UnityEngine;

namespace RPG.Characters {

    [CreateAssetMenu(menuName = "RPG/Special Ability/Area Attack")]
    public class AreaAttackConfig : SpecialAbility {

        [Header("Specific Area Attack")]
        [SerializeField] float radius = 3f;
        [SerializeField] float damageToEachTarget = 10f;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo) {
            AreaAttackBehaviour behaviourComponent = gameObjectToAttachTo.AddComponent<AreaAttackBehaviour>();
            behaviourComponent.SetConfiguration(this);
            behaviour = behaviourComponent;
        }

        public float GetRadius(){
            return radius;
        }

        public float GetDamageToEachTarget(){
            return damageToEachTarget;
        }
    }
}
