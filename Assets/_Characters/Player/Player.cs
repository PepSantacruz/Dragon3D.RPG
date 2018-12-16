using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Weapons;
using RPG.Core;
using UnityEngine.SceneManagement;
using System;

namespace RPG.Characters {

    public class Player : MonoBehaviour, IDamagable {
        const string ATTACK_TRIGGER = "Attack";
        const string DEATH_TRIGGER = "Death";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        const string DEFAULT_DEATH = "DEFAULT DEATH";

        [SerializeField] float maximumHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;
        [Range(0.1f, 1)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float crititicalHitMultiplier = .25f;
        [SerializeField] GameObject criticalParticleEffectPrefab = null;

        [SerializeField] AnimatorOverrideController animatorOverrideController;

        [SerializeField] Weapon currentWeaponConfig = null;

        [SerializeField] private AbilityConfig[] specialAbilities = null;

        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;


        float currentHealthPoints;
        float lastHitTime = 0f;
        bool isCharacterDead = false;

        CameraRaycaster cameraRaycaster = null;
        Animator animator = null;
        Energy energy = null;
        AudioSource audioSource = null;
        Enemy enemy = null;
        int currentAbilityIndex = 0;
        GameObject weaponGameObject;

        public delegate void OnPlayerDeath();
        public event OnPlayerDeath onPlayerDeath;

        void Start() {
            SetUpAudioSource();

            SetCurrentMaxHealth();
            SetUpEnergyBar();
            RegisterForMouseClick();
            PutWeaponInHand(currentWeaponConfig);
            SetupAttackAndDeathAnimation();
            AddSpecialAbilitiesComponents();
        }

        void Update() {
            if (currentHealthPoints > Mathf.Epsilon)
                ScanForAbilityKeyDown();
        }

        private void ScanForAbilityKeyDown() {
            for (int key = 1; key < specialAbilities.Length; key++)
                if (Input.GetKeyDown(key.ToString())) {
                    AttemptSpecialAbility(key);
                    currentAbilityIndex = key;
                }
        }

        private void SetUpAudioSource() {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        private void AddSpecialAbilitiesComponents() {
            foreach(AbilityConfig config in specialAbilities)
                config.AttachAbilityTo(gameObject); 
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
            if (GetComponent<Rigidbody>().isKinematic == false) { //if the player is not already dead TODO necessary? another way?

                currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maximumHealthPoints);
                if (currentHealthPoints <= 0) {
                    GetComponent<Rigidbody>().isKinematic = true; //to prevent player collision when he is dead TODO error new unity version
                    
                    StartCoroutine(KillPlayer());
                }
                else {
                    PlayAudioHit();
                }
            }
        }

        public void Heal(float healthToHeal) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + healthToHeal, 0f, maximumHealthPoints);
        }

        private void PlayAudioHit() {
            if (audioSource.isPlaying == false) {
                audioSource.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
                audioSource.Play();
            }
        }

        IEnumerator KillPlayer(){
            onPlayerDeath(); //notify the observers

            animator.SetTrigger(DEATH_TRIGGER); 

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length); //use audio clip lenght

            SceneManager.LoadScene(0);
        }

        private void SetCurrentMaxHealth() {
            currentHealthPoints = maximumHealthPoints;
        }

        private void SetupAttackAndDeathAnimation() {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimationClip();
            animatorOverrideController[DEFAULT_DEATH] = currentWeaponConfig.GetDeathAnimationClip();
        }

        public void PutWeaponInHand(Weapon weaponConfig) {
            currentWeaponConfig = weaponConfig;
            var weaponPrefab = weaponConfig.GetWeaponPrefab();
            GameObject weaponSocket = RequestDominantHand();
            Destroy(weaponGameObject);
            weaponGameObject = Instantiate(weaponPrefab, weaponSocket.transform);
            weaponGameObject.transform.localPosition = currentWeaponConfig.weaponTransform.localPosition;
            weaponGameObject.transform.localRotation = currentWeaponConfig.weaponTransform.localRotation;
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

        void OnMouseOverEnemy(Enemy enemyToSet) {
            enemy = enemyToSet;

            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
                AttackTarget();
            else {
                if (Input.GetMouseButtonDown(1))
                    AttemptSpecialAbility(currentAbilityIndex);  //TODO always the last ability, exclude healing?
            }
        }

        private void AttemptSpecialAbility(int abilityIndex) {
            float energyCost = specialAbilities[abilityIndex].GetEnergyCost();

            if (energy.IsEnergyAvailable(energyCost)) {
                energy.ConsumeEnergyPoints(energyCost);
                //use the ability
                AbilityParams abilityParams = new AbilityParams(enemy, baseDamage);
                specialAbilities[abilityIndex].Use(abilityParams);
            }
        }

        private void AttackTarget() {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits()) {
                SetupAttackAndDeathAnimation();  //update animation attack and death
                animator.SetTrigger(ATTACK_TRIGGER);
                (enemy as IDamagable).TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage() {
            float damage = baseDamage + currentWeaponConfig.GetWeaponDamage();

            if (UnityEngine.Random.Range(0, 1f) <= criticalHitChance) {
                damage *= crititicalHitMultiplier;
                InstantiateCriticalParticleEffect();
            }

            return damage;
        }

        private void InstantiateCriticalParticleEffect() {
                GameObject effectPrefab = Instantiate(criticalParticleEffectPrefab, transform.position, Quaternion.identity);
                effectPrefab.transform.parent = transform; //attach effect to the player
                ParticleSystem myParticleSystem = effectPrefab.GetComponent<ParticleSystem>();
                myParticleSystem.Play();
                Destroy(effectPrefab, myParticleSystem.main.duration);

        }

        private bool IsTargetInRange(GameObject target) {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return (distanceToTarget <= currentWeaponConfig.GetMaxAttackRange());
        }
    }
}
