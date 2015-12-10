using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Constructable : MonoBehaviour {

    void Start () {
        SetupBlueprint();
    }

    public string Name;
    public List<Resource> NeededResources;
    public float ConstructionTime;

    public virtual void SetupBlueprint () {

        if (this.GetComponent<MeshRenderer>() != null) {
            this.GetComponent<MeshRenderer>().enabled = false;
        }

        // colliders

        // lights

        // active scripts..

    }

    public void AddResource (Resource res) {

        for (int i = 0; i < NeededResources.Count; i++) {
            if (res.type == NeededResources[i].type) {
                NeededResources[i].SetAmount(NeededResources[i].amount - res.amount);
                break;
            }
        }

        for (int i = 0; i < NeededResources.Count; i++) {
            if (NeededResources[i].amount > 0) return;
        }

        EndConstruction();

    }

    public virtual void EndConstruction () {
        // activate stuff, turn it into a functional object..
    }

}
