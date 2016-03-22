using UnityEngine;
using System.Collections;

namespace Song
{

    public class SlideNote : MonoBehaviour
    {

        void Start()
        {
            GameObject longNoteControllerGameObject = new GameObject();
            longNoteControllerGameObject.AddComponent<SlideNoteController>();
            longNoteControllerGameObject.GetComponent<SlideNoteController>().Initialize(GetComponent<SlideNote>());
        }
    }
}