using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionItem : MonoBehaviour
{
    public TMP_Text OptionName;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    /// <summary>
    /// 设置选项文本和点击事件
    /// </summary>
    /// <param name="text"></param>
    /// <param name="callback"></param>
    public void Setup(string text, System.Action callback)
    {
        OptionName.text = text;
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => callback?.Invoke());
        }
    }
}