using UnityEngine;
using System.Collections;
using RPG.Core;
using System;

namespace RPG.Characters {
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour {
        enum State { idle, attacking, patrolling, chasing };

        [SerializeField] float chaseRadius = 10f;
        [SerializeField] PatrollPathContainer patrollPathContainer;
        [SerializeField] float waypointTolerance = 2f;

        PlayerMovement player;
        Character character;

        State state = State.idle;
        float currentWeaponRange;
        float distanceToPlayer;
        int nextWaypointIndex = 0;

        private void Start() {
            player = FindObjectOfType<PlayerMovement>(); //TODO Find player by tag?
            character = GetComponent<Character>();

            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetWeaponConfig().GetMaxAttackRange();
        }

        private void Update() {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer > chaseRadius && state != State.patrolling) {
                StopAllCoroutines();
                StartCoroutine(Patroll());
            }

            if (distanceToPlayer <= chaseRadius && state != State.chasing) {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }

            if (distanceToPlayer <= currentWeaponRange && state != State.attacking) {
                StopAllCoroutines();
                state = State.attacking;
            }
        }

        IEnumerator Patroll() {
            state = State.patrolling;

            while (true) {
                Vector3 nextWaypointPosition = patrollPathContainer.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPosition);
                CycleWaypointWhenClose(nextWaypointPosition);
                yield return new WaitForSeconds(.5f);
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPosition) {
            float distanceToWayPoint = Vector3.Distance(nextWaypointPosition, transform.position);
            if (distanceToWayPoint <= waypointTolerance)
                nextWaypointIndex = (nextWaypointIndex + 1) % patrollPathContainer.transform.childCount;
        }

        IEnumerator ChasePlayer() {
            state = State.chasing;

            while (distanceToPlayer >= currentWeaponRange) {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }

        }

        private void OnDrawGizmos() {
            Gizmos.color = new Color(0, 0, 255, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
            Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);
        }

    }
}
