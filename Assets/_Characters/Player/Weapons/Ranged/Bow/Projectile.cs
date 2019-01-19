using RPG.Core;
using UnityEngine;

namespace RPG.Characters {
    public class Projectile : MonoBehaviour {
        public float damageToDeal  { get;  set;}


        void OnCollisionEnter(Collision collision) {
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();

            if (healthSystem)
                healthSystem.TakeDamage(damageToDeal);

            ParticleUtility.PlayParticleEffect(
                                    target.transform,
                                    currentWeaponConfig.GetWeaponHitParticlePrefab(),
                                    target.transform.position,
                                    ParticleUtility.PARTICLE_STD_Y_OFFSET
                            );
            Destroy(gameObject);
        }
    }
}
