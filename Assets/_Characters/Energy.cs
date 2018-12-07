using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class Energy : MonoBehaviour {
        [SerializeField] Image energyOrb = null;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenerationPointsPerSecond = 10f;

        float currentEnergyPoints;

        void Start() {
            UpdateEnergyBar();
            currentEnergyPoints = maxEnergyPoints;
        }

        void Update() {
            if (currentEnergyPoints < maxEnergyPoints) {
                RegenerateBar();
                UpdateEnergyBar();
            }
        }

        private void RegenerateBar() {
            float regenerationEneryPoints = regenerationPointsPerSecond * Time.deltaTime + currentEnergyPoints;
            currentEnergyPoints = Mathf.Clamp(regenerationEneryPoints, 0, maxEnergyPoints);
        }

        public void ConsumeEnergyPoints(float amount) {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - amount, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        public bool IsEnergyAvailable(float amount){
            return currentEnergyPoints>=amount;
        }

        private void UpdateEnergyBar() {
            energyOrb.fillAmount = getCurrentEnergyPointsAsPercent();
        }

        private float getCurrentEnergyPointsAsPercent() {
            return currentEnergyPoints / maxEnergyPoints;
        }

    }
}