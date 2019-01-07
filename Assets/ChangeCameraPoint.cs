using System.Collections;
using UnityEngine;

namespace RPG.CameraUI {
    public class ChangeCameraPoint : MonoBehaviour {
        [SerializeField] float timeOfTravel = 2;

        RectTransform rectTransformCamera;

        void Start() {
            rectTransformCamera = Camera.main.GetComponent<RectTransform>();
        }

        private void OnTriggerEnter(Collider other) {
            // TODO use scriptable object to store the desired position
            print("Enter camera change");
            StartCoroutine(LerpObject(other.gameObject.transform,rectTransformCamera.anchoredPosition3D, new Vector3(0, 8, -10)));
        }

        IEnumerator LerpObject(Transform targetToLook, Vector3 startPosition, Vector3 endPosition) {
            float currentTime = 0;
            while (currentTime <= timeOfTravel) {
                currentTime += Time.deltaTime;
                float normalizedValue = currentTime / timeOfTravel; // we normalize our time 
                rectTransformCamera.anchoredPosition3D = Vector3.Lerp(startPosition, endPosition, normalizedValue);
                rectTransformCamera.LookAt(targetToLook);
                yield return null;
            }

        }
    }
}
