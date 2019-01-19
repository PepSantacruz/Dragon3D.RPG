using System.Collections;
using UnityEngine;
using RPG.Core;

namespace RPG.CameraUI {
    public class ChangeCameraPoint : MonoBehaviour {
        [SerializeField] float timeOfTravel;
        [SerializeField] Vector3 pointOfView;

        RectTransform rectTransformCamera;

        void Start() {
            rectTransformCamera = Camera.main.GetComponent<RectTransform>();
        }

        private void OnTriggerEnter(Collider other) {
       
            if (other.gameObject.tag == Constants.PLAYER_TAG) {
                StopAllCoroutines();
                Vector3 oldPointOfView = rectTransformCamera.anchoredPosition3D;
                StartCoroutine(LerpObject(other.gameObject.transform, rectTransformCamera.anchoredPosition3D, pointOfView));
                pointOfView = oldPointOfView;
            }
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
