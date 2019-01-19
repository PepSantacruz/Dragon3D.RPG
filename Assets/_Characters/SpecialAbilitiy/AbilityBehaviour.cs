using RPG.Core;
using UnityEngine;

namespace RPG.Characters {

    public abstract class AbilityBehaviour : MonoBehaviour {

        protected AbilityConfig config;

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
            animatorOverrideController[Constants.DEFAULT_ATTACK] = config.GetSpecialAbilityAnimation();

            animator.SetTrigger(Constants.ATTACK_TRIGGER);

        }

        protected void PlayAbilitySound(){
            var audioClip = config.GetRandomAudioEffect();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClip);
        }
    }
}