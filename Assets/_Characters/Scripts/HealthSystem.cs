using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG.Characters {
    public class HealthSystem : MonoBehaviour {
        [SerializeField] float maximumHealthPoints = 100f;
        [SerializeField] float deathVanishSeconds = 2f;
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        Animator animator;
        AudioSource audioSource;
        Character characterMovement;

        float currentHealthPoints;

        void Start() {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();

            currentHealthPoints = maximumHealthPoints;
        }

        // Update is called once per frame
        void Update() {
            UpdateHealthBar();
        }

        private void UpdateHealthBar() {
            if (healthBar)
                healthBar.fillAmount = healthAsPercentage;
        }

        public float healthAsPercentage {
            get {
                return currentHealthPoints / maximumHealthPoints;
            }
        }

        public void Heal(float healthToHeal) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + healthToHeal, 0f, maximumHealthPoints);
        }


        private void SetCurrentMaxHealth() {
            currentHealthPoints = maximumHealthPoints;
        }

        public void TakeDamage(float damage) {

            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maximumHealthPoints);
            if (currentHealthPoints <= 0) {
                GetComponent<Rigidbody>().isKinematic = true; //to prevent player collision when he is dead TODO error new unity version

                StartCoroutine(KillCharacter());
            }
            else {
                PlayAudioHit();
            }

        }

        private void PlayAudioHit() {
            if (audioSource.isPlaying == false) {
                AudioClip audioClip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
                audioSource.PlayOneShot(audioClip);
            }
        }

        IEnumerator KillCharacter() {
            StopAllCoroutines();
            characterMovement.Kill();

            var playerComponent = GetComponent<PlayerMovement>();

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            animator.SetTrigger(AnimationConstants.DEATH_TRIGGER);

            if (playerComponent && playerComponent.isActiveAndEnabled) {
                yield return new WaitForSecondsRealtime(audioSource.clip.length);
                SceneManager.LoadScene(0);
            }
            else
                Destroy(gameObject, deathVanishSeconds);

        }
    }
}