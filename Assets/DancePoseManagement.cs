using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DancePoseManagement : MonoBehaviour
{
    public RawImage refPoseDisplay;
    public Text userPoseValue;
    public Text refPoseValue;
    public Text comparisonValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        userPoseValue.text = Utlity.USER_POSE;
        PoseSimilarity(Utlity.REF_POSE_VEC, Utlity.USER_POSE_VEC);
        comparisonValue.text = (100.0f - Utlity.POSE_RESULT).ToString("F2") + "%";
        if (Utlity.POSE_RESULT > 50)
        {
            comparisonValue.color = Color.red;
        }
        else if (Utlity.POSE_RESULT > 30)
        {
            comparisonValue.color = new Color(255, 102, 0);
        }
        else if (Utlity.POSE_RESULT > 10)
        {
            comparisonValue.color = new Color(255, 187, 0);
        }
        else
        {
            comparisonValue.color = Color.green;
        }

    }


    public void ChangeRefPoseValue(string poseValue)
    {
        refPoseValue.text = poseValue;
        ConvertStringToList(ref Utlity.REF_POSE_VEC, poseValue);
    }

    public void ChangePose(Texture curImage)
    {
        refPoseDisplay.texture = curImage;
    }

    bool ConvertStringToList(ref List<float> poseList, string data)
    {
        poseList = new List<float>();
        string[] jointAngles = data.Split('-');
        if (data == "")
        {
            return false;
        }
        for (int i = 0; i < jointAngles.Length; i++)
        {
            poseList.Add(float.Parse(jointAngles[i].Split(':')[1].Trim()));
        }
        return true;
    }

    float PoseSimilarity(List<float> refAngleList, List<float> userAngleList)
    {
        if (refAngleList.Count != userAngleList.Count)
        {
            return 0.0f;
        }
        float result = 0.0f;

        //float top = 0.0f;
        //float bot1 = 0.0f;
        //float bot2 = 0.0f;

        float difValue = 0.0f;
        //string totalDifValue = "";
        for (int i = 0; i < refAngleList.Count; i++)
        {
            //top += (refAngleList[i] * userAngleList[i]);
            //bot1 += Mathf.Pow(refAngleList[i], 2);
            //bot2 += Mathf.Pow(userAngleList[i], 2);
            difValue = Mathf.Abs(refAngleList[i] - userAngleList[i]);
            result += difValue;
            //totalDifValue += difValue.ToString("F2") + " ";
        }

        //result = top / (Mathf.Sqrt(bot1) * Mathf.Sqrt(bot2));
        result /= refAngleList.Count;
        //Debug.Log(totalDifValue);


        Utlity.POSE_RESULT = result;
        return result;
    }

    //// Great npm package for computing cosine similarity  
    //const similarity = require('compute-cosine-similarity');

    //// Cosine similarity as a distance function. The lower the number, the closer // the match
    //// poseVector1 and poseVector2 are a L2 normalized 34-float vectors (17 keypoints each  
    //// with an x and y. 17 * 2 = 32)
    //function cosineDistanceMatching(poseVector1, poseVector2)
    //{
    //    let cosineSimilarity = similarity(poseVector1, poseVector2);
    //    let distance = 2 * (1 - cosineSimilarity);
    //    return Math.sqrt(distance);
    //}

    //function weightedDistanceMatching(poseVector1, poseVector2)
    //{
    //    let vector1PoseXY = poseVector1.slice(0, 34);
    //    let vector1Confidences = poseVector1.slice(34, 51);
    //    let vector1ConfidenceSum = poseVector1.slice(51, 52);

    //    let vector2PoseXY = poseVector2.slice(0, 34);

    //    // First summation
    //    let summation1 = 1 / vector1ConfidenceSum;

    //    // Second summation
    //    let summation2 = 0;
    //    for (let i = 0; i < vector1PoseXY.length; i++)
    //    {
    //        let tempConf = Math.floor(i / 2);
    //        let tempSum = vector1Confidences[tempConf] * Math.abs(vector1PoseXY[i] - vector2PoseXY[i]);
    //        summation2 = summation2 + tempSum;
    //    }

    //    return summation1 * summation2;
    //}
}
