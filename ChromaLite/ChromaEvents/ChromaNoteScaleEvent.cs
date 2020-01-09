using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChromaLite.ChromaEvents {

    class ChromaNoteScaleEvent : ChromaColourEvent {

        public float Scale {
            get { return A.r * 1.5f; }
        }

        public ChromaNoteScaleEvent(BeatmapEventData data) : base(data, new Color[] { }) { }

        public override bool Activate(ref MonoBehaviour light, ref BeatmapEventData data, ref BeatmapEventType eventType) {
            /*if (ChromaBehaviour.Instance is ChromaBehaviour chroma) {
                ChromaLogger.Log("Scalechange : " + Scale);
                chroma.eventNoteScale = Scale;
                return true;
            }
            return false;*/
            return true;
        }

        private static Dictionary<float, float> noteTimeToScale = new Dictionary<float, float>();

        public override void OnEventSet(BeatmapEventData lightmapEvent) {
            noteTimeToScale.Add(data.time, Scale);
            base.OnEventSet(lightmapEvent);
        }

        public static void Clear() {
            noteTimeToScale.Clear();
        }

        public static float GetScale(float time) {
            float scale = 1f;
            foreach (KeyValuePair<float, float> keyv in noteTimeToScale.OrderBy(i => i.Key)) {
                if (time >= keyv.Key) scale = keyv.Value;
            }
            return scale;
        }

        /**/

    }

}
