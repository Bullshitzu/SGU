using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spaceship;
using System;

namespace FileParser {
    namespace Parsers {
        public class ShipTileParser : Parser {

            public ShipTileParser (string path) : base(path) { }

            internal override void SetContext () {
                base.SetContext();

                this.context = new ParserContext(
                    true,
                    new LineTypes.Property[] {
                    new LineTypes.Property("ID", "ID = VALUE", LineTypes.Property.ArgumentTypes.String),
                    new LineTypes.Property("EditorName", "EditorName = VALUE", LineTypes.Property.ArgumentTypes.String),
                    new LineTypes.Property("EditorDescription", "EditorDescription = VALUE", LineTypes.Property.ArgumentTypes.String),
                    new LineTypes.Property("EditorThumbnail", "EditorThumbnail = PATH", LineTypes.Property.ArgumentTypes.TexturePath),
                    new LineTypes.Property("Mass", "Mass = VALUE", LineTypes.Property.ArgumentTypes.Float),
                    new LineTypes.Property("HP", "HP = VALUE", LineTypes.Property.ArgumentTypes.Integer),
                },
                    new LineTypes.Component[] { },
                    ParserContext.ContextType.Root
                    );
            }

            internal override IParentObject GenerateSetupObject () {
                return new ShipTilePrefab();
            }

            internal override void ProcessComponent (int index, LineTypes.Component line, IParentObject obj, Stack<GameObject> objectStack, Stack<ParserContext> contextStack, ParserContext context) {

                if (objectStack.Count > 0) {
                    base.ObjectProcessComponent(index, line, obj, objectStack, contextStack, context);
                    return;
                }

                throw new Exception("Invalid member in parsed lines list");
            }

            internal override void ProcessProperty (int index, LineTypes.Property line, IParentObject obj, Stack<GameObject> objectStack, ParserContext context) {

                if (objectStack.Count > 0) {
                    base.ObjectProcessProperty(index, line, obj, objectStack, context);
                    return;
                }

                if (context == this.context) {
                    ShipTilePrefab tile = (ShipTilePrefab)obj;

                    switch (index) {
                        case 0:
                            tile.ID = line.argumentsData[0].ToString();
                            break;
                        case 1:
                            tile.EditorName = line.argumentsData[0].ToString();
                            break;
                        case 2:
                            tile.EditorDescription = line.argumentsData[0].ToString();
                            break;
                        case 3:
                            tile.EditorThumbnail = (Texture2D)line.argumentsData[0];
                            break;
                        case 4:
                            tile.Mass = (float)line.argumentsData[0];
                            break;
                        case 5:
                            tile.HP = (int)line.argumentsData[0];
                            break;
                    }
                    return;
                }

                throw new Exception("Invalid member in parsed lines list");
            }
        }
    }
}
