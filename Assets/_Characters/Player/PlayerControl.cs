using UnityEngine;
using RPG.CameraUI;
using System.Collections;

namespace RPG.Characters {

    public class PlayerControl : MonoBehaviour {
        WeaponSystem weaponSystem;

        Character character;
        SpecialAbilities specialAbilities;

        int currentAbilityIndex = 0;

        //TODO only for debug purposes, make them local
        Vector3 P = Vector3.zero;
        float currentWeaponRange = 0f;

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
            currentWeaponRange = weaponSystem.GetWeaponConfig().GetMaxAttackRange();
            //Move to the target
            while (!IsTargetInRange(target)) {
                Vector3 A = transform.position;
                Vector3 B = target.transform.position;

                P = Vector3.Lerp(A, B, 1 - (currentWeaponRange * 0.7f) / (A - B).magnitude);

                character.SetDestination(P);
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

        private void OnDrawGizmos() {

            Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = new Color(0, 0, 255, 1);
            Gizmos.DrawSphere(P, 0.2f);
        }
    }
}
