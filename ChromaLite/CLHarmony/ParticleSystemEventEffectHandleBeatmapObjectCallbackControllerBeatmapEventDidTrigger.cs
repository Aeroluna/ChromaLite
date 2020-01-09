using ChromaLite.ChromaEvents;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChromaLite.CLHarmony {

    [HarmonyPatch(typeof(ParticleSystemEventEffect))]
    [HarmonyPatch("HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger")]
    class ParticleSystemEventEffectHandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger {

        static bool Prefix(ParticleSystemEventEffect __instance, ref BeatmapEventData beatmapEventData, ref BeatmapEventType ____colorEvent) {

            if (beatmapEventData.type == ____colorEvent) {
                //CustomLightBehaviour customLight = CustomLightBehaviour.GetCustomLightColour(beatmapEventData);
                ChromaEvent customEvent = ChromaEvent.GetChromaEvent(beatmapEventData);
                if (customEvent != null) {
                    MonoBehaviour __monobehaviour = __instance;
                    customEvent.Activate(ref __monobehaviour, ref beatmapEventData, ref ____colorEvent);
                    return false;
                }
            }

            return true;
        }

    }

}
