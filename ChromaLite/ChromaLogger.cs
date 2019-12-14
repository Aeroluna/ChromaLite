using System;
using IPALogger = IPA.Logging.Logger;

namespace ChromaLite {

    public static class ChromaLogger {

        public static IPALogger logger;

        public static void Log(Exception e) {
            logger.Error(e);
        }

        public static void Log(Object obj) {
            Log(obj.ToString());
        }

        public static void Log(string message) {
            logger.Info(message);
        }

    }

}
