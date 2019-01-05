using UnityEngine;
using RPG.CameraUI;
using System.Collections;

namespace RPG.Characters {

    public class PlayerControl : MonoBehaviour {
        WeaponSystem weaponSystem;

        Character character;
        SpecialAbilities specialAbilities;

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

        void OnMouseOverTerrain(Vector3 destination) {
            if (Input.GetMouseButton(0)) {
                weaponSystem.StopAttacking();
                character.SetDestination(destination);
            }
        }

        bool IsTargetInRange(EnemyAI target) {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return (distanceToTarget <= weaponSystem.GetWeaponConfig().GetMaxAttackRange());
        }

        void OnMouseOverEnemy(EnemyAI enemy) {

            if (Input.GetMouseButton(0) && IsTargetInRange(enemy)) {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy)) {
                StartCoroutine(MoveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy)) {
                specialAbilities.AttemptSpecialAbility(currentAbilityIndex, enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy)) {
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        IEnumerator MoveToTarget(EnemyAI target) {

            //Move to the target
            while (!IsTargetInRange(target)) {
                character.SetDestination(target.transform.position);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndAttack(EnemyAI target) {
            yield return StartCoroutine(MoveToTarget(target));
            weaponSystem.AttackTarget(target.gameObject);
        }

        IEnumerator MoveAndPowerAttack(EnemyAI target) {
            yield return StartCoroutine(MoveToTarget(target));
            specialAbilities.AttemptSpecialAbility(currentAbilityIndex, target.gameObject);
        }
    }
}
