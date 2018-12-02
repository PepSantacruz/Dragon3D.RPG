using UnityEngine;

namespace RPG.Characters {
    public abstract class SpecialAbilityConfig : ScriptableObject {
        [Header("Special Ability Section")]
        [SerializeField] float energyCost = 10f;

        abstract public ISpecialAbility AddComponent(GameObject gameObjectToAttachTo);
    }
}