using System.Collections;
using UnityEngine;

namespace RPG.CameraUI {
    public class ChangeCameraPoint : MonoBehaviour {
        [SerializeField] float timeOfTravel = 0.5f;

        //Start vector : pos = new Vector3(5, 8 -11);  
        //Bridge vector : pos = new Vector3(10, 8 1);  

        RectTransform rectTransformCamera;

        void Start() {
            rectTransformCamera = Camera.main.GetComponent<RectTransform>();
        }

        private void OnTriggerEnter(Collider other) {
            // TODO use scriptable object to store the desired position or 4 predefined
            // TODO only if collides with the player
            print("Enter camera change");
            StopAllCoroutines();
            StartCoroutine(LerpObject(other.gameObject.transform,rectTransformCamera.anchoredPosition3D, new Vector3(10, 8, 1)));
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
