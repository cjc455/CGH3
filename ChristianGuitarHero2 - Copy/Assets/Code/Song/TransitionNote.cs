using UnityEngine;
using System.Collections;

namespace Song
{
    public class TransitionNote : MonoBehaviour
    {


        Note note;
        // Use this for initialization
        void Start()
        {
            note = GetComponent<Note>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(note.GetTrail().keyCode))
            {

            }
            

        }
    }
}