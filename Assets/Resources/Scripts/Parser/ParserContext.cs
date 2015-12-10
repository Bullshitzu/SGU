using UnityEngine;
using System.Collections;

namespace FileParser {
    internal class ParserContext {

        public enum ContextType {
            Root,
            Object,
            Component
        }

        public bool ObjectsAllowed = false;
        public LineTypes.Property[] Properties;
        public LineTypes.Component[] Components;
        public ContextType Type;

        public ParserContext (bool objectsAllowed, LineTypes.Property[] properties, LineTypes.Component[] components, ContextType type) {
            this.ObjectsAllowed = objectsAllowed;
            this.Properties = properties;
            this.Components = components;
            this.Type = type;
        }

        public string ListValidMembers () {
            string temp = "";

            if (ObjectsAllowed) {
                temp += "Object []";
                if (Properties.Length > 0 || Components.Length > 0) temp += ", ";
            }

            for (int i = 0; i < Properties.Length; i++) {
                temp += Properties[i].name;
                if (Components.Length > 0 || i + 1 < Properties.Length) temp += ", ";
            }

            for (int i = 0; i < Components.Length; i++) {
                temp += Components[i].name+"()";
                if (i + 1 < Components.Length) temp += ", ";
            }

            return temp;
        }
    }
}
