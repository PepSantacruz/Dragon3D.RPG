using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Characters;
using RPG.Core;

namespace RPG.CameraUI {
    public class CameraRaycaster : MonoBehaviour {

        [SerializeField] public Texture2D walkCursor;
        [SerializeField] Texture2D swordCursor;
        [SerializeField] Texture2D questionCursor;
        [SerializeField] Vector2 cursorHotspot;

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverTerrain;

        public delegate void OnMouseOverEnemy(EnemyAI enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        public void UIButtonClicked() {
            print("Button Clicked");
        }

        void Update() {

            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }
            else {
                PerformRaycasts();
            }
        }

        void PerformRaycasts() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (RaycastForEnemy(ray)) return;
            if (RaycastForTerrain(ray)) return;
            SetQuestionCursor();
        }

        private void SetQuestionCursor() {
            Cursor.SetCursor(questionCursor, cursorHotspot, CursorMode.Auto);
        }

        bool RaycastForEnemy(Ray ray) {
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Constants.MAX_RAYCASTER_DEPTH)) {
                GameObject gameObjectHit = hitInfo.collider.gameObject;
                EnemyAI enemyHit = gameObjectHit.GetComponent<EnemyAI>();
                if (enemyHit) {
                    Cursor.SetCursor(swordCursor, cursorHotspot, CursorMode.Auto);
                    onMouseOverEnemy(enemyHit);
                    return true;
                }
            }
            return false;
        }

        bool RaycastForTerrain(Ray ray) {
            RaycastHit hitInfo;
            LayerMask terrainLayerMask = 1 << Constants.WALKABLE_LAYER;
            bool terrainHit = Physics.Raycast(ray, out hitInfo, Constants.MAX_RAYCASTER_DEPTH, terrainLayerMask);
            if (terrainHit) {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverTerrain(hitInfo.point);
                return true;
            }
            return false;
        }
    }
}