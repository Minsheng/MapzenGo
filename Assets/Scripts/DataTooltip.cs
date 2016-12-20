using UnityEngine;
using UnityEngine.UI;

public class DataTooltip : MonoBehaviour {
    public string displayText;
    public int fontSize = 14;
    public Color fontColor = Color.blue;
    public Color containerColor = Color.black;

    public void Reset()
    {
        SetContainer();
        SetText("UITextFront");
        SetText("UITextReverse");
    }

    // Use this for initialization
    void Start() {
        Reset();
    }

    private void SetContainer()
    {
        var tmpContainer = transform.FindChild("DataTooltipCanvas/UIContainer");
        tmpContainer.GetComponent<Image>().color = containerColor;
    }

    private void SetText(string name)
    {
        var tmpText = transform.FindChild("DataTooltipCanvas/" + name).GetComponent<Text>();
        tmpText.material = Resources.Load("UIText") as Material;
        tmpText.color = fontColor;
        tmpText.fontSize = fontSize;
    }

    void Update()
    {
        Reset();
    }
}
