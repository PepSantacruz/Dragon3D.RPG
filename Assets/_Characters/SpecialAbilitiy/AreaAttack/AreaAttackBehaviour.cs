using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {
    public class AreaAttackBehaviour : AbilityBehaviour {

        public override void Use(AbilityParams abilityParams) {
            PlayAbilitySound();
            DealRadialDamage(abilityParams);
            PlayParticleEffect();
        }

        private void DealRadialDamage(AbilityParams abilityParams) {
            float radius = (config as AreaAttackConfig).GetRadius();
            RaycastHit[] raycastHits = Physics.SphereCastAll(
                            transform.position, radius, Vector3.up, radius
                        );

            foreach (RaycastHit hit in raycastHits) {
                if (hit.collider.gameObject != gameObject) {
                    IDamagable damagable = hit.collider.gameObject.GetComponent<IDamagable>();
                    if (damagable != null) {
                        float damageToDeal = (config as AreaAttackConfig).GetDamageToEachTarget() + abilityParams.baseDamage;
                        damagable.TakeDamage(damageToDeal);
                    }
                }
            }
        }
    }
}
