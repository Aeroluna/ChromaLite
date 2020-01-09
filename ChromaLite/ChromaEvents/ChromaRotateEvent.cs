using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChromaLite.ChromaEvents {

    class ChromaRotateEvent : ChromaColourEvent {

        public ChromaRotateEvent(BeatmapEventData data) : base(data, new Color[] { }) { }

        public override bool Activate(ref MonoBehaviour light, ref BeatmapEventData data, ref BeatmapEventType eventType) {
            PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
            if (playerController != null) {
                playerController.transform.root.rotation = Quaternion.Euler(A.r * 360, A.g * 360, A.b * 360);
                return true;
            }
            return false;
        }

    }

}
