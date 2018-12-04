using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Weapons;
using RPG.Core;
using UnityEngine.SceneManagement;

namespace RPG.Characters {

    public class Player : MonoBehaviour, IDamagable {
        [SerializeField] float maximumHealthPoints = 100f;

        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse;

        [SerializeField] private SpecialAbility[] specialAbilities; //temp for debug

        float currentHealthPoints;
        float lastHitTime = 0f;
        CameraRaycaster cameraRaycaster;
        Animator animator;
        Energy energy;

        void Start() {
            SetCurrentMaxHealth();
            SetUpEnergyBar();
            RegisterForMouseClick();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            AddSpecialAbilitiesComponents();
        }

        private void AddSpecialAbilitiesComponents() {
            foreach(SpecialAbility config in specialAbilities)
                config.AttachComponentTo(gameObject); 
        }

        private void SetUpEnergyBar() {
            energy = GetComponent<Energy>();
        }

        public float healthAsPercentage {
            get {
                return currentHealthPoints / maximumHealthPoints;
            }
        }

        public void TakeDamage(float damage) {
            ReduceHealth(damage);

            bool playerDies = (currentHealthPoints - damage <= 0);
            if (playerDies){
                StartCoroutine(KillPlayer());
            }

        }

        IEnumerator KillPlayer(){
            yield return new WaitForSecondsRealtime(2f); //use audio clip lenght
            SceneManager.LoadScene(0);
        }

        private void ReduceHealth(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maximumHealthPoints);
        }

        private void SetCurrentMaxHealth() {
            currentHealthPoints = maximumHealthPoints;
        }

        private void SetupRuntimeAnimator() {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAnimationClip();
        }

        private void PutWeaponInHand() {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject weaponSocket = RequestDominantHand();
            GameObject weapon = Instantiate(weaponPrefab, weaponSocket.transform);
            weapon.transform.localPosition = weaponInUse.weaponTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.weaponTransform.localRotation;
        }

        private GameObject RequestDominantHand() {
            DominantHand[] dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand script found in Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand script found, please remove " + (numberOfDominantHands - 1));

            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick() {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemy) {

            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
                AttackTarget(enemy);
            else {
                if (Input.GetMouseButtonDown(1))
                    AttemptSpecialAbility(0, enemy);
            }
        }

        private void AttemptSpecialAbility(int abilityIndex, Enemy enemy) {
            float energyCost = specialAbilities[abilityIndex].GetEnergyCost();

            if (energy.IsEnergyAvailable(energyCost)) {
                energy.ConsumeEnergyPoints(energyCost);
                //use the ability
                AbilityParams abilityParams = new AbilityParams(enemy, baseDamage);
                specialAbilities[abilityIndex].Use(abilityParams);
            }
        }

        private void AttackTarget(Enemy enemy) {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits()) {
                animator.SetTrigger("Attack");
                (enemy as IDamagable).TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target) {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return (distanceToTarget <= weaponInUse.GetMaxAttackRange());
        }
    }
}
