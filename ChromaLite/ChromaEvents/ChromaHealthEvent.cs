using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChromaLite.ChromaEvents {

    class ChromaHealthEvent : ChromaColourEvent {

        public float HealthChangeAmount {
            get { return (A.r - 0.5f) * 2; }
        }

        public ChromaHealthEvent(BeatmapEventData data) : base(data, new Color[] { }) { }

        public override bool Activate(ref MonoBehaviour light, ref BeatmapEventData data, ref BeatmapEventType eventType) {
            GameEnergyCounter counter = GameObject.FindObjectOfType<GameEnergyCounter>();
            if (counter != null) {
                //ChromaLogger.Log("Changing health by " + HealthChangeAmount);
                counter.SetField("energy", counter.energy + HealthChangeAmount);
                return true;
            }
            return false;
        }

    }

}
