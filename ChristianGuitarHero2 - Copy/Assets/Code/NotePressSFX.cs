using UnityEngine;
using System.Collections;

public class NotePressSFX : MonoBehaviour {

    public float transparencyTime;
    public float scaleTime;
    public float finalScale;
    public float deathTime;
    float startTime;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        Set(0);
	}
	
    void Update()
    {
        Set(Time.time - startTime);
    }
	// Update is called once per frame
	void Set (float t) {

        if (t >= deathTime)
            Destroy(this.gameObject);

        float scale = finalScale * ((t) / scaleTime);
        transform.localScale = new Vector3(scale, scale, scale);

        float transparency =1 - ((t) / transparencyTime);
        transparency = Mathf.Clamp(transparency, 0f, 1f);
        Color color = GetComponent<Renderer>().material.color;
        color.a = transparency;
        GetComponent<Renderer>().material.color = color;
    }
}
