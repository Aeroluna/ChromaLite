﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChromaLite.ChromaEvents {

    abstract class ChromaColourEvent : ChromaEvent {

        private Color[] colors = new Color[1];

        public int Amount {
            get { return colors.Length; }
        }

        public Color[] Colors {
            get { return colors; }
            set { colors = value; }
        }

        public Color A {
            get { return colors[0]; }
            set {
                colors[0] = value;
            }
        }

        public Color B {
            get { return colors.Length > 1 ? colors[1] : colors[0]; }
            set {
                if (colors.Length < 2) {
                    Color[] newColors = new Color[2];
                    newColors[0] = colors[0];
                    colors = newColors;
                }
                colors[1] = value;
            }
        }

        public ChromaColourEvent(BeatmapEventData data, Color a) : this(data, a, a) { }

        public ChromaColourEvent(BeatmapEventData data, Color a, Color b) : this(data, new Color[] { a, b }) { }

        public ChromaColourEvent(BeatmapEventData data, Color[] colors) : base(data) {
            this.colors = colors;
        }

        public Color GetColor(int i) {
            return colors[i];
        }

    }

}
