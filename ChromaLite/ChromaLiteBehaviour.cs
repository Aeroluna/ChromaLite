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

            BeatmapData beatmapData = ReadBeatmapEvents(dataModel);
            
            //ChromaLogger.Log("Events read!");
        }

        public BeatmapData ReadBeatmapEvents(BeatmapData beatmapData) {
            //ChromaLogger.Log("Attempting to read lighting events");
            ChromaLiteMapReader.ReadMapData(beatmapData); //.CreateTransformedData(beatmapData, ref chroma, ref mode, ref gameplayOptions, ref gameplayMode);
            return beatmapData;
        }

    }

}
