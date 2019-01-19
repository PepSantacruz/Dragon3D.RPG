using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.Core;

namespace RPG.Characters {
    public class WeaponSystem : MonoBehaviour {

        //TODO Remove, it's in UtilityPrefab
        const float PARTICLE_CLEAN_UP_DELAY = 5.0f;
        protected float PARTICLE_Y_OFFSET = 1.2f;

        [SerializeField] float baseDamage = 10f;
        [SerializeField] protected WeaponConfig currentWeaponConfig = null;

        protected Character character;
        Animator animator;
        GameObject target;
        GameObject weaponGameObject;

        float lastHitTime;

        void Start() {
            SetUpReferencesToComponents();
            PutWeaponInHand(currentWeaponConfig);
            SetupAttackAndDeathAnimation();
        }

        void Update() {
            bool targetIsDead;
            bool targetOutOfRange;

            if (target == null) {
                targetIsDead = false;
                targetOutOfRange = false;
            }
            else {
                float targetHealth = target.GetComponent<HealthSystem>().healthAsPercentage;
                targetIsDead = targetHealth <= Mathf.Epsilon;

                float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
                targetOutOfRange = distanceToTarget > currentWeaponConfig.GetMaxAttackRange();
            }

            float characterHealth = character.GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = characterHealth <= Mathf.Epsilon;

            if (characterIsDead || targetOutOfRange || targetIsDead) {
                StopAllCoroutines();
            }
        }

        public void StopAttacking() {
            animator.StopPlayback();
            StopAllCoroutines();
        }

        void SetUpReferencesToComponents() {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
        }

        void SetupAttackAndDeathAnimation() {

            if (!character.GetAnimatorOverrideController()) {
                Debug.Break();
                Debug.LogAssertion("Provide " +gameObject+" with an animation Override Controller");
            }
            else {
                AnimatorOverrideController animatorOverrideController = character.GetAnimatorOverrideController();
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController[Constants.DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimationClip();
                animatorOverrideController[Constants.DEFAULT_DEATH] = currentWeaponConfig.GetDeathAnimationClip();
            }
        }

        GameObject RequestDominantHand() {
            DominantHand[] dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand script found in "+gameObject+", please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand script found in "+gameObject+", please remove " + (numberOfDominantHands - 1));

            return dominantHands[0].gameObject;
        }

        protected float CalculateDamage() {
            return baseDamage + currentWeaponConfig.GetWeaponDamage();
        }

        public void AttackTarget(GameObject targetToAttack) {
            target = targetToAttack;
            StartCoroutine(AttackTargetRepeatedly());
        }

        IEnumerator AttackTargetRepeatedly() {
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive) {
                float animationClipTime = currentWeaponConfig.GetDeathAnimationClip().length;
                animationClipTime /= character.GetAnimationSpeedMultiplier();
                float timeToWait = animationClipTime + currentWeaponConfig.GetMinTimeBetweenAnimationCycles();

                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                if (isTimeToHitAgain) {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }

                yield return new WaitForSeconds(timeToWait);
            }
        }

        //TODO make abilitybehaviour call this method instead of doing it itself?
        void AttackTargetOnce() {
            transform.LookAt(target.transform);
            animator.SetTrigger(Constants.ATTACK_TRIGGER);
            SetupAttackAndDeathAnimation();
            float damageDelay = currentWeaponConfig.GetDamageDelay(); //to know exactly when in the animation we're gona hit
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float damageAfterDelay) {
            yield return new WaitForSecondsRealtime(damageAfterDelay);
            DoDamage();
        }

        protected virtual void DoDamage() {
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
            PlayParticleEffect();
        }

        private void PlayParticleEffect() {
            ParticleUtility.PlayParticleEffect(
                                    target.transform,
                                    currentWeaponConfig.GetWeaponHitParticlePrefab(),
                                    target.transform.position,
                                    ParticleUtility.PARTICLE_STD_Y_OFFSET
                            );
        }

        public void PutWeaponInHand(WeaponConfig weaponConfig) {
            currentWeaponConfig = weaponConfig;
            var weaponPrefab = weaponConfig.GetWeaponPrefab();
            GameObject weaponSocket = RequestDominantHand();
            if (weaponGameObject != null)
                Destroy(weaponGameObject);
            weaponGameObject = Instantiate(weaponPrefab, weaponSocket.transform);
            weaponGameObject.transform.localPosition = currentWeaponConfig.weaponTransform.localPosition;
            weaponGameObject.transform.localRotation = currentWeaponConfig.weaponTransform.localRotation;
        }

        public WeaponConfig GetWeaponConfig() {
            return currentWeaponConfig;
        }
    }
}