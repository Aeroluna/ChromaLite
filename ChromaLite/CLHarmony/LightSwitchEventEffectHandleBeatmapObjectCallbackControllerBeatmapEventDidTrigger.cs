﻿using ChromaLite.ChromaEvents;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChromaLite.CLHarmony {

    [HarmonyPatch(typeof(LightSwitchEventEffect))]
    [HarmonyPatch("HandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger")]
    class LightSwitchEventEffectHandleBeatmapObjectCallbackControllerBeatmapEventDidTrigger {

        static bool Prefix(LightSwitchEventEffect __instance, ref BeatmapEventData beatmapEventData, ref BeatmapEventType ____event) {

            if (beatmapEventData.type == ____event) {
                //CustomLightBehaviour customLight = CustomLightBehaviour.GetCustomLightColour(beatmapEventData);
                ChromaEvent customEvent = ChromaEvent.GetChromaEvent(beatmapEventData);
                if (customEvent != null) {
                    MonoBehaviour __monobehaviour = __instance;
                    customEvent.Activate(ref __monobehaviour, ref beatmapEventData, ref ____event);
                    return false;
                }
            }

            return true;
        }

    }

}
