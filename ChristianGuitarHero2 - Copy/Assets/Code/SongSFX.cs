using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using Song;

public class SongSFX : MonoBehaviour {

    public GameObject globalScale;
    public Light globalLight;
    
    NoteEntry globalLightNoteEntry;
    NoteEntry globalScaleNoteEntry;
    NoteEntry bloomNoteEntry;
    public float scaleAmmount = .25f;
    Color emptyColor;
    float globalLightStart = 0;
    float globalScaleStart = 0;
    float cameraBloomStart = 0;
    
    // Use this for initialization
    void Start () {
        emptyColor = new Color(0, 0, 0);
        globalLight.color = emptyColor;
    }
	
	// Update is called once per frame
	void Update () {
        if(globalLightNoteEntry == null)
        {
            return;
        }
        Color c = emptyColor;
        float time = ((Time.time - globalLightStart) / globalLightNoteEntry.audioLength * Mathf.PI);
        if (time < Mathf.PI)
        {
            float result = Mathf.Sin(time) * globalLightNoteEntry.audioMagnitude;
            c = globalLightNoteEntry.globalColor * result;
            
        }
        else
        {
            globalLightNoteEntry = null;
        }
        globalLight.color = c;



        if (globalScaleNoteEntry == null)
        {
            return;
        }
        float s = 1;
        float time2 = ((Time.time - globalScaleStart) / globalScaleNoteEntry.audioLength * Mathf.PI);
        if (time2 < Mathf.PI)
        {
            s = s + (scaleAmmount * Mathf.Sin(time2) * globalScaleNoteEntry.audioMagnitude);
            

        }
        else
        {
            globalScaleNoteEntry = null;
        }
        globalScale.transform.localScale = new Vector3(1, 1, 1) * s;

        UpdateBloom();
    }
    private void UpdateBloom()
    {
        if (bloomNoteEntry == null)
        {
            return;
        }
        float s = .45f;
        float time2 = ((Time.time - globalScaleStart) / bloomNoteEntry.audioLength * Mathf.PI);
        if (time2 < Mathf.PI)
        {
            s = s - (bloomNoteEntry.bloomAmmount * Mathf.Sin(time2));


        }
        else
        {
            globalScaleNoteEntry = null;
        }
        Camera.main.GetComponent<Bloom>().bloomThreshold = s;
    }
    public void GlobalLight(NoteEntry noteEntry)
    {
        globalLightStart = Time.time;
        globalLightNoteEntry = noteEntry;
    }
    public void GlobalScale(NoteEntry noteEntry)
    {
        globalScaleStart = Time.time;
        globalScaleNoteEntry = noteEntry;
    }
    public void Bloom(NoteEntry noteEntry)
    {
        if(bloomNoteEntry != null)
        {
            return;
        }
        cameraBloomStart = Time.time;
        bloomNoteEntry = noteEntry;
    }
}
