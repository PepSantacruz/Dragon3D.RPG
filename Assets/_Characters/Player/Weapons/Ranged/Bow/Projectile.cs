
using UnityEngine;

namespace RPG.Characters {
    public class Projectile : MonoBehaviour {
        public float damageToDeal  { get;  set;}

        private void OnCollisionEnter(Collision collision) {
            print("Collision: " + collision.gameObject);
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();

            if (healthSystem)
                healthSystem.TakeDamage(damageToDeal);

            Destroy(gameObject);
        }
    }
}
