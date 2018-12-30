using UnityEngine;
using System.Collections;
using RPG.Core;

namespace RPG.Characters {
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour {
        enum State { idle, attacking, patrolling, chasing };

        [SerializeField] float chaseRadius = 10f;
       
        PlayerMovement player;
        Character character;

        State state = State.idle;
        float currentWeaponRange;
        float distanceToPlayer;

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
                state = State.patrolling;
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
