using RPG.Core;
using UnityEngine;

namespace RPG.Characters {

    public abstract class AbilityBehaviour : MonoBehaviour {

        protected AbilityConfig config;

        const float PARTICLE_CLEAN_UP_DELAY = 10.0f;

        public void SetConfiguration(AbilityConfig configToSet) {
            config = configToSet;
        }

        public abstract void Use(GameObject target = null);

        protected void PlayParticleEffect() {
            ParticleUtility.PlayParticleEffect(
                                    transform,
                                    config.GetParticleEffectPrefab(),
                                    transform.position,
                                    0
                            );
        }

        protected void PlayAbilityAnimation() {
            AnimatorOverrideController animatorOverrideController = GetComponent<Character>().GetAnimatorOverrideController();
            Animator animator = GetComponent<Animator>();

            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[AnimationConstants.DEFAULT_ATTACK] = config.GetSpecialAbilityAnimation();

            animator.SetTrigger(AnimationConstants.ATTACK_TRIGGER);

        }

        protected void PlayAbilitySound(){
            var audioClip = config.GetRandomAudioEffect();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClip);
        }
    }
}