using UnityEngine;
using System.Collections;

namespace AssetDatabaseHelpers {
    public class Exceptions {

        // todo: individual exceptions here

    }
    public class Helpers {
        public static string CorrectPath (string path) {
            return path.Replace('\\', '/');
        }
    }
}
