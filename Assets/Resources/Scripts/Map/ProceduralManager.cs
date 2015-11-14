using UnityEngine;
using System.Collections;

public class ProceduralManager : MonoBehaviour {

    public GameObject QuadRef;

	void Start () {
        SystemGenerator.Generate();
	}
	
}
