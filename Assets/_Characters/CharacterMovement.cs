using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters {

    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(NavMeshAgent))]

    public class CharacterMovement : MonoBehaviour {
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 2f;

        ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
        GameObject walkTarget;
        Animator animator;
        Rigidbody rigidBody;
        NavMeshAgent agent;
        //Transform m_Cam;

        private void Start() {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("walkTarget");

            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();

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
                character.Move(agent.desiredVelocity);
            }
            else {
                character.Move(Vector3.zero);
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

        //Callback! don't delete it
        void OnAnimatorMove() {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0) {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
            }

        }

    }
}

