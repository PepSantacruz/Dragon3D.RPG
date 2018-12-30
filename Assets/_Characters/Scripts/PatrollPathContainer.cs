using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class PatrollPathContainer : MonoBehaviour {

        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        void OnDrawGizmos() {
            Vector3 previousPosition = transform.GetChild(0).position;

            foreach(Transform waypoint in transform) {
                Gizmos.DrawSphere(waypoint.position, .2f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }

            Gizmos.DrawLine(previousPosition, transform.GetChild(0).position);

        }
    }
}