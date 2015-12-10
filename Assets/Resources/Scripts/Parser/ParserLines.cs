using UnityEngine;
using System.Collections;

namespace FileParser {
    internal interface IParserLine {
        Parser.LineType GetType ();
    }

    internal class LineTypes {

        public class Property : IParserLine {
            public new Parser.LineType GetType () {
                return Parser.LineType.Property;
            }

            public enum ArgumentTypes {
                String,
                Float,
                Integer,
                Vector2,
                Vector3,
                Boolean,
                Color,
                TexturePath,
                Enumeration
            }

            public string name;
            public string validExample;
            public ArgumentTypes[] arguments;
            public object[] argumentsData;

            public Property (string name, string validExample, params ArgumentTypes[] arguments) {
                this.name = name;
                this.validExample = validExample;
                this.arguments = arguments;
            }
            public Property (string name, params object[] arguments) {
                this.name = name;
                argumentsData = arguments;
            }

            #region Operators
            public static bool operator == (Property a, Property b) {
                return a.name.ToLower().Trim() == b.name.ToLower().Trim();
            }
            public static bool operator != (Property a, Property b) {
                return !(a == b);
            }
            public override bool Equals (object obj) {
                return base.Equals(obj);
            }
            public override int GetHashCode () {
                return base.GetHashCode();
            }
            #endregion
        }

        public class Component : IParserLine {
            public new Parser.LineType GetType () {
                return Parser.LineType.Component;
            }

            public string name;
            public ParserContext context;
            public Component (string name, ParserContext context) {
                this.name = name;
                this.context = context;
            }

            #region Operators
            public static bool operator == (Component a, Component b) {
                return a.name.ToLower().Trim() == b.name.ToLower().Trim();
            }
            public static bool operator != (Component a, Component b) {
                return !(a == b);
            }
            public override bool Equals (object obj) {
                return base.Equals(obj);
            }
            public override int GetHashCode () {
                return base.GetHashCode();
            }
            #endregion
        }

        public class Object : IParserLine {
            public new Parser.LineType GetType () {
                return Parser.LineType.Object;
            }

            public string name;
            public Object (string name) {
                this.name = name;
            }
        }

        public class ComponentClose : IParserLine {
            public new Parser.LineType GetType () {
                return Parser.LineType.ComponentClose;
            }
        }
        public class ObjectClose : IParserLine {
            public new Parser.LineType GetType () {
                return Parser.LineType.ObjectClose;
            }
        }
    }
}
