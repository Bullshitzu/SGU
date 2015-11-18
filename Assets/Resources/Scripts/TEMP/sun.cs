using UnityEngine;
using System.Collections;

public class sun : MonoBehaviour {

    public static float t = 0;
    public float speed = 1f;

	void Update () {

        float t1 = (t * 180f) * speed;
        this.transform.rotation = Quaternion.Euler(45, t1, 0);

	}
}
