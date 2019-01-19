using UnityEngine;
using RPG.Core;

namespace RPG.Characters {
    public class WeaponRangeSystem : WeaponSystem {
    
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] GameObject spawnProjectilePoint;
        [SerializeField] float projectileSpeed;
        
        protected override void DoDamage() {
            GameObject player = GameObject.FindWithTag(Constants.PLAYER_TAG);

            GameObject newProjectile = Instantiate(projectilePrefab,spawnProjectilePoint.transform.position,Quaternion.identity);
            Projectile projectile = newProjectile.GetComponent<Projectile>();

            projectile.damageToDeal = CalculateDamage();
            projectile.particleEffectPrefab = currentWeaponConfig.GetWeaponHitParticlePrefab();

            Vector3 targetProjectile = player.transform.position + new Vector3(0, Constants.PARTICLE_Y_OFFSET, 0);

            Vector3 unitVectorToPlayer = (targetProjectile - newProjectile.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer*projectileSpeed;
        }

    }
}