using UnityEngine;

namespace RPG.Characters {

    [CreateAssetMenu(menuName = "RPG/Special Ability/Healing Effect")]
    public class HealingEffectConfig : SpecialAbility {

        [Header("Specific Healing")]
        [SerializeField] float healingPoints = 10f;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo) {
            HealingEffectBehaviour behaviourComponent = gameObjectToAttachTo.AddComponent<HealingEffectBehaviour>();
            behaviourComponent.SetConfiguration(this);
            behaviour = behaviourComponent;
        }

        public float GetHealingPoints(){
            return healingPoints;
        }
    }
}
