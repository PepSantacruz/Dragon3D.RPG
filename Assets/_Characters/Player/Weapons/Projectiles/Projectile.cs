using UnityEngine;
using RPG.Core;

namespace RPG.Characters {

    public class Projectile : MonoBehaviour {

        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter;

        float damageCaused;

        public void SetDamage(float damage) {
            damageCaused = damage;
        }

        public void SetShooter(GameObject shooter) {
            this.shooter = shooter;
        }

        public float GetDefaultProjectileSpeed() {
            return projectileSpeed;
        }

        private void OnCollisionEnter(Collision collision) {
            if (shooter && collision.gameObject.layer != shooter.layer) {
                DamageIfDamagable(collision);
            }

            Destroy(gameObject); //Ben put this line inside DamagaIfDamagable, could destroy an already destroyed gO
        }

        private void DamageIfDamagable(Collision collision) {
            HealthSystem damagableComponent = collision.gameObject.GetComponent<HealthSystem>();

            if (damagableComponent) {
                damagableComponent.TakeDamage(damageCaused);
            }
        }
    }
}
