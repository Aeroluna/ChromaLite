using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromaLite.ChromaEvents {

    abstract class ChromaEvent {

        public const int CHROMA_EVENT_SCALE = 1950000001;
        public const int CHROMA_EVENT_HEALTH = 1950000002;
        public const int CHROMA_EVENT_ROTATE = 1950000003;
        public const int CHROMA_EVENT_AMBIENT_LIGHT = 1950000004;
        public const int CHROMA_EVENT_BARRIER_COLOUR = 1950000005;

        private static Dictionary<BeatmapEventData, ChromaEvent> chromaEvents = new Dictionary<BeatmapEventData, ChromaEvent>();

        public static void ClearChromaEvents() {
            chromaEvents.Clear();

            ChromaBarrierColourEvent.Clear();
            ChromaNoteScaleEvent.Clear();
        }

        public static ChromaEvent SetChromaEvent(BeatmapEventData lightEvent, ChromaEvent chromaEvent) {
            if (chromaEvents.ContainsKey(lightEvent)) {
                chromaEvents.Remove(lightEvent);
            }
            chromaEvents.Add(lightEvent, chromaEvent);
            chromaEvent.OnEventSet(lightEvent);
            return chromaEvent;
        }

        public static void RemoveChromaEvent(BeatmapEventData beatmapEvent) {
            if (chromaEvents.ContainsKey(beatmapEvent)) chromaEvents.Remove(beatmapEvent);
        }

        public static ChromaEvent GetChromaEvent(BeatmapEventData beatmapEvent) {
            if (chromaEvents.ContainsKey(beatmapEvent)) {
                if (chromaEvents.TryGetValue(beatmapEvent, out ChromaEvent chromaEvent)) return chromaEvent;
            }
            return null;
        }

        protected BeatmapEventData data;
        public BeatmapEventData Data {
            get {
                return data;
            }
        }

        public ChromaEvent(BeatmapEventData data) {
            this.data = data;
        }

        public abstract bool Activate(ref UnityEngine.MonoBehaviour light, ref BeatmapEventData data, ref BeatmapEventType eventType);

        public virtual void OnEventSet(BeatmapEventData lightmapEvent) {

        }

    }

}
