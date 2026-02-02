using UnityEngine;
using UnityEngine.UI;

public class ToolSelectionManager : MonoBehaviour
{
    public static ToolSelectionManager Instance { get; private set; }

    [System.Serializable]
    public class ToolUI
    {
        public ToolType type;

        [Tooltip("The BORDER Image (the one that should turn green/red)")]
        public Image borderImage;

        [Tooltip("Optional: the inner icon image (not required)")]
        public Image iconImage;
    }

    [Header("Tools in UI")]
    public ToolUI[] tools;

    [Header("Border Colors")]
    public Color selectedColor = Color.green;
    public Color deselectedColor = Color.red;

    [Header("Default Tool")]
    public ToolType defaultTool = ToolType.Brush;

    public ToolType CurrentTool { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SelectTool(defaultTool);
    }

    public void SelectTool(ToolType tool)
    {
        CurrentTool = tool;

        for (int i = 0; i < tools.Length; i++)
        {
            bool isSelected = (tools[i].type == tool);

            if (tools[i].borderImage != null)
                tools[i].borderImage.color = isSelected ? selectedColor : deselectedColor;
        }
    }

    public bool IsSelected(ToolType tool) => CurrentTool == tool;
}
