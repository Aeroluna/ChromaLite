using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ChromaLite {

    class ChromaLiteBehaviour : MonoBehaviour {

        void Start() {
            StartCoroutine(ReadEvents());
        }

        IEnumerator ReadEvents() {
            yield return new WaitForSeconds(0f);

            BeatmapObjectCallbackController beatmapObjectCallbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>()
                        .FirstOrDefault();
            BeatmapData dataModel = beatmapObjectCallbackController.GetField<BeatmapData>("_beatmapData");

                bool rgblight = UI.ChromaUI.ModPrefs.GetBool("Map", "customColourEventsEnabled", true, true);
            bool specialevent = UI.ChromaUI.ModPrefs.GetBool("SongCore", "customSpecialEventsEnabled", true, true);
            BeatmapData beatmapData = ReadBeatmapEvents(dataModel, rgblight, specialevent);
            
            //ChromaLogger.Log("Events read!");
        }

        public BeatmapData ReadBeatmapEvents(BeatmapData beatmapData, bool rgbEvents, bool specialEvents) {
            //ChromaLogger.Log("Attempting to read lighting events");
            ChromaLiteMapReader.ReadMapData(beatmapData, rgbEvents, specialEvents); //.CreateTransformedData(beatmapData, ref chroma, ref mode, ref gameplayOptions, ref gameplayMode);
            return beatmapData;
        }

    }

}
