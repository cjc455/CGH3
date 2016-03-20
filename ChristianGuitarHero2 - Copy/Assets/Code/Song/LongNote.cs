using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Song
{

    public class LongNote : MonoBehaviour
    {

        void Start()
        {
            GameObject longNoteControllerGameObject = new GameObject();
            longNoteControllerGameObject.AddComponent<LongNoteController>();
            longNoteControllerGameObject.GetComponent<LongNoteController>().Initialize(GetComponent<LongNote>());
        }
    }
}