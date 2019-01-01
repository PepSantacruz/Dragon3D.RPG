﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters {
    public class WeaponSystem : MonoBehaviour {


        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig = null;

        Animator animator;
        GameObject target;
        GameObject weaponGameObject;
        Character character;

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
                targetOutOfRange = distanceToTarget <= Mathf.Epsilon;
            }

            float characterHealth = character.GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = characterHealth <= Mathf.Epsilon;

            if (characterIsDead || targetOutOfRange || targetIsDead) {
                StopAllCoroutines();
            }
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
                animatorOverrideController[AnimationConstants.DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimationClip();
                animatorOverrideController[AnimationConstants.DEFAULT_DEATH] = currentWeaponConfig.GetDeathAnimationClip();
            }
        }

        GameObject RequestDominantHand() {
            DominantHand[] dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand script found in Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand script found, please remove " + (numberOfDominantHands - 1));

            return dominantHands[0].gameObject;
        }

        float CalculateDamage() {
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
                float weaponHitPeriod = currentWeaponConfig.GetMinTimeBetweenHits();
                float timeToWait = weaponHitPeriod * character.GetAnimationSpeedMultiplier();

                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                if (isTimeToHitAgain) {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }

                yield return new WaitForSeconds(timeToWait);
            }
        }

        void AttackTargetOnce() {
            transform.LookAt(target.transform);
            animator.SetTrigger(AnimationConstants.ATTACK_TRIGGER);
            float damageDelay = 1f; //to know exactly when in the animation we're gona hit
            SetupAttackAndDeathAnimation();
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float damageAfterDealay) {
            yield return new WaitForSecondsRealtime(damageAfterDealay);
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
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