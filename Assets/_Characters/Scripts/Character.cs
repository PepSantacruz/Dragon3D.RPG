using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;
using System;

namespace RPG.Characters {

    [SelectionBase]
    public class Character : MonoBehaviour {
        //TODO create a collider just for the cursor click on enemy to attack
        [Header("Animator Setup")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;
        [SerializeField] [Range(0.1f, 1)] float animatorForward = 1f;

        [Header("Audio")]
        [SerializeField] float spatialBlend;

        [Header("CapsuleCollider")]
        [SerializeField] Vector3 colliderCenter;
        [SerializeField] float colliderRadius;
        [SerializeField] float colliderHeight;

        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier = 1.3f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;

        [Header("NavAgent")]
        [SerializeField] float navMeshAgentSteeringSpeed = 1.0f;
        [SerializeField] float navMeshAgentStopingDistance = 1.3f;

        [Header("Rigidbody")]

        [SerializeField] CollisionDetectionMode collisionDetectionMode;

        Animator animator;
        Rigidbody rigidBody;
        NavMeshAgent navAgent;

        float currentAnimatorForward = 1f;
        float turnAmount;
        float forwardAmount;
        bool isAlive = true;

        void Awake() {
            AddRequiredComponents();
        }

        void AddRequiredComponents() {
            CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = spatialBlend;

            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            rigidBody.collisionDetectionMode = collisionDetectionMode;
            rigidBody.mass = Mathf.Epsilon;

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;
            currentAnimatorForward = animatorForward;

            navAgent = gameObject.AddComponent<NavMeshAgent>();
            navAgent.speed = navMeshAgentSteeringSpeed;
            navAgent.stoppingDistance = navMeshAgentStopingDistance;
            navAgent.autoBraking = false;
            navAgent.updateRotation = false;
            navAgent.updatePosition = true;
        }

        void Update() {
            if (navAgent.remainingDistance > navAgent.stoppingDistance && isAlive) {
                ActivateRigidbody();
                Move(navAgent.desiredVelocity);
            }
            else {
                Move(Vector3.zero);
                if (!isAlive)
                    SetDestination(transform.position);
            }
        }

        private void DeactivateRigidbody() {
            rigidBody.isKinematic = true;
            rigidBody.detectCollisions = false;
        }

        public void ActivateRigidbody() {
            rigidBody.isKinematic = false;
            rigidBody.detectCollisions = true;
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

        void Move(Vector3 movement) {
            SetupForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        void SetupForwardAndTurn(Vector3 move) {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            if (move.magnitude > moveThreshold)
                move.Normalize();

            var localMove = transform.InverseTransformDirection(move);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void ApplyExtraTurnRotation() {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        void UpdateAnimator() {
            animator.SetFloat("Forward", forwardAmount * currentAnimatorForward, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }

        public void Kill() {
            isAlive = false;
        }

        public void SetDestination(Vector3 worldPosition) {
            navAgent.destination = worldPosition;
        }

        public AnimatorOverrideController GetAnimatorOverrideController() {
            return animatorOverrideController;
        }

        public float GetAnimationSpeedMultiplier() {
            return animator.speed;
        }

        public void setMaximumAnimatorForward() {
            currentAnimatorForward = 1;
        }

        public void setSelectedAnimatorForward() {
            currentAnimatorForward = animatorForward;
        }
    }
}

