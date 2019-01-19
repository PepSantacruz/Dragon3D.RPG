using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RPG.CameraUI;
using RPG.Core;

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
            characterMovement.Kill();
            var playerComponent = GetComponent<PlayerControl>();

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            animator.SetTrigger(Constants.DEATH_TRIGGER);

            if (playerComponent) {
                //to prevent the player move when is dead and the uses clicks on terrain
                FindObjectOfType<CameraRaycaster>().enabled = false;
            }
            else {
                //to prevent the enemy to follow the player while he is dead 
                GetComponent<EnemyAI>().enabled = false;
            }

            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            if (playerComponent && playerComponent.isActiveAndEnabled)
                SceneManager.LoadScene(0);
            else
                Destroy(gameObject, deathVanishSeconds);

        }
    }
}