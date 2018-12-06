using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Weapons;
using RPG.Core;
using UnityEngine.SceneManagement;

namespace RPG.Characters {

    public class Player : MonoBehaviour, IDamagable {
        const string ATTACK_TRIGGER = "Attack";
        const string DEATH_TRIGGER = "Death";

        [SerializeField] float maximumHealthPoints = 100f;

        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse;

        [SerializeField] private SpecialAbility[] specialAbilities;

        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        float currentHealthPoints;
        float lastHitTime = 0f;

        CameraRaycaster cameraRaycaster;
        Animator animator;
        Energy energy;
        AudioSource audioSource;

        void Start() {
            SetUpAudioSource();
            SetCurrentMaxHealth();
            SetUpEnergyBar();
            RegisterForMouseClick();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            AddSpecialAbilitiesComponents();
        }

        private void SetUpAudioSource() {
            audioSource = gameObject.AddComponent<AudioSource>();
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

            if (currentHealthPoints<=0) {
                StartCoroutine(KillPlayer());
            }
            else {
                PlayAudioHit();
            }
        }

        private void PlayAudioHit() {
            if (audioSource.isPlaying == false) {
                audioSource.clip = damageSounds[Random.Range(0, damageSounds.Length)];
                audioSource.Play();
            }
        }

        IEnumerator KillPlayer(){
            animator.SetTrigger(DEATH_TRIGGER); 

            audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length); //use audio clip lenght

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
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimationClip();
            animatorOverrideController["DEFAULT DEATH"] = weaponInUse.GetDeathAnimationClip();
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
                animator.SetTrigger(ATTACK_TRIGGER);
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
