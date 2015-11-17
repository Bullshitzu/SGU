using UnityEngine;
using System.Collections;

public class ProceduralManager : MonoBehaviour {

    public GameObject QuadRef;

	void Start () {
        StarSystem a = SystemGenerator.Generate();

        for (int i = 0; i < a.Planets.Count; i++) {
            GameObject temp = Instantiate(QuadRef, new Vector3(i * 6, 0, 0), Quaternion.Euler(90, 0, 0)) as GameObject;
            temp.GetComponent<MeshRenderer>().material.mainTexture = a.Planets[i].textureDiffuse;

            if (a.Planets[i].textureIllumination != null) {
                temp.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", a.Planets[i].textureIllumination);
                temp.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(1, 1, 1, 1));
                temp.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            }

            temp.GetComponent<MeshRenderer>().material.SetTexture("_BumpMap", a.Planets[i].textureNormal);
            temp.GetComponent<MeshRenderer>().material.SetFloat("_BumpScale", 0.3f);
            temp.GetComponent<MeshRenderer>().material.EnableKeyword("_NORMALMAP");
        }
	}
}
