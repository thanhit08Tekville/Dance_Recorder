using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Texture2D> frames;
    int framesPerSecond = 2;
    bool playRef = true;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!playRef)
        {
            return;
        }
        int index = (int)((Time.time * framesPerSecond) % frames.Count);
        this.GetComponent<RawImage>().texture = frames[index];
        if (index == (frames.Count - 1))
        {
            playRef = false;
        }

    }
}
