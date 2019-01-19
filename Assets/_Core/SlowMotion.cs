using UnityEngine;
using System.Collections;
using RPG.Core;

namespace RPG.Core {
    public class SlowMotion: MonoBehaviour {
        public void SlowTime() {
            Time.timeScale = Constants.SLOW_MOTION_RATE;
            StartCoroutine(RestoreTime());
        }

        IEnumerator RestoreTime() {
            yield return new WaitForSecondsRealtime(Constants.SLOW_MOTION_TIME);
            Time.timeScale = 1;
        }
    }
}
