using UnityEngine;

namespace RPG.CameraUI {
    public class CameraChange : MonoBehaviour {
        public void ChangePosition(Vector3 moveToPosition) {
            // Move the camera into position
            transform.position = Vector3.Lerp(transform.position, moveToPosition, 2);

            // Ensure the camera always looks at the player
            //transform.LookAt(transform.parent);
        }
    }
}