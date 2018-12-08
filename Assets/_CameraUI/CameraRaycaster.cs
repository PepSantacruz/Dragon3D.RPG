using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Characters;
using System;

namespace RPG.CameraUI {
    public class CameraRaycaster : MonoBehaviour {

        [SerializeField] public Texture2D walkCursor = null;
        [SerializeField] Texture2D swordCursor = null;
        [SerializeField] Texture2D questionCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        [SerializeField] GameObject gm;

        const int WALKABLE_LAYER = 9;
        float maxRaycastDepth = 100f;

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverTerrain;

        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        public void UIButtonClicked(){
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

        void PerformRaycasts(){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (RaycastForEnemy(ray)) return;
            if (RaycastForTerrain(ray)) return;
            SetQuestionCursor();
        }

        private void SetQuestionCursor() {
            Cursor.SetCursor(questionCursor, cursorHotspot, CursorMode.Auto);
        }

        bool RaycastForEnemy(Ray ray){
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, maxRaycastDepth)) {
                gm= hitInfo.collider.gameObject;
                GameObject gameObjectHit = hitInfo.collider.gameObject;
                Enemy enemyHit = gameObjectHit.GetComponent<Enemy>();
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
            LayerMask terrainLayerMask = 1 << WALKABLE_LAYER;
            bool terrainHit=Physics.Raycast(ray, out hitInfo, maxRaycastDepth,terrainLayerMask);
            if (terrainHit) {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverTerrain(hitInfo.point);
                return true;
            }
            return false;
        }
    }
}