using UnityEngine;
using System.Collections;
using RPG.Core;
using System;

namespace RPG.Characters {

    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(WeaponSystem))]

    public class EnemyAI : MonoBehaviour {
        enum State { idle, attacking, patrolling, chasing };

        [SerializeField] float chaseRadius = 10f;
        [SerializeField] PatrollPathContainer patrollPathContainer;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] float dwellTime = 0.5f;

        PlayerControl player;
        Character character;
        WeaponSystem weaponSystem;

        State state = State.idle;
        float currentWeaponRange;
        float distanceToPlayer;
        int nextWaypointIndex = 0;
        private Vector3 P = new Vector3(0,0,0);

        private void Start() {
            player = FindObjectOfType<PlayerControl>(); //TODO Find player by tag?
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();

            currentWeaponRange = weaponSystem.GetWeaponConfig().GetMaxAttackRange();
        }

        private void Update() {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            currentWeaponRange = weaponSystem.GetWeaponConfig().GetMaxAttackRange();

            bool inWeaponCircle = distanceToPlayer <= currentWeaponRange; 
            bool inChaseRing = distanceToPlayer > currentWeaponRange      
                                 &&
                               distanceToPlayer <= chaseRadius;

            bool outsideChaseRing = distanceToPlayer > chaseRadius;   

            if (outsideChaseRing) {
                StopAttacking();
                character.setSelectedAnimatorForward();
                StartCoroutine(Patrol());
            }
            if (inChaseRing) {
                character.setMaximumAnimatorForward();
                StopAttacking();
                StartCoroutine(ChasePlayer());
            }
            if (inWeaponCircle) {
                StopAllCoroutines();
                state = State.attacking;
                weaponSystem.AttackTarget(player.gameObject);
            }
        }

        private void StopAttacking() {
            StopAllCoroutines();
            weaponSystem.StopAttacking();
        }

        IEnumerator Patrol() {
            state = State.patrolling;

            while (patrollPathContainer != null) {
                Vector3 nextWaypointPosition = patrollPathContainer.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPosition);
                CycleWaypointWhenClose(nextWaypointPosition);
                yield return new WaitForSecondsRealtime(dwellTime);
            }
            yield return new WaitForEndOfFrame();
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPosition) {
            float distanceToWayPoint = Vector3.Distance(nextWaypointPosition, transform.position);
            if (distanceToWayPoint <= waypointTolerance)
                nextWaypointIndex = (nextWaypointIndex + 1) % patrollPathContainer.transform.childCount;
        }

        IEnumerator ChasePlayer() {
            state = State.chasing;

            while (distanceToPlayer >= currentWeaponRange) {
                //TODO creat a common function with the player control
                Vector3 A = transform.position; 
                Vector3 B = player.transform.position;

                P = Vector3.Lerp(A, B, 1 -  (currentWeaponRange*0.7f)/ (A - B).magnitude);
               
                character.SetDestination(P);
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = new Color(0, 0, 255, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
            Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = new Color(0, 255, 0, 1);
            Gizmos.DrawSphere(P, 0.2f);
        }

    }
}
