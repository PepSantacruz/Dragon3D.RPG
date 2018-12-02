using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons {

    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject {

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;

        public Transform weaponTransform;

        public float GetMinTimeBetweenHits(){
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange(){
            return maxAttackRange;
        }

        public AnimationClip GetAnimationClip() {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        //So that asset pack cannot cause crashes 
        private void RemoveAnimationEvents() {
            attackAnimation.events = new AnimationEvent[0];
        }

        public GameObject GetWeaponPrefab() {
            return weaponPrefab;
        }

    }
}
