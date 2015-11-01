using UnityEngine;
using System.Collections;

public class ProceduralManager : MonoBehaviour {

    public GameObject QuadRef;

	void Start () {
        StarSystem a = SystemGenerator.Generate();

        for (int i = 0; i < a.Planets.Count; i++) {
            GameObject temp = Instantiate(QuadRef, new Vector3(i * 6, 0, 0), Quaternion.Euler(90, 0, 0)) as GameObject;
            temp.GetComponent<MeshRenderer>().material.mainTexture = a.Planets[i].surfaceTexture;
        }
	}
	
}
