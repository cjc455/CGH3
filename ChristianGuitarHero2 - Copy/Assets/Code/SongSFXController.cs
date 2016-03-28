using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System.Collections.Generic;
using Song;

public class SongSFXValue
{
    SongSFXType type;
    float magnitude;
    float startTimeInSong;
    float lengthTime;
    MathFunction function;

    public enum MathFunction
    {
        Sine,
        SineNegative,
        Constant
    }
    private SongSFXValue(float magnitude, float startTimeInSong, float lengthTime, SongSFXType type, MathFunction function)
    {
        this.magnitude = magnitude;
        this.startTimeInSong = startTimeInSong;
        this.lengthTime = lengthTime;
        this.type = type;
        this.function = function;
    }
    public static SongSFXValue FactoryInstant(SongSFXType type, MathFunction function, float startTimeInSong, float magnitude, float lengthTime )
    {
        SongSFXValue v = new SongSFXValue(magnitude, startTimeInSong, lengthTime, type, function);
        return v;
    }
    /*
    public SongSFXValue FactoryWait(SongSFXType type, float magnitude, float lengthTime, float startTimeInSong)
    {
        SongSFXValue v = new SongSFXValue(magnitude, Time.time, lengthTime, type);
        return v;
    }
    */
    public SongSFXType GetSFXType()
    {
        return type;
    }
    public float GetValue()
    {
        //float timeInSong = song.GetTimeInSong();
        float t = ((GetTime()) / lengthTime * Mathf.PI);
        float v = 0;
        switch (function)
        {
            case MathFunction.Sine:
                v = Mathf.Sin(t) * magnitude;
                break;
            case MathFunction.SineNegative:
                v = Mathf.Sin(t) * -magnitude;
                break;
            case MathFunction.Constant:
                float fadeSpeed = .1f;
                if(t <= fadeSpeed)
                {
                    v = Mathf.Sin(t / fadeSpeed) * magnitude;
                }
                else if (t >= lengthTime - fadeSpeed )
                {
                    v = Mathf.Sin(t / fadeSpeed) * magnitude;
                }
                else
                {
                    v = magnitude;
                }
                
                break;
        }
        
        return v;
    }
    public bool Expired()
    {
        if(MSingleton.GetSingleton<Song.Song>().GetTimeInSong() >= startTimeInSong + lengthTime)
        {
            Debug.Log("Expired at " + GetTime().ToString() + " out of " + lengthTime.ToString() + " start + " + startTimeInSong.ToString());
            return true;
        }
        return false;
    }
    float GetTime()
    {
        float result = MSingleton.GetSingleton<Song.Song>().GetTimeInSong() - startTimeInSong;

        return result;
    }
   
}
public enum SongSFXType
{
    Bloom,
    Twirl,
    GlobalTrailScale,
    GlobalLight
}
public class SongSFX
{
    List<SongSFXValue> values;
    SongSFXType type;
    float baseValue;
   
    public SongSFX(SongSFXType type)
    {
        values = new List<SongSFXValue>();
        this.type = type;
        this.baseValue = MSingleton.GetSingleton<SongSFXController>().GetValue(type);
        
    }
    public SongSFXType GetSFXType()
    {
        return type;
    }
    public void AddValue(SongSFXValue value)
    {
        values.Add(value);
    }
    public float GetValue()
    {
        float actualValue = baseValue;
       
        for(int i = 0; i < values.Count; i++)
        {
            if (values[i].Expired())
            {
                values.RemoveAt(i);
            }
            else
            {
                actualValue += values[i].GetValue();
            }
           
        }
        return actualValue;
    }
}
public class SongSFXController : MonoBehaviour {

    public Light globalLight;
    public GameObject globalTrailScale;
    public GameObject[] trailScale;
    public Camera mainCamera;

    List<SongSFX> sfx;
    //List<SongSFXValue> values;
    
    // Use this for initialization
    void Start () {
        sfx = new List<SongSFX>();
        sfx.Add(new SongSFX(SongSFXType.Bloom)) ;
        sfx.Add(new SongSFX(SongSFXType.Twirl));
        sfx.Add(new SongSFX(SongSFXType.GlobalTrailScale));
        sfx.Add(new SongSFX(SongSFXType.GlobalLight));
    }
	public void AddSFX( SongSFXValue value)
    {
        SongSFX s = sfx.Find(x => x.GetSFXType() == value.GetSFXType());
        s.AddValue(value);
    }
    
    public float GetValue(SongSFXType type)
    {
        float result = 0;
        switch (type)
        {
            case SongSFXType.Bloom:
                result = Camera.main.GetComponent<Bloom>().bloomThreshold;
                break;
            case SongSFXType.GlobalLight:
                result = globalLight.intensity;
                break;
            case SongSFXType.GlobalTrailScale:
                result = 1f;
                break;
            case SongSFXType.Twirl:
                result = 0f;
                break;
        }
        return result;
    }
    void UpdateSFX()
    {
       // switch
       
        foreach(SongSFX s in sfx)
        {
            float actualValue = s.GetValue();
            switch (s.GetSFXType())
            {
                case SongSFXType.Bloom:
                    Camera.main.GetComponent<Bloom>().bloomThreshold =  actualValue;
                    break;
                case SongSFXType.GlobalLight:
                    globalLight.intensity = actualValue;
                    break;
                case SongSFXType.GlobalTrailScale:
                    globalTrailScale.transform.localScale = new Vector3(1,1,1) * actualValue;
                    break;
                case SongSFXType.Twirl:
                    Camera.main.GetComponent<Twirl>().angle = actualValue;
                    break;

            }

        }
    }

	// Update is called once per frame
	void Update () {
        UpdateSFX();
        
    }
   
}
