using RPG.Core;
using UnityEngine;

namespace RPG.Characters {
    public class Projectile : MonoBehaviour {
        public float damageToDeal  { get;  set;}
        public GameObject particleEffectPrefab { get; set; }

        void OnCollisionEnter(Collision collision) {
            GameObject target = collision.gameObject;
            HealthSystem healthSystem = target.GetComponent<HealthSystem>();
            SlowMotion slowMotion = Camera.main.GetComponent<SlowMotion>();

            if (healthSystem) {
                healthSystem.TakeDamage(damageToDeal);
                slowMotion.SlowTime();
                ParticleUtility.PlayParticleEffect(
                                    target.transform,
                                    particleEffectPrefab,
                                    target.transform.position,
                                    ParticleUtility.PARTICLE_STD_Y_OFFSET
                            );
            }


            Destroy(gameObject);
        }
    }
}
