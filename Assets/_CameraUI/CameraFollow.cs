using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI {
    public class CameraFollow : MonoBehaviour {

        Transform playerTransform;
        // Use this for initialization
        void Start() {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        void LateUpdate() {
            transform.position = playerTransform.position;
        }
    }
}
