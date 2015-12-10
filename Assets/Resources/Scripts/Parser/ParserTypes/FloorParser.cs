using UnityEngine;
using System.Collections;

namespace FileParser {
    namespace Parsers {
        public class FloorParser : Parser {

            public FloorParser (string path) : base(path) { }

            internal override void SetContext () {
                base.SetContext();

                this.context = new ParserContext(
                    false,
                    new LineTypes.Property[] { 
                        new LineTypes.Property("Name", "Name = VALUE", LineTypes.Property.ArgumentTypes.String),
                        new LineTypes.Property("Diffuse", "Diffuse = PATH", LineTypes.Property.ArgumentTypes.TexturePath),
                        new LineTypes.Property("Normal", "Normal = PATH", LineTypes.Property.ArgumentTypes.TexturePath),
                        // todo: friction and stuff, maybe references to footstep sounds....
                    },
                    new LineTypes.Component[] { },
                    ParserContext.ContextType.Root
                    );
            }

            internal override IParentObject GenerateSetupObject () {
                return new Floor();
            }

            internal override void ProcessProperty (int index, LineTypes.Property line, IParentObject obj, System.Collections.Generic.Stack<GameObject> objectStack, ParserContext context) {
                Floor tempObj = (Floor)obj;
                switch (index) {
                    case 0:
                        tempObj.name = line.argumentsData[0].ToString();
                        break;
                    case 1:
                        tempObj.diffuse = (Texture2D)line.argumentsData[0];
                        break;
                    case 2:
                        tempObj.normal = (Texture2D)line.argumentsData[0];
                        break;
                }
            }

            internal override void FinalizeObject (IParentObject obj) {
                Floor tempObj = (Floor)obj;

                if (tempObj.name == "") throw new System.Exception("Name not assigned");
                if (tempObj.diffuse == null) throw new System.Exception("Diffuse texture not assigned");
                if (tempObj.normal == null) throw new System.Exception("Normal map not assigned");
            }
        }
    }
}
