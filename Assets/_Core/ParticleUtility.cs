using UnityEngine;

namespace RPG.Core {
    public static class ParticleUtility {
        public const float PARTICLE_STD_Y_OFFSET = 1.2f;

        const float PARTICLE_CLEAN_UP_DELAY = 5.0f;

        public static void PlayParticleEffect(Transform particleParentTransform, GameObject particleEffectPrefab, Vector3 particleEffectPosition, float particleEffectOffsetY) {
           
            particleEffectPosition.y += particleEffectOffsetY;

            GameObject effectPrefab = Object.Instantiate(  //the particle effect configures the local or world coordinates
                particleEffectPrefab,
                particleEffectPosition,
                particleEffectPrefab.transform.rotation);
            effectPrefab.transform.parent = particleParentTransform;

            effectPrefab.GetComponent<ParticleSystem>().Play();
            Object.Destroy(effectPrefab, PARTICLE_CLEAN_UP_DELAY);
        }
    }
}
