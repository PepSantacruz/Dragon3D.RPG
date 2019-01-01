using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {
    public class AreaAttackBehaviour : AbilityBehaviour {

        public override void Use(GameObject target) {
            PlayAbilitySound();
            DealRadialDamage();
            PlayParticleEffect();
            PlayAbilityAnimation();
        }

        private void DealRadialDamage() {
            float radius = (config as AreaAttackConfig).GetRadius();
            RaycastHit[] raycastHits = Physics.SphereCastAll(
                            transform.position, radius, Vector3.up, radius
                        );

            foreach (RaycastHit hit in raycastHits) {
                if (hit.collider.gameObject != gameObject) {
                    HealthSystem damagable = hit.collider.gameObject.GetComponent<HealthSystem>();
                    if (damagable != null) {
                        float damageToDeal = (config as AreaAttackConfig).GetDamageToEachTarget();
                        damagable.TakeDamage(damageToDeal);
                    }
                }
            }
        }
    }
}
