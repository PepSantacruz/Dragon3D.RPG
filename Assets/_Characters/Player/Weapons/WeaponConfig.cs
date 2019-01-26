using UnityEngine;
using RPG.Core;

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

        //as an script object, the variables store in disk its values
        float[] damageDelayAttackAnimation;
        int currentDamageAttackAnimationIndex;


        public Transform weaponTransform;

        public void Initialize() {
            damageDelayAttackAnimation = new float[attackAnimation.Length];

            for (int i = 0; i < attackAnimation.Length; i++) {
                if (attackAnimation[i].events.Length>0) {
                    float time = attackAnimation[i].events[0].time - Constants.ANIMATION_HIT_OFFSET;
                    damageDelayAttackAnimation[i] = time;
                }
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
