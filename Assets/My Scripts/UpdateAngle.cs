using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAngle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = Utlity.ANGLE;//Utlity.ANGLE1.ToString() + " - " + Utlity.ANGLE2.ToString();
    }
}
