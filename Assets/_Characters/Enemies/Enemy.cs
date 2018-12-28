using UnityEngine;
using RPG.Core;

namespace RPG.Characters {

    public class Enemy : MonoBehaviour {

        [SerializeField] float attackRadius = 5f;
        [SerializeField] float chaseRadius = 10f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenAttack = 0.5f;
        [SerializeField] float secondsVariationBetweenAttack = 0.1f;
        [SerializeField] Vector3 aimOffSet = new Vector3(0f, 1f, 0f);

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;

        PlayerMovement player = null;

        bool isAttacking = false;

        private void Start() {
            player = FindObjectOfType<PlayerMovement>();
        }

        private void Update() {

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= attackRadius && !isAttacking) {
                isAttacking = true;
                float delayBetweenAttacks = Random.Range(secondsBetweenAttack-secondsVariationBetweenAttack,secondsBetweenAttack+secondsVariationBetweenAttack);
                InvokeRepeating("SpawnProjectile", 0f, delayBetweenAttacks);
            }

            if (distanceToPlayer > attackRadius) {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius) {
                //aiCharacterControl.SetTarget(player.transform);
            }
            else {
                //aiCharacterControl.SetTarget(null);
            }

        }

        private void SpawnProjectile() {


            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectile = newProjectile.GetComponent<Projectile>();
            projectile.SetShooter(gameObject);
            projectile.SetDamage(damagePerShot);

            Vector3 unitVectorPlayer = ((player.transform.position + aimOffSet) - projectileSocket.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorPlayer * projectile.GetDefaultProjectileSpeed();
        }

        private void OnDrawGizmos() {
            Gizmos.color = new Color(0, 0, 255, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);


            Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }

        public void TakeDamage(float changePoints) {
            //TODO Get rid of the IDamagable interface
        }
    }
}
