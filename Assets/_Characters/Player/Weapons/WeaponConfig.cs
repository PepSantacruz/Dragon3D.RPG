using UnityEngine;

namespace RPG.Characters {

    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject {

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] GameObject weaponHitParticlePrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] AnimationClip deathAnimation;
        [SerializeField] float timeBetweenAnimationCycles = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float damageDelay = .5f;

        public Transform weaponTransform;

        public float GetMinTimeBetweenAnimationCycles(){
            return timeBetweenAnimationCycles;
        }

        public float GetMaxAttackRange(){
            return maxAttackRange;
        }

        public AnimationClip GetAttackAnimationClip() {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        public AnimationClip GetDeathAnimationClip() {
            RemoveAnimationEvents();
            return deathAnimation;
        }

        //So that asset pack cannot cause crashes 
        private void RemoveAnimationEvents() {
            attackAnimation.events = new AnimationEvent[0];
            deathAnimation.events = new AnimationEvent[0];
        }

        public GameObject GetWeaponPrefab() {
            return weaponPrefab;
        }

        public float GetWeaponDamage() {
            return weaponDamage;
        }

        public GameObject GetWeaponHitParticlePrefab() {
            return weaponHitParticlePrefab;
        }

        public float GetDamageDelay() {
            return damageDelay;
        }
    }
}
