using UnityEngine;
using System.Collections;

public class ProceduralManager : MonoBehaviour {

    public GameObject QuadRef;
    Material[] mat;

	void Start () {
        StarSystem a = SystemGenerator.Generate();

        mat = new Material[a.Planets.Count];

        for (int i = 0; i < a.Planets.Count; i++) {
            GameObject temp = Instantiate(QuadRef, new Vector3(i * 6, 0, 0), Quaternion.Euler(90, 0, 0)) as GameObject;
            mat[i] = temp.GetComponent<MeshRenderer>().material;
            mat[i].mainTexture = a.Planets[i].textureDiffuse;

            if (a.Planets[i].textureIllumination != null) {
                mat[i].SetTexture("_EmissionMap", a.Planets[i].textureIllumination);
                mat[i].SetColor("_EmissionColor", new Color(1, 1, 1, 1));
                mat[i].EnableKeyword("_EMISSION");
            }

            if (a.Planets[i].textureNormal != null) {
                mat[i].SetTexture("_BumpMap", a.Planets[i].textureNormal);
                mat[i].SetFloat("_BumpScale", a.Planets[i].Blueprint.NormalMult);
                mat[i].EnableKeyword("_NORMALMAP");
            }
        }
	}

    /*
     * _NORMALMAP (Normal Mapping)
     * _ALPHATEST_ON (“Cut out” Transparency Rendering Mode)
     * _ALPHABLEND_ON (“Fade” Transparency Rendering Mode)
     * _ALPHAPREMULTIPLY_ON (“Transparent” Transparency Rendering Mode)
     * _EMISSION (Emission Colour or Emission Mapping)
     * _PARALLAXMAP (Height Mapping)
     * _DETAIL_MULX2 (Secondary “Detail” Maps (Albedo & Normal Map)
     * _METALLICGLOSSMAP (Metallic/Smoothness Mapping in Metallic Workflow)
     * _SPECGLOSSMAP (Specular/Smoothness Mapping in Specular Workflow)
     * 
    */

    float t=0;

    void Update () {

        t += Time.deltaTime * 0.1f;
        sun.t = t;

        for (int i = 0; i < mat.Length; i++) {
            mat[i].SetTextureOffset("_DetailAlbedoMap", new Vector2(t, 0));
        }
    }
}
