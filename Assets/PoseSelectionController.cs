using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PoseSelectionController : MonoBehaviour
{
    public Dictionary<string, Texture2D> listRefPoseImages;
    public Dictionary<string, string> listRefPoseValue;
    private GameObject dancePoseManagement;
    private GameObject nextButton;
    private GameObject prevButton;
    private Dropdown poseSelector;
    private int poseIndex = 0;
    public List<Texture2D> refPoseImages;
    // Start is called before the first frame update
    void Start()
    {
        poseIndex = 0;
        dancePoseManagement = GameObject.Find("DancePoseManager");
        nextButton = GameObject.Find("Next");
        nextButton.GetComponent<Button>().onClick.AddListener(nextPose);
        prevButton = GameObject.Find("Prev");
        prevButton.GetComponent<Button>().onClick.AddListener(prevPose);

        poseSelector = GetComponent<Dropdown>();
        poseSelector.onValueChanged.AddListener(delegate { OnValueChanged(poseSelector); });

        InitializeOptions();
    }

    void InitializeOptions()
    {
        listRefPoseImages = new Dictionary<string, Texture2D>();
        listRefPoseValue = new Dictionary<string, string>();
        for (int i = 0; i < refPoseImages.Count; i++)
        {
            string labelPose = "Pose " + i.ToString();
            AddPose(labelPose, refPoseImages[i]);
        }

        for (int i = 0; i < refPoseImages.Count; i++)
        {
            string labelPose = "Pose " + i.ToString();
            listRefPoseValue.Add(labelPose, Utlity.REF_POSE_VALUE[i]);
        }
        poseSelector = GetComponent<Dropdown>();
        poseSelector.ClearOptions();
        poseSelector.AddOptions(listRefPoseImages.Select(pair => pair.Key).ToList());

        OnValueChanged(poseSelector);
    }

    void OnValueChanged(Dropdown dropdown)
    {
        var option = dropdown.options[dropdown.value];
        poseIndex = dropdown.value;
        Texture2D refImage = listRefPoseImages[option.text];
        string refValue = listRefPoseValue[option.text];

        dancePoseManagement.GetComponent<DancePoseManagement>().ChangePose(refImage);
        dancePoseManagement.GetComponent<DancePoseManagement>().ChangeRefPoseValue(refValue);
    }

    void AddPose(string label, Texture2D pose)
    {
        if (pose != null)
        {
            listRefPoseImages.Add(label, pose);
        }
    }

    void nextPose()
    {
        poseIndex++;
        if (poseIndex < poseSelector.options.Count)
        {
            poseSelector.value = poseIndex;
        }
        else
        {
            poseIndex--;
        }
    }

    void prevPose()
    {
        poseIndex--;
        if (poseIndex >= 0)
        {
            poseSelector.value = poseIndex;
        }
        else
        {
            poseIndex++;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
