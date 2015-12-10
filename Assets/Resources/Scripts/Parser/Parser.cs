using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace FileParser {
    public partial class Parser {

        #region Line Interpreting Members
        public enum LineType {
            Empty,
            Comment,
            Property,
            Component,
            Object,
            ComponentClose,
            ObjectClose,
            Unknown
        }
        public static LineType GetLineType (string line) {
            line = line.Trim();

            if (line == "") return LineType.Empty;
            if (line.Trim().StartsWith("//")) return LineType.Comment;
            if (line.Contains("=")) return LineType.Property;
            if (line.EndsWith("(")) return LineType.Component;
            if (line.EndsWith("[")) return LineType.Object;
            if (line.Trim() == ")") return LineType.ComponentClose;
            if (line.Trim() == "]") return LineType.ObjectClose;
            return LineType.Unknown;
        }
        internal LineTypes.Property GetProperty (string line, LineTypes.Property contextProperty) {

            string[] args = line.Split('=');
            if (args.Length - 1 != contextProperty.arguments.Length) throw new ParserExceptions.WrongLineFormatException(contextProperty.name, contextProperty.validExample);

            object[] parameters = new object[args.Length - 1];

            for (int i = 1; i < args.Length; i++) {
                switch (contextProperty.arguments[i - 1]) {
                    case LineTypes.Property.ArgumentTypes.Integer:
                        parameters[i - 1] = ParseInt(args[i]);
                        break;
                    case LineTypes.Property.ArgumentTypes.Float:
                        parameters[i - 1] = ParseFloat(args[i]);
                        break;
                    case LineTypes.Property.ArgumentTypes.Boolean:
                        parameters[i - 1] = ParseBool(args[i]);
                        break;
                    case LineTypes.Property.ArgumentTypes.Color:
                        parameters[i - 1] = ParseColor(args[i]);
                        break;
                    case LineTypes.Property.ArgumentTypes.Vector2:
                        parameters[i - 1] = ParseVector2(args[i]);
                        break;
                    case LineTypes.Property.ArgumentTypes.Vector3:
                        parameters[i - 1] = ParseVector3(args[i]);
                        break;
                    case LineTypes.Property.ArgumentTypes.TexturePath:
                        parameters[i - 1] = LoadTexture(Path.Trim() + "/" + args[i].Trim());
                        break;
                    case LineTypes.Property.ArgumentTypes.String:
                        parameters[i - 1] = args[i].Trim();
                        break;
                    default:
                        throw new ParserExceptions.UnknownLineTypeException();
                }
            }

            return new LineTypes.Property(contextProperty.name, parameters);
        }
        #endregion

        #region Data Parsing
        static byte ParseByte (string line) {
            byte temp;
            if (!byte.TryParse(line, out temp)) throw new Exception("Failed parsing \"" + line.Trim() + "\" into byte");
            return temp;
        }
        static int ParseInt (string line) {
            int temp;
            if (!int.TryParse(line, out temp)) throw new Exception("Failed parsing \"" + line.Trim() + "\" into integer");
            return temp;
        }
        static float ParseFloat (string line) {
            float temp;
            if (!float.TryParse(line, out temp)) throw new Exception("Failed parsing \"" + line.Trim() + "\" into float");
            return temp;
        }
        static bool ParseBool (string line) {
            bool temp;
            if (!bool.TryParse(line, out temp)) throw new Exception("Failed parsing \"" + line.Trim() + "\" into boolean");
            return temp;
        }
        static Vector2 ParseVector2 (string line) {
            string[] args = line.Split(',');
            if (args.Length != 2) throw new ParserExceptions.WrongLineFormatException("Vector2", "Vector2 = X, Y");
            return new Vector2(ParseFloat(args[0]), ParseFloat(args[1]));
        }
        static Vector3 ParseVector3 (string line) {
            string[] args = line.Split(',');
            if (args.Length != 3) throw new ParserExceptions.WrongLineFormatException("Vector3", "Vector3 = X, Y, Z");
            return new Vector3(ParseFloat(args[0]), ParseFloat(args[1]), ParseFloat(args[2]));
        }
        static Color ParseColor (string line) {
            string[] args = line.Split(',');
            if (args.Length != 4) throw new ParserExceptions.WrongLineFormatException("Color", "Color = R, G, B, A");
            return new Color32(ParseByte(args[0]), ParseByte(args[1]), ParseByte(args[2]), ParseByte(args[3]));
        }
        static int ParseEnum (string line, Type type) {
            try {
                return (int)Enum.Parse(type, line);
            }
            catch (Exception) {
                throw new Exception("Failed parsing \"" + line.Trim() + "\" into enumeration type " + type.Name);
            }
        }
        static int ParseEnum (string line, params string[] enumMembers) {
            return ParseEnumArray(line, enumMembers);
        }
        static int ParseEnumArray (string line, string[] enumMembers) {
            string enumMembersCombined = "";
            for (int i = 0; i < enumMembers.Length; i++) {
                if (line.Trim().ToLower() == enumMembers[i].ToLower()) return i;

                enumMembersCombined += enumMembers[i];
                if (i + 1 != enumMembers.Length) enumMembersCombined += ", ";
            }
            throw new Exception("Error parsing \"" + line.Trim() + "\", valid enumeration values are: " + enumMembersCombined);
        }
        #endregion

        #region Data Loading
        static void CheckFileExists (string fullPath) {
            if (!File.Exists(fullPath)) throw new Exception("File not found: " + fullPath);
        }
        static Texture2D LoadTexture (string path) {
            CheckFileExists(path);

            if (path.ToLower().Contains(".jpg") || path.ToLower().Contains(".png")) {
                Texture2D tempTex = new Texture2D(2, 2);
                tempTex.LoadImage(File.ReadAllBytes(path));
                return tempTex;
            }
            throw new ParserExceptions.WrongFileFormatException(path, ".jpg, .png");
        }
        #endregion

        #region Base Implementation
        private string Path;
        private string Filename;
        public Parser (string path) {
            this.Path = path;
            this.Filename = "file.cfg";
            SetContext();
        }

        internal ParserContext context;
        internal ParserContext objectContext;
        
        internal virtual void SetContext () {

            this.context = new ParserContext(
                false,
                new LineTypes.Property[0],
                new LineTypes.Component[0],
                ParserContext.ContextType.Root
                );
            

            SetObjectContext();
        }

        List<IParserLine> Lines;
        public void StartParse () {
            string fullPath = this.Path + "/" + this.Filename;
            Lines = new List<IParserLine>();

            CountingReader reader;

            try {
                reader = new CountingReader(fullPath);
            }
            catch (Exception e) {
                throw new Exception("Error opening file " + fullPath + " - ", e);
            }

            try {
                Parse(reader, this.context);
            }
            catch (Exception e) {
                reader.Close();

                string[] data = ParserLogger.GetFileData(fullPath);
                string joinedData = "";
                for (int i = 0; i < data.Length; i++) {
                    joinedData += data[i];
                }
                ParserLogger.TempData = joinedData;

                throw new Exception("Error reading file " + fullPath + " - Line " + reader.GetCurrentLine + " - ", e);
            }
            finally {
                reader.Close();
            }
        }
        internal void Parse (CountingReader reader, ParserContext context) {

            while (!reader.EndOfStream) {

                string line = reader.ReadLine();
                line = line.Trim();
                LineType type = GetLineType(line);

                if (type == LineType.Empty || type == LineType.Comment) continue;
                if (type == LineType.Unknown) throw new ParserExceptions.UnknownLineTypeException();

                if (type == LineType.ComponentClose) {
                    if (context.Type != ParserContext.ContextType.Component)
                        throw new Exception("Syntax error - Cannot end component outside of component block");

                    Lines.Add(new LineTypes.ComponentClose());
                    return;
                }
                if (type == LineType.ObjectClose) {
                    if (context.Type != ParserContext.ContextType.Object)
                        throw new Exception("Syntax error - Cannot end object outside of object block");

                    Lines.Add(new LineTypes.ObjectClose());
                    return;
                }

                if (type == LineType.Property) {
                    if (context.Properties.Length == 0) throw new Exception("Properties are not allowed in this context. Valid members are: " + context.ListValidMembers());

                    string propName = line.Split('=')[0].Trim().ToLower();

                    bool found = false;
                    for (int i = 0; i < context.Properties.Length; i++) {
                        if (propName == context.Properties[i].name.ToLower()) {
                            found = true;
                            Lines.Add(GetProperty(line, context.Properties[i]));
                            break;
                        }
                    }

                    if (!found) throw new Exception("Invalid property member \"" + propName + "\". Valid members are: " + context.ListValidMembers());
                    continue;
                }
                if (type == LineType.Component) {
                    if (context.Components.Length == 0) throw new Exception("Components are not allowed in this context. Valid members are: "+context.ListValidMembers());

                    string compName = line.Substring(0, line.IndexOf('(')).Trim().ToLower();

                    bool found = false;
                    for (int i = 0; i < context.Components.Length; i++) {
                        if (compName == context.Components[i].name.ToLower()) {
                            found = true;
                            Lines.Add(new LineTypes.Component(context.Components[i].name, context.Components[i].context));
                            Parse(reader, context.Components[i].context);
                            break;
                        }
                    }

                    if (!found) throw new Exception("Invalid component member \"" + compName + "\". Valid members are: " + context.ListValidMembers());
                    continue;
                }
                if (type == LineType.Object) {
                    if (!context.ObjectsAllowed) throw new Exception("Objects are not allowed in this context. Valid members are: "+context.ListValidMembers());

                    string objectName = line.Substring(0, line.IndexOf('[')).Trim();
                    Lines.Add(new LineTypes.Object(objectName));
                    Parse(reader, this.objectContext);

                    continue;
                }

            }

        }

        #region Generation
        internal object Generate () {

            IParentObject rootObject = GenerateSetupObject();
            Stack<ParserContext> contextStack = new Stack<ParserContext>();
            Stack<GameObject> objectStack = new Stack<GameObject>();
            contextStack.Push(this.context);

            while (Lines.Count > 0) {
                IParserLine tempLine = Lines[0];
                ProcessLine(rootObject, contextStack, objectStack, tempLine);
                Lines.RemoveAt(0);
            }

            FinalizeObject(rootObject);

            return rootObject;
        }
        internal virtual IParentObject GenerateSetupObject () {
            return null;
        }
        private void ProcessLine (IParentObject rootObject, Stack<ParserContext> contextStack, Stack<GameObject> objectStack, IParserLine tempLine) {

            LineType type = tempLine.GetType();
            ParserContext currentContext = contextStack.Peek();

            int index = -1;

            switch (type) {
                case LineType.Property:
                    LineTypes.Property lineProperty = (LineTypes.Property)tempLine;

                    for (int i = 0; i < currentContext.Properties.Length; i++) {
                        if (currentContext.Properties[i].name == lineProperty.name) {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1) throw new Exception("Invalid member in parsed lines list");
                    ProcessProperty(index, lineProperty, rootObject, objectStack, currentContext);
                    break;
                case LineType.Component:
                    LineTypes.Component lineComponent = (LineTypes.Component)tempLine;

                    for (int i = 0; i < currentContext.Components.Length; i++) {
                        if (currentContext.Components[i].name == lineComponent.name) {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1) throw new Exception("Invalid member in parsed lines list");
                    ProcessComponent(index, lineComponent, rootObject, objectStack, contextStack, currentContext);
                    contextStack.Push(currentContext.Components[index].context);
                    break;
                case LineType.Object:
                    LineTypes.Object lineObject = (LineTypes.Object)tempLine;

                    GameObject tempGO = new GameObject();
                    tempGO.name = lineObject.name;
                    tempGO.SetActive(false);

                    if (objectStack.Count > 0) tempGO.transform.parent = objectStack.Peek().transform;
                    rootObject.AssignChild(tempGO);

                    objectStack.Push(tempGO);
                    contextStack.Push(this.objectContext);
                    break;
                case LineType.ObjectClose:
                case LineType.ComponentClose:
                    contextStack.Pop();
                    break;
                default:
                    throw new System.Exception("Invalid line type in parsed lines list");
            }
        }
        internal virtual void ProcessProperty (int index, LineTypes.Property line, IParentObject obj, Stack<GameObject> objectStack, ParserContext context) {
            // overload with switch-case and handle functionalities
            throw new Exception("Invalid member in parsed lines list");
        }
        internal virtual void ProcessComponent (int index, LineTypes.Component line, IParentObject obj, Stack<GameObject> objectStack, Stack<ParserContext> contextStack, ParserContext context) {
            // overload with switch-case and handle functionalities
            throw new Exception("Invalid member in parsed lines list");
        }
        internal virtual void FinalizeObject (IParentObject obj) {
            // overload if needed and do finishing touches to object
        }
        #endregion
        #endregion

    }
}
