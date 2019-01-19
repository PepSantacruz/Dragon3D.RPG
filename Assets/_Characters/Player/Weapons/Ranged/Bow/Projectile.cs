using RPG.Core;
using UnityEngine;

namespace RPG.Characters {
    public class Projectile : MonoBehaviour {
        public float damageToDeal  { get;  set;}
        public GameObject particleEffectPrefab { get; set; }

        void OnCollisionEnter(Collision collision) {
            GameObject target = collision.gameObject;
            HealthSystem healthSystem = target.GetComponent<HealthSystem>();

            if (healthSystem)
                healthSystem.TakeDamage(damageToDeal);

            ParticleUtility.PlayParticleEffect(
                                    target.transform,
                                    particleEffectPrefab,
                                    target.transform.position,
                                    ParticleUtility.PARTICLE_STD_Y_OFFSET
                            );
            Destroy(gameObject);
        }
    }
}
