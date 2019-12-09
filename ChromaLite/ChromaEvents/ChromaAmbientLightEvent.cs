﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChromaLite.ChromaEvents {

    class ChromaAmbientLightEvent : ChromaColourEvent {

        public ChromaAmbientLightEvent(BeatmapEventData data) : base(data, new Color[] { }) { }

        public override bool Activate(ref LightSwitchEventEffect light, ref BeatmapEventData data, ref BeatmapEventType eventType) {
            ColourManager.RecolourAmbientLights(A);
            return true;
        }

    }

}
