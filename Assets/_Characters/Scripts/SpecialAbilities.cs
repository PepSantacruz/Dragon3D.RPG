using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class SpecialAbilities : MonoBehaviour {
        [SerializeField] private AbilityConfig[] specialAbilities = null;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenerationPointsPerSecond = 10f;
        [SerializeField] AudioClip outOfEnergyAudioClip;

        AudioSource audioSource;

        float currentEnergyPoints;

        float currentEnergyPointsAsPercent{ get { return currentEnergyPoints / maxEnergyPoints; } }

        void Start() {
            audioSource = GetComponent<AudioSource>();

            currentEnergyPoints = maxEnergyPoints;
            UpdateEnergyBar();

            AddSpecialAbilitiesComponents();
        }

        void Update() {
            if (currentEnergyPoints < maxEnergyPoints) {
                RegenerateBar();
                UpdateEnergyBar();
            }
        }

        void RegenerateBar() {
            float regenerationEneryPoints = regenerationPointsPerSecond * Time.deltaTime + currentEnergyPoints;
            currentEnergyPoints = Mathf.Clamp(regenerationEneryPoints, 0, maxEnergyPoints);
        }

        void UpdateEnergyBar() {
            energyBar.fillAmount = currentEnergyPointsAsPercent;
        }

        void AddSpecialAbilitiesComponents() {
            foreach (AbilityConfig config in specialAbilities)
                config.AttachAbilityTo(gameObject);
        }

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null) {
            float energyCost = specialAbilities[abilityIndex].GetEnergyCost();

            if (energyCost <= currentEnergyPoints) {
                ConsumeEnergyPoints(energyCost);
                specialAbilities[abilityIndex].Use(target);
            }
            else
                audioSource.PlayOneShot(outOfEnergyAudioClip);
        }


        public int getNumberOfAbilities() {
            return specialAbilities.Length;
        }

        public void ConsumeEnergyPoints(float amount) {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - amount, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

    }
}