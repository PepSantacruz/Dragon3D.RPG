using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters {

    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]

    public class PlayerMovement : MonoBehaviour {

        AICharacterControl aICharacterControl;
        ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster;
        Vector3 clickPoint;
        GameObject walkTarget;

        bool isInIndirectMode = false;

        Transform m_Cam;

        private void Start() {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            aICharacterControl = GetComponent<AICharacterControl>();
            m_Cam = Camera.main.transform;

            cameraRaycaster.onMouseOverTerrain += OnMouseOverTerrain;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

            walkTarget = new GameObject("walkTarget");
        }

        void OnMouseOverTerrain(Vector3 destination) {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1)){
                walkTarget.transform.position = destination;
                aICharacterControl.SetTarget(walkTarget.transform);
            }
        }

        void OnMouseOverEnemy(Enemy enemy) {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) 
                aICharacterControl.SetTarget(enemy.transform);

        }

        private void ProcessDirectMovment() {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 cameraForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * cameraForward + h * m_Cam.right;

            //thirdPersonCharacter.Move(move, false, false);
        }

    }
}

