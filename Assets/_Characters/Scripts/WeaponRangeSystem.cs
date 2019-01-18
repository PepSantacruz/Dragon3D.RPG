using UnityEngine;

namespace RPG.Characters {
    public class WeaponRangeSystem : WeaponSystem {
    
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] GameObject spawnProjectilePoint;
        [SerializeField] float projectileSpeed;

        //TODO Oerride the coroutine DamageAfterDelay and undo doDamage
        protected override void DoDamage() {
            GameObject player = GameObject.FindWithTag("Player");

            GameObject newProjectile = Instantiate(projectilePrefab,spawnProjectilePoint.transform.position,Quaternion.identity);
            newProjectile.GetComponent<Projectile>().damageToDeal = CalculateDamage();

            Vector3 targetProjectile = player.transform.position + new Vector3(0, PARTICLE_Y_OFFSET, 0);

            Vector3 unitVectorToPlayer = (targetProjectile - newProjectile.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer*projectileSpeed;
        }

    }
}