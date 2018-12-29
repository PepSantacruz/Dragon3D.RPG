using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters {

    public class PlayerMovement : MonoBehaviour {
        WeaponSystem weaponSystem;

        Character character;
        SpecialAbilities specialAbilities;
        Enemy enemy;

        int currentAbilityIndex = 0;

        void Start() {
            SetUpCharacterAndSystems();
            SetUpEnergyBar();
            RegisterForMouseEvents();
        }

        void RegisterForMouseEvents() {
            CameraRaycaster cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverTerrain += OnMouseOverTerrain;
        }

        void SetUpCharacterAndSystems() {
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
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

        void OnMouseOverEnemy(Enemy enemyToSet) {
            this.enemy = enemyToSet;

            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
                weaponSystem.AttackTarget(enemyToSet.gameObject);
            else {
                if (Input.GetMouseButtonDown(1))
                    specialAbilities.AttemptSpecialAbility(currentAbilityIndex, enemy.gameObject);  //TODO always the last ability, exclude healing?
            }
        }

        void OnMouseOverTerrain(Vector3 destination) {
            if (Input.GetMouseButton(0))
                character.SetDestination(destination);
        }

        bool IsTargetInRange(GameObject target) {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return (distanceToTarget <= weaponSystem.GetWeaponConfig().GetMaxAttackRange());
        }
    }
}
