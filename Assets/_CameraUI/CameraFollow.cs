using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI {
    public class CameraFollow : MonoBehaviour {

        Transform playerTransform;
        RectTransform cameraRectTransform;

        void Start() {
            playerTransform = GameObject.FindWithTag("Player").transform;
            cameraRectTransform = Camera.main.GetComponent<RectTransform>();
        }

        void LateUpdate() {
            transform.position = playerTransform.position;
            cameraRectTransform.LookAt(playerTransform);
        }
    }
}
