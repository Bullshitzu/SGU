using UnityEngine;
using System.Collections;

public struct Resource {

    public enum Type {
        Copper,
        Iron,
        Whatever
    }

    public Type type;
    public float amount;

    public Resource (Type type, float amount) {
        this.type = type;
        this.amount = amount;
    }

    public void SetAmount (float val) {
        this.amount = val;
    }

}
