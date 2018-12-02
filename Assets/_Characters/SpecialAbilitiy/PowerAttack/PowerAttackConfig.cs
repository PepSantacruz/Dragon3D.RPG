﻿using UnityEngine;

namespace RPG.Characters {

    [CreateAssetMenu(menuName = "RPG/Special Ability/Power Attack")]
    public class PowerAttackConfig : SpecialAbility {

        [Header("Specific Power Attack")]
        [SerializeField] float extraDamage = 10f;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo) {
            PowerAttackBehaviour behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.SetConfiguration(this);
            behaviour = behaviourComponent;
        }

        public float GetExtraDamage(){
            return extraDamage;
        }
    }
}
