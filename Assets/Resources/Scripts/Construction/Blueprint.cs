using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Blueprint : MonoBehaviour {

    public List<Constructable> components;

    void Start () {
        components = new List<Constructable>();
    }

    public void Display () {
        // todo: graphics.draw on each (unfinished) component to display blueprint...
    }

}
