using UnityEngine;

namespace RPG.Characters{

    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        [SerializeField] public Transform target;                                    // target to aim for

        Player player;
        Rigidbody playerRigidBody;
        Animator animator;

        private void Start() {
            FindComponents();
            RegistrationOnPlayerDeath();

            agent.updateRotation = false;
            agent.updatePosition = true;
        }

        private void FindComponents() {
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            player = FindObjectOfType<Player>();
            playerRigidBody = player.GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        private void RegistrationOnPlayerDeath() {
            player.onPlayerDeath += OnPlayerDeath;
        }

        void OnPlayerDeath() {
            agent.SetDestination(gameObject.transform.position);
            animator.SetFloat("Forward", 0f);
        }

        private void Update()
        {
            if (!playerRigidBody.isKinematic) {  //if player is dead don't allow click on terrain to move
                if (target != null)
                    agent.SetDestination(target.position);

                if (agent.remainingDistance > agent.stoppingDistance)
                    character.Move(agent.desiredVelocity, false);
                else {
                    StopCharacterMovement();
                }
            }
        }

        private void StopCharacterMovement() {
            agent.velocity = Vector3.zero;
            character.Move(Vector3.zero, false);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
