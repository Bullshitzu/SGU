using UnityEngine;
using System.Collections;

namespace Spaceship {
    public class ShipTilePrefab : FileParser.IParentObject {

        public string ID { get; set; }
        public string EditorName { get; set; }
        public string EditorDescription { get; set; }
        public Texture2D EditorThumbnail { get; set; }
        public float Mass { get; set; }
        public int HP { get; set; }
        public GameObject GORef { get; set; }
        // todo: add a tiling/size system info

        public ShipTilePrefab () { }

        public void AssignChild (GameObject obj) {
            this.GORef = obj;
        }

        // for debugging
        public override string ToString () {
            return ("ID: " + ID + ", Name: " + EditorName + ", Description: " + EditorDescription + ", Thumbnail assigned: " + (EditorThumbnail != null) + ", Mass: " + Mass + ", HP: " + HP + ", Object assigned: " + (GORef != null));
        }
    }
}
