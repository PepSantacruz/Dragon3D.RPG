using UnityEngine;

namespace RPG.Characters {

    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject {

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] GameObject weaponHitParticlePrefab;
        [SerializeField] AnimationClip[] attackAnimation;
        [SerializeField] AnimationClip deathAnimation;
        [SerializeField] float timeBetweenAnimationCycles = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float currentDamageDelay = .5f;

        //as an script object, the variables store in disk its values
        bool initialized = false;
        float[] damageDelayAttackAnimation;
        int currentDamageAttackAnimationIndex;


        public Transform weaponTransform;

        public void Initialize() {
            initialized = false;
            damageDelayAttackAnimation = new float[attackAnimation.Length];

            //TODO adjust the constant 0.1f
            for (int i = 0; i < attackAnimation.Length; i++) {
                float time = attackAnimation[i].events[0].time - 0.1f;
                damageDelayAttackAnimation[i] = time;
            }
        }

        public float GetMinTimeBetweenAnimationCycles() {
            return timeBetweenAnimationCycles;
        }

        public float GetMaxAttackRange() {
            return maxAttackRange;
        }

        public AnimationClip GetAttackAnimationClip() {
            currentDamageAttackAnimationIndex = Random.Range(0, attackAnimation.Length);

            RemoveAnimationEvents(attackAnimation[currentDamageAttackAnimationIndex]);
            return attackAnimation[currentDamageAttackAnimationIndex];
        }

        public AnimationClip GetDeathAnimationClip() {
            //RemoveAnimationEvents(deathAnimation);
            return deathAnimation;
        }

        //So that asset pack cannot cause crashes 
        private void RemoveAnimationEvents(AnimationClip animation) {
            animation.events = new AnimationEvent[0];
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
            return damageDelayAttackAnimation[currentDamageAttackAnimationIndex];
        }
    }
}
