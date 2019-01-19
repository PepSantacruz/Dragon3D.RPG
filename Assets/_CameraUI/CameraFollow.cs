using System.Collections;
using RPG.Core;
using UnityEngine;

namespace RPG.CameraUI {
    public class CameraFollow : MonoBehaviour {

        Transform playerTransform;
        RectTransform cameraRectTransform;

        void Start() {
            playerTransform = GameObject.FindWithTag(Constants.PLAYER_TAG).transform;
            cameraRectTransform = Camera.main.GetComponent<RectTransform>();
        }

        void LateUpdate() {
            transform.position = playerTransform.position;
            cameraRectTransform.LookAt(playerTransform);
        }
    }
}
