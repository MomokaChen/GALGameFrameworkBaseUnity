using UnityEngine;
using System.Collections.Generic;

public class OptionsController : MonoSingleton<OptionsController>
{
    public GameObject optionItemPrefab;

    private List<OptionItem> activeOptions = new List<OptionItem>();

    /// <summary>
    /// 创建选项
    /// </summary>
    /// <param name="options"></param>
    public void CreateOptions(List<string> options)
    {
        ClearOptions();

        for (int i = 0; i < options.Count; i++)
        {
            if (!string.IsNullOrEmpty(options[i]))
            {
                GameObject optionGo = Instantiate(optionItemPrefab, transform);
                OptionItem optionItem = optionGo.GetComponent<OptionItem>();
                optionItem.OptionName.text = options[i];

                int index = i + 1;
                optionGo.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                {
                    DialogueManager.Instance.OnOptionSelected(index);
                });

                activeOptions.Add(optionItem);
            }
        }

        gameObject.SetActive(activeOptions.Count > 0);
    }

    /// <summary>
    /// 清除所有选项
    /// </summary>
    public void ClearOptions()
    {
        foreach (OptionItem item in activeOptions)
        {
            Destroy(item.gameObject);
        }
        activeOptions.Clear();
        gameObject.SetActive(false);
    }
}