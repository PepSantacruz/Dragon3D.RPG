using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class Energy : MonoBehaviour {
        [SerializeField] RawImage energyBar = null;
        [SerializeField] float maxEnergyPoints = 100f;

        float currentEnergyPoints;

        void Start() {
            currentEnergyPoints = maxEnergyPoints;
        }

        public void ConsumeEnergyPoints(float amount) {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - amount, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        public bool IsEnergyAvailable(float amount){
            return currentEnergyPoints>=amount;
        }

        private void UpdateEnergyBar() {
            float xValue = -(getCurrentEnergyPointsAsPercent() / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        private float getCurrentEnergyPointsAsPercent() {
            return currentEnergyPoints / maxEnergyPoints;
        }

    }
}