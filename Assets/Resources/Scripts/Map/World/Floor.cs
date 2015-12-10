using UnityEngine;
using System.Collections;

public class Floor : FileParser.IParentObject {

    public string name;
    public Texture2D diffuse;
    public Texture2D normal;

    //      friction mult
    //      footstep sound
    //      tire tracks
    //      etc...

    public Floor () { }

    public Floor (string name, Texture2D diffuse, Texture2D normal) {
        this.name = name;
        this.diffuse = diffuse;
        this.normal = normal;
    }

    public void AssignChild (GameObject obj) {
        // do nothing
    }

    public override string ToString () {
        return "Floor " + this.name + ", Diffuse assigned: " + (this.diffuse != null).ToString() + ", Normal assigned: " + (this.normal != null).ToString();
    }
}
