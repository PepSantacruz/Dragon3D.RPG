using UnityEngine;
using RPG.Core;

namespace RPG.Characters {
    public class AreaAttackBehaviour : MonoBehaviour, ISpecialAbility {

        AreaAttackConfig config;

        public void SetConfiguration(AreaAttackConfig configToSet){
            config = configToSet;
        }

        public void Use(AbilityParams abilityParams){
            RaycastHit[] raycastHits = Physics.SphereCastAll(
                transform.position, config.GetRadius(), Vector3.up, config.GetRadius()
            );

            foreach(RaycastHit hit in raycastHits){
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
