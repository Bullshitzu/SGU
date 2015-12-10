using UnityEngine;
using System.Collections;

public partial class Utility {
    public class MeshHelper : MonoBehaviour {

        static MeshHelper () {
            GameObject tempGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quadMesh = tempGO.GetComponent<MeshFilter>().sharedMesh;
            DestroyObject(tempGO);
        }

        static Mesh quadMesh;
        public static Mesh GetQuad () {
            return quadMesh;
        }

    }
}
