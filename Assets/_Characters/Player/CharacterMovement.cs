using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters {

    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(NavMeshAgent))]

    public class CharacterMovement : MonoBehaviour {
        [SerializeField] float stoppingDistance = 1f;

        ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
        GameObject walkTarget;
        NavMeshAgent agent;

        //Transform m_Cam;

        private void Start() {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("walkTarget");

            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;

            agent.stoppingDistance = stoppingDistance;

            //m_Cam = Camera.main.transform;

            cameraRaycaster.onMouseOverTerrain += OnMouseOverTerrain;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

           
        }

        void Update() {
            if (agent.remainingDistance > agent.stoppingDistance) {
                character.Move(agent.desiredVelocity, false);
            }
            else {
                character.Move(Vector3.zero,false);
            }
        }

        void OnMouseOverTerrain(Vector3 destination) {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                agent.SetDestination(destination);
        }

        void OnMouseOverEnemy(Enemy enemy) {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) 
                agent.SetDestination(enemy.transform.position);
        }

        //private void ProcessDirectMovment() {
        //    float h = Input.GetAxis("Horizontal");
        //    float v = Input.GetAxis("Vertical");

        //    Vector3 cameraForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        //    Vector3 move = v * cameraForward + h * m_Cam.right;

        //    //thirdPersonCharacter.Move(move, false, false);
        //}

    }
}

