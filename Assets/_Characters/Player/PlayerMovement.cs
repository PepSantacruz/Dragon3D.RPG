using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;

namespace RPG.Characters {

    public class PlayerMovement : MonoBehaviour {
        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        [SerializeField] float baseDamage = 10f;
        [Range(0.1f, 1)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float crititicalHitMultiplier = .25f;
        [SerializeField] GameObject criticalParticleEffectPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Weapon currentWeaponConfig = null;

        CameraRaycaster cameraRaycaster = null;
        Animator animator;
        SpecialAbilities specialAbilities;
        Enemy enemy;
        GameObject weaponGameObject;
        HealthSystem healthSystem;
        Character character;

        int currentAbilityIndex = 0;
        float lastHitTime = 0f;

        void Start() {
            SetUpCharacterAndHealthSystem();
            SetUpEnergyBar();

            RegisterForMouseEvents();
            PutWeaponInHand(currentWeaponConfig);
            SetupAttackAndDeathAnimation();
        }

        void RegisterForMouseEvents() {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverTerrain += OnMouseOverTerrain;
        }

        void SetUpCharacterAndHealthSystem() {
            character = GetComponent<Character>();
            healthSystem = GetComponent<HealthSystem>();
        }

        void Update() {
            ScanForAbilityKeyDown();
        }

        void ScanForAbilityKeyDown() {
            for (int key = 1; key < specialAbilities.getNumberOfAbilities(); key++)
                if (Input.GetKeyDown(key.ToString())) {
                    specialAbilities.AttemptSpecialAbility(key);
                    currentAbilityIndex = key;
                }
        }

        void SetUpEnergyBar() {
            specialAbilities = GetComponent<SpecialAbilities>();
        }

        void SetupAttackAndDeathAnimation() {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimationClip();
            animatorOverrideController[healthSystem.DEFAULT_DEATH] = currentWeaponConfig.GetDeathAnimationClip();
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

        GameObject RequestDominantHand() {
            DominantHand[] dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand script found in Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand script found, please remove " + (numberOfDominantHands - 1));

            return dominantHands[0].gameObject;
        }

        void OnMouseOverEnemy(Enemy enemyToSet) {
            enemy = enemyToSet;

            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
                AttackTarget();
            else {
                if (Input.GetMouseButtonDown(1))
                    specialAbilities.AttemptSpecialAbility(currentAbilityIndex, enemy.gameObject);  //TODO always the last ability, exclude healing?
            }
        }

        void OnMouseOverTerrain(Vector3 destination) {
            if (Input.GetMouseButton(0))
                character.SetDestination(destination);
        }

        void AttackTarget() {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits()) {
                SetupAttackAndDeathAnimation();  //update animation attack and death
                animator.SetTrigger(ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        float CalculateDamage() {
            float damage = baseDamage + currentWeaponConfig.GetWeaponDamage();

            if (UnityEngine.Random.Range(0, 1f) <= criticalHitChance) {
                damage *= crititicalHitMultiplier;
                InstantiateCriticalParticleEffect();
            }

            return damage;
        }

        void InstantiateCriticalParticleEffect() {
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
