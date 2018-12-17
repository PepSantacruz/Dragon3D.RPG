using UnityEngine;
using RPG.Core;

namespace RPG.Characters {

    public class Enemy : MonoBehaviour, IDamagable {
        [SerializeField]
        float maximumHealthPoints = 100f;

        [SerializeField] float attackRadius = 5f;
        [SerializeField] float chaseRadius = 10f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenAttack = 0.5f;
        [SerializeField] float secondsVariationBetweenAttack = 0.1f;
        [SerializeField] Vector3 aimOffSet = new Vector3(0f, 1f, 0f);

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;

        Player player = null;
        AICharacterControl aiCharacterControl = null;

        float currentHealthPoints;
        bool isAttacking = false;

        private void Start() {
            player = FindObjectOfType<Player>();
            currentHealthPoints = maximumHealthPoints;
            aiCharacterControl = GetComponent<AICharacterControl>();
        }

        private void Update() {
            if (player.healthAsPercentage <= Mathf.Epsilon) {
                StopAllCoroutines();
                Destroy(this);  //To stop enemy behaviour
            }

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
                aiCharacterControl.SetTarget(player.transform);
            }
            else {
                aiCharacterControl.SetTarget(null);
            }

        }

        public float healthAsPercentage {
            get {
                return currentHealthPoints / maximumHealthPoints;
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

        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maximumHealthPoints);

            if (currentHealthPoints <= 0f) Destroy(gameObject);
        }

        private void OnDrawGizmos() {
            Gizmos.color = new Color(0, 0, 255, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);


            Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}
