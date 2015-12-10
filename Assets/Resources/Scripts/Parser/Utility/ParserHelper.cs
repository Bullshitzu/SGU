using UnityEngine;
using System.Collections;

namespace FileParser {
    public class Helper {

        internal static string GetPropertyFormatExample (FileParser.LineTypes.Property prop) {
            string temp = prop.name + " = ";
            for (int i = 0; i < prop.arguments.Length; i++) {
                temp += GetArgumentExampleWord(prop.arguments[i]);
                if (i + 1 < prop.arguments.Length) temp += " = ";
            }
            return temp;
        }
        private static string GetArgumentExampleWord (FileParser.LineTypes.Property.ArgumentTypes arg) {
            switch (arg) {
                case FileParser.LineTypes.Property.ArgumentTypes.Boolean:
                    return "BOOLEAN";
                case FileParser.LineTypes.Property.ArgumentTypes.Color:
                    return "R, G, B, A";
                case FileParser.LineTypes.Property.ArgumentTypes.TexturePath:
                    return "PATH";
                case FileParser.LineTypes.Property.ArgumentTypes.Vector2:
                    return "X, Y";
                case FileParser.LineTypes.Property.ArgumentTypes.Vector3:
                    return "X, Y, Z";
                default:
                    return "VALUE";
            }
        }
    }
}
