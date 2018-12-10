using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {
    public class AreaAttackBehaviour : MonoBehaviour, ISpecialAbility {

        AreaAttackConfig config = null;

        public void SetConfiguration(AreaAttackConfig configToSet){
            config = configToSet;
        }

        public void Use(AbilityParams abilityParams) {
            DealRadialDamage(abilityParams);
            InstantiateParticleEffect(transform.position, Quaternion.identity);
        }

        private void InstantiateParticleEffect(Vector3 position, Quaternion quaternion) {
            GameObject effectPrefab = Instantiate(config.GetParticleEffectPrefab(), position, quaternion);
            ParticleSystem myParticleSystem = effectPrefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(effectPrefab, myParticleSystem.main.duration);
        }

        private void DealRadialDamage(AbilityParams abilityParams) {
            RaycastHit[] raycastHits = Physics.SphereCastAll(
                            transform.position, config.GetRadius(), Vector3.up, config.GetRadius()
                        );

            foreach (RaycastHit hit in raycastHits) {
                if (hit.collider.gameObject != gameObject) {
                    IDamagable damagable = hit.collider.gameObject.GetComponent<IDamagable>();
                    if (damagable != null) {
                        float damageToDeal = config.GetDamageToEachTarget() + abilityParams.baseDamage;
                        damagable.TakeDamage(damageToDeal);
                    }
                }
            }
        }
    }
}
