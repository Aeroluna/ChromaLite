using ChromaLite.ChromaEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChromaLite {

    class ChromaLiteMapReader {

        public static void ReadMapData(BeatmapData beatmapData, bool rgbEvents, bool specialEvents) {

            ChromaLogger.Log("Reading event data... ");

            //Events
            BeatmapEventData[] bevData = beatmapData.beatmapEventData;
            ChromaColourEvent unfilledEvent = null;
            for (int i = bevData.Length - 1; i >= 0; i--) {
                try {
                    //ChromaLogger.Log("Event data: " + i);
                    ChromaEvent cLight = ApplyCustomEvent(bevData[i], ref unfilledEvent, rgbEvents, specialEvents);
                    //if (cLight != null) ChromaLogger.Log("Custom Event " + cLight.GetType() + " found."); //lets not destroy the log
                } catch (Exception e) {
                    ChromaLogger.Log(e);
                    continue;
                }
            }

        }

        // Colour Events
        //   - Stores two colour values, A and B
        //   - These events have two purposes
        //     - Used to recolour any lighting events placed after them, unless...
        //     - if used immediately after a "data event", their values will influence the data event.
        // 1,900,000,001 = 1900000001 = A/B
        // 1,900,000,002 = 1900000002 = AltA/AltB
        // 1,900,000,003 = 1900000003 = White/Half White
        // 1,900,000,004 = 1900000004 = Technicolor/Technicolor
        // 1,900,000,005 = 1900000005 = RandomColor/RandomColor

        // Data Events
        //   - Require Colour Events before them
        //   - Remember Unity colour uses 0-1, not 0-255
        // 1,950,000,001 = 1950000001 = Note Scale Event        - Scales all future spawned notes by (A.red * 1.5f)
        // 1,950,000,002 = 1950000002 = Health Event            - Alters the player's health by ((A.red - 0.5f) * 2)
        // 1,950,000,003 = 1950000003 = Rotate Event            - Rotates the player on all three axes by (A.red * 360 on x axis, A.green * 360 on y axis, A.blue * 360 on z axis)
        // 1,950,000,004 = 1950000004 = Ambient Light Event     - Immediately changes the colour of ambient lights to (A)
        // 1,950,000,005 = 1950000005 = Barrier Colour Event    - Changes all future spawned barrier colours to (A)

        // > 2,000,000,000 = >2000000000 = RGB (see ColourManager.ColourFromInt)

        public static ChromaEvent ApplyCustomEvent(BeatmapEventData bev, ref ChromaColourEvent unfilledColourEvent, bool rgbEvents, bool specialEvents) {

            //ChromaLogger.Log("Checking BEV ||| " + bev.time + "s : " + bev.value + "v");

            Color a, b;

            if (bev.value >= ColourManager.RGB_INT_OFFSET) { // > 2,000,000,000 = >2000000000 = RGB (see ColourManager.ColourFromInt)
                a = ColourManager.ColourFromInt(bev.value);
                b = a;
                if (FillColourEvent(bev, ref unfilledColourEvent, a)) return unfilledColourEvent;
            } else {
                switch (bev.value) {
                    case ChromaLightEvent.CHROMA_LIGHT_OFFSET + 1: // 1,900,000,001 = 1900000001 = A/B
                        a = ColourManager.LightA;
                        b = ColourManager.LightB;
                        break;
                    case ChromaLightEvent.CHROMA_LIGHT_OFFSET + 2: // 1,900,000,002 = 1900000002 = AltA/AltB
                        a = ColourManager.LightAltA;
                        b = ColourManager.LightAltB;
                        break;
                    case ChromaLightEvent.CHROMA_LIGHT_OFFSET + 3: // 1,900,000,003 = 1900000003 = White/Half White
                        a = ColourManager.LightWhite;
                        b = ColourManager.LightGrey;
                        break;
                    case ChromaLightEvent.CHROMA_LIGHT_OFFSET + 4: // 1,900,000,004 = 1900000004 = Technicolor/Technicolor
                        a = ColourManager.GetTechnicolour(true, bev.time, ColourManager.TechnicolourStyle.WARM_COLD);
                        b = ColourManager.GetTechnicolour(false, bev.time, ColourManager.TechnicolourStyle.WARM_COLD);
                        break;
                    case ChromaLightEvent.CHROMA_LIGHT_OFFSET + 5: // 1,900,000,005 = 1900000005 = RandomColor/RandomColor
                        a = UnityEngine.Random.ColorHSV(); a.a = 1;
                        b = UnityEngine.Random.ColorHSV(); b.a = 1;
                        break;
                    /*
                     * 
                     */
                    case ChromaEvent.CHROMA_EVENT_SCALE: //1,950,000,001 = 1950000001 = Note Scale Event
                        if (specialEvents) unfilledColourEvent = new ChromaNoteScaleEvent(bev);
                        return null;
                    case ChromaEvent.CHROMA_EVENT_HEALTH: //1,950,000,002 = 1950000002 = Health Event
                        if (specialEvents) unfilledColourEvent = new ChromaHealthEvent(bev);
                        return null;
                    case ChromaEvent.CHROMA_EVENT_ROTATE: //1,950,000,003 = 1950000003 = Rotate Event
                        if (specialEvents) unfilledColourEvent = new ChromaRotateEvent(bev);
                        return null;
                    case ChromaEvent.CHROMA_EVENT_AMBIENT_LIGHT: //1,950,000,004 = 1950000004 = Ambient Light Event
                        if (specialEvents) unfilledColourEvent = new ChromaAmbientLightEvent(bev);
                        return null;
                    case ChromaEvent.CHROMA_EVENT_BARRIER_COLOUR: //1,950,000,005 = 1950000005 = Barrier Colour Event
                        if (specialEvents) unfilledColourEvent = new ChromaBarrierColourEvent(bev);
                        return null;
                    default: return null;
                }
                if (FillColourEvent(bev, ref unfilledColourEvent, a, b)) return unfilledColourEvent;
            }

            if (unfilledColourEvent != null) unfilledColourEvent = null;

            if (!rgbEvents) return null;
            return ChromaEvent.SetChromaEvent(bev, new ChromaLightEvent(bev, a, b));
        }

        public static bool FillColourEvent(BeatmapEventData bev, ref ChromaColourEvent unfilledColourEvent, params Color[] colors) {
            if (unfilledColourEvent != null) {
                unfilledColourEvent.Colors = colors;
                ChromaEvent.SetChromaEvent(bev, unfilledColourEvent);
                //ChromaLogger.Log("Filled " + unfilledColourEvent.GetType().ToString() + " event.");
                unfilledColourEvent = null;
                return true;
            }
            return false;
        }

    }

}
