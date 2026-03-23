using TMPro;
using UnityEngine;

public class DefeatPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bonesText;
    void Start()
    {
        
    }

    void Update()
    {
        bonesText.text = "Bones Collected: " + FloorTracker.runCollectedBones.ToString();   
    }
}
