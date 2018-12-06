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
        bool isCharacterDead = false;

        private void Start() {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            RegistrationOnPlayerDeath();

            agent.updateRotation = false;
            agent.updatePosition = true;
        }

        private void RegistrationOnPlayerDeath() {
            player = FindObjectOfType<Player>();
            player.onPlayerDeath += OnPlayerDeath;
        }

        void OnPlayerDeath() {
            isCharacterDead = true;
            agent.SetDestination(gameObject.transform.position);
            StopCharacterMovement();
        }

        private void Update()
        {
            if (!isCharacterDead) {
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
