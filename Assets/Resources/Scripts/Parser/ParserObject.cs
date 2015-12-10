using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FileParser {
    public partial class Parser {

        private void SetObjectContext () {
            // SUB-CONTEXTS

            #region Transform
            ParserContext transformContext = new ParserContext(
                false,
                new LineTypes.Property[] {
                    new LineTypes.Property("Position", "Position = X, Y, Z", LineTypes.Property.ArgumentTypes.Vector3),
                    new LineTypes.Property("Rotation", "Rotation = X, Y, Z", LineTypes.Property.ArgumentTypes.Vector3),
                    new LineTypes.Property("Scale", "Scale = X, Y, Z", LineTypes.Property.ArgumentTypes.Vector3),
                    // todo: add layer (first add enum handling)
                },
                new LineTypes.Component[] { },
                ParserContext.ContextType.Component
                );
            #endregion
            #region Renderer
            ParserContext rendererContext = new ParserContext(
                false,
                new LineTypes.Property[] {
                    new LineTypes.Property("Texture", "Texture = NAME = PATH", LineTypes.Property.ArgumentTypes.String, LineTypes.Property.ArgumentTypes.TexturePath),
                    new LineTypes.Property("TextureScale", "TextureScale = NAME = XSCALE, YSCALE", LineTypes.Property.ArgumentTypes.String, LineTypes.Property.ArgumentTypes.Vector2),
                    new LineTypes.Property("TextureOffset", "TextureOffset = NAME = XOFFSET, YOFFSET", LineTypes.Property.ArgumentTypes.String, LineTypes.Property.ArgumentTypes.Vector2),
                    new LineTypes.Property("Color", "Color = NAME = VALUE", LineTypes.Property.ArgumentTypes.String, LineTypes.Property.ArgumentTypes.Color),
                    new LineTypes.Property("Shader", "Shader = NAME", LineTypes.Property.ArgumentTypes.String),
                    new LineTypes.Property("Mode", "Mode = VALUE", LineTypes.Property.ArgumentTypes.String),
                },
                new LineTypes.Component[] { },
                ParserContext.ContextType.Component
                );

            #endregion
            #region Collider

            ParserContext physicsMaterialContext = new ParserContext(
                false,
                new LineTypes.Property[] { 
                    new LineTypes.Property("Friction", "Friction = VALUE", LineTypes.Property.ArgumentTypes.Float),
                    new LineTypes.Property("Bounciness", "Bounciness = VALUE", LineTypes.Property.ArgumentTypes.Float),
                },
                new LineTypes.Component[] { },
                ParserContext.ContextType.Component
                );

            ParserContext boxColliderContext = new ParserContext(
                false,
                new LineTypes.Property[] { 
                    new LineTypes.Property("Center", "Center = X, Y, Z", LineTypes.Property.ArgumentTypes.Vector3),
                    new LineTypes.Property("Size", "Size = X, Y, Z", LineTypes.Property.ArgumentTypes.Vector3),
                },
                new LineTypes.Component[] { 
                    new LineTypes.Component("Material", physicsMaterialContext)
                },
                ParserContext.ContextType.Component
                );

            ParserContext circleColliderContext = new ParserContext(
                false,
                new LineTypes.Property[] { 
                    new LineTypes.Property("Center", "Center = X, Y, Z", LineTypes.Property.ArgumentTypes.Vector3),
                    new LineTypes.Property("Radius", "Radius = VALUE", LineTypes.Property.ArgumentTypes.Float),
                },
                new LineTypes.Component[] { 
                    new LineTypes.Component("Material", physicsMaterialContext)
                },
                ParserContext.ContextType.Component
                );

            ParserContext colliderContext = new ParserContext(
                false,
                new LineTypes.Property[] { },
                new LineTypes.Component[] {
                    new LineTypes.Component("Box", boxColliderContext),
                    new LineTypes.Component("Circle", circleColliderContext)
                    // todo: add polygon collider
                },
                ParserContext.ContextType.Component
                );

            #endregion
            // todo: add lights, interaction, scripts, resources.....

            // MAIN CONTEXT

            this.objectContext = new ParserContext(
                true,
                new LineTypes.Property[0],
                new LineTypes.Component[] { 
                    new LineTypes.Component("Transform", transformContext),
                    new LineTypes.Component("Renderer", rendererContext),
                    new LineTypes.Component("Collider", colliderContext),
                },
                ParserContext.ContextType.Object
                );
        }

        internal void ObjectProcessProperty (int index, LineTypes.Property line, IParentObject obj, Stack<GameObject> objectStack, ParserContext context) {

            #region Object context
            if (context == this.objectContext) {
                // properties for object itself (none atm)
                // return;
            }
            #endregion

            #region Transform
            if (context == this.objectContext.Components[0].context) {
                switch (index) {
                    case 0: // position
                        objectStack.Peek().transform.localPosition = (Vector3)line.argumentsData[0];
                        return;
                    case 1: // rotation
                        objectStack.Peek().transform.localRotation = Quaternion.Euler((Vector3)line.argumentsData[0]);
                        return;
                    case 2: // scale
                        objectStack.Peek().transform.localScale = (Vector3)line.argumentsData[0];
                        return;
                }
            }
            #endregion

            #region Renderer
            if (context == this.objectContext.Components[1].context) {

                MeshRenderer renderer = objectStack.Peek().GetComponent<MeshRenderer>();

                switch (index) {
                    case 0: // texture
                        renderer.sharedMaterial.SetTexture(line.argumentsData[0].ToString(), (Texture2D)line.argumentsData[1]);
                        return;
                    case 1: // texture scale
                        renderer.sharedMaterial.SetTextureScale(line.argumentsData[0].ToString(), (Vector2)line.argumentsData[1]);
                        return;
                    case 2: // texture offset
                        renderer.sharedMaterial.SetTextureOffset(line.argumentsData[0].ToString(), (Vector2)line.argumentsData[1]);
                        return;
                    case 3: // color
                        renderer.sharedMaterial.SetColor(line.argumentsData[0].ToString(), (Color)line.argumentsData[1]);
                        return;
                    case 4: // shader
                        renderer.sharedMaterial = new Material(Shader.Find(line.argumentsData[0].ToString()));
                        return;
                    case 5: // rendering mode
                        string mode = line.argumentsData[0].ToString().Trim().ToLower();
                        switch (mode) {
                            case "opaque":
                                renderer.sharedMaterial.SetFloat("_Mode", 0);
                                break;
                            case "cutout":
                                renderer.sharedMaterial.SetFloat("_Mode", 1);
                                break;
                            case "fade":
                                renderer.sharedMaterial.SetFloat("_Mode", 2);
                                break;
                            case "transparent":
                                renderer.sharedMaterial.SetFloat("_Mode", 3);
                                break;
                            default:
                                throw new ParserExceptions.ParserEnumException(mode, "Opaque", "Cutout", "Fade", "Transparent");
                        }
                        return;
                    default:
                        throw new Exception("Invalid member in parsed lines list");
                }
            }
            #endregion

            // todo: other contexts

            throw new Exception("Invalid member in parsed lines list");
        }

        internal void ObjectProcessComponent (int index, LineTypes.Component line, IParentObject obj, Stack<GameObject> objectStack, Stack<ParserContext> contextStack, ParserContext context) {

            #region Object context
            if (context == this.objectContext) {
                switch (index) {
                    case 0:
                    case 2:
                        return;
                    case 1:
                        MeshFilter filter = objectStack.Peek().AddComponent<MeshFilter>();
                        MeshRenderer renderer = objectStack.Peek().AddComponent<MeshRenderer>();
                        filter.mesh = Utility.MeshHelper.GetQuad();
                        renderer.material = new Material(Shader.Find("Standard"));
                        return;
                }
            }
            #endregion

            if (context == this.objectContext.Components[2].context) {
                // COLLIDER

                // todo: this
            }

            // other components that have sub-components

            throw new Exception("Invalid member in parsed lines list");
        }
    }
}
