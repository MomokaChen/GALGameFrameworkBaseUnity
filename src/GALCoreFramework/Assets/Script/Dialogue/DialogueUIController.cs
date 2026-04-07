using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoSingleton<DialogueUIController>
{
    public Image CharPosRight;
    public Image CharPosLeft;
    public Image CharPosMid;
    public Image DialogueFrame;
    public TMP_Text Name;
    public TMP_Text Content;
    public OptionsController Options;

    private Coroutine typingCoroutine;
    private string currentFullText;
    [Range(0.1f,0.01f)]public float typingSpeed = 0.05f;

    private void Start()
    {
        DialogueFrame.GetComponent<Button>().onClick.AddListener(OnDialogueFrameClicked);
        HideOptions();
    }

    /// <summary>
    /// 更新UI方法
    /// </summary>
    /// <param name="node"></param>
    public void UpdateDialogueUI(DialogDefine node)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (!string.IsNullOrEmpty(node.speaker))
        {
            Name.text = node.speaker;
            Name.gameObject.SetActive(true);
        }
        else
        {
            Name.gameObject.SetActive(false);
        }

        UpdateAllCharacter(node);
        HideOptions();

        currentFullText = node.content;
        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        Content.text = "";
        StringBuilder sb = new StringBuilder();

        foreach (char c in currentFullText)
        {
            sb.Append(c);
            Content.text = sb.ToString();
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null;
    }

    /// <summary>
    /// 下一步
    /// </summary>
    private void OnDialogueFrameClicked()
    {
        if (typingCoroutine != null)
        {
            // 正在打字时点击，显示全部文本
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            Content.text = currentFullText;
        }
        else
        {
            // 已经显示全部文本，进入下一个对话
            DialogueManager.Instance.OnContinueClicked();
        }
    }

    /// <summary>
    /// 更新所有立绘
    /// </summary>
    /// <param name="node"></param>
    private void UpdateAllCharacter(DialogDefine node)
    {
        UpdateCharacter(CharPosLeft, node.leftcharurl);
        UpdateCharacter(CharPosMid, node.midcharurl);
        UpdateCharacter(CharPosRight, node.rightcharurl);
    }

    /// <summary>
    /// 立绘更换方法
    /// </summary>
    /// <param name="image"></param>
    /// <param name="url"></param>
    private void UpdateCharacter(Image image, string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            Sprite sprite = Resources.Load<Sprite>(url);
            if (sprite != null)
            {
                image.sprite = sprite;
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
        else
        {
            image.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 显示选项方法
    /// </summary>
    /// <param name="node"></param>
    public void ShowOptions(DialogDefine node)
    {
        List<string> options = new List<string>();

        // 收集所有非空选项
        if (!string.IsNullOrEmpty(node.option1) && node.option1 != "0")
        {
            options.Add(node.option1);
        }

        if (!string.IsNullOrEmpty(node.option2) && node.option2 != "0")
        {
            options.Add(node.option2);
        }

        if (!string.IsNullOrEmpty(node.option3) && node.option3 != "0")
        {
            options.Add(node.option3);
        }

        if (!string.IsNullOrEmpty(node.option4) && node.option4 != "0")
        {
            options.Add(node.option4);
        }

        if (options.Count > 0)
        {
            Options.CreateOptions(options);
        }
    }

    /// <summary>
    /// 隐藏选项
    /// </summary>
    private void HideOptions()
    {
        Options.ClearOptions();
    }

    /// <summary>
    /// 显示对话框
    /// </summary>
    public void ShowDialogueUI()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏对话框
    /// </summary>
    public void HideDialogueUI()
    {
        gameObject.SetActive(false);
    }
}
