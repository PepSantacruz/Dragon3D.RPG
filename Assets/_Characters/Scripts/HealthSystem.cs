using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RPG.CameraUI;
using RPG.Core;
using UnityEngine.AI;

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
        SlowMotion slowMotion;

        float currentHealthPoints;

        void Start() {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();
            slowMotion = Camera.main.GetComponent<SlowMotion>();

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
            slowMotion.SlowTime();
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
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(GetComponent<Rigidbody>());

            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            if (playerComponent && playerComponent.isActiveAndEnabled)
                SceneManager.LoadScene(0);
            else
                StartCoroutine(FadeTo(0, deathVanishSeconds));
        }

        IEnumerator FadeTo(float aValue, float aTime) {
            SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            MeshRenderer[] mesheshRenderers = GetComponentsInChildren<MeshRenderer>();

            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                ChangeRendererMode(skinnedMeshRenderer.material);

            foreach (MeshRenderer mesheshRenderer in mesheshRenderers)
                ChangeRendererMode(mesheshRenderer.material);


            float alpha = skinnedMeshRenderers[0].material.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));

                foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                    skinnedMeshRenderer.material.color = newColor;

                foreach (MeshRenderer mesheshRenderer in mesheshRenderers)
                    mesheshRenderer.material.color = newColor;

                yield return null;
            }

            Destroy(gameObject);
        }

        private static void ChangeRendererMode(Material material) {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }
}