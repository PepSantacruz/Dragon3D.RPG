using UnityEngine;

namespace RPG.Characters {
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility {

        PowerAttackConfig config;

        public void SetConfiguration(PowerAttackConfig configToSet){
            config = configToSet;
        }

        public void Use(AbilityParams abilityParams) {
            DealDamageOnTarget(abilityParams);
            InstantiateParticleEffect(transform.position, Quaternion.identity);
        }

        private void InstantiateParticleEffect(Vector3 position, Quaternion quaternion) {
            GameObject effectPrefab = Instantiate(config.GetParticleEffectPrefab(), position, quaternion);
            ParticleSystem myParticleSystem = effectPrefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(effectPrefab, myParticleSystem.main.duration);
        }

        private void DealDamageOnTarget(AbilityParams abilityParams) {
            float damageToDeal = abilityParams.baseDamage + config.GetExtraDamage();
            abilityParams.target.TakeDamage(damageToDeal);
        }
    }
}
