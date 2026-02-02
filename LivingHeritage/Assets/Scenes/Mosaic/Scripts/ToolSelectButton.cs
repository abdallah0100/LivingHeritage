using UnityEngine;

public class ToolSelectButton : MonoBehaviour
{
    public ToolType toolType;

    public void OnClickSelect()
    {
        if (ToolSelectionManager.Instance != null)
            ToolSelectionManager.Instance.SelectTool(toolType);
    }
}
