using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    public DialogueUIController uiController;

    private int currentNodeId;
    private int currentBranchId;
    private bool isDialogueActive = false;

    private void Awake()
    {
        DataManager.Instance.Load();
        uiController.gameObject.SetActive(false);
    }

    /// <summary>
    /// 进入对话框
    /// </summary>
    public void StartDialogue(int branchId,int nodeId)
    {
        currentBranchId = branchId;
        currentNodeId = nodeId;
        isDialogueActive = true;
        uiController.ShowDialogueUI();
        ShowCurrentNode();
    }

    /// <summary>
    /// 获取当前节点
    /// </summary>
    private void ShowCurrentNode()
    {
        DialogDefine currentNode = FindNodeByBranchAndId(currentBranchId, currentNodeId);

        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        uiController.UpdateDialogueUI(currentNode);
    }

    /// <summary>
    /// 根据branchid和id查找节点
    /// </summary>
    private DialogDefine FindNodeByBranchAndId(int branchId, int nodeId)
    {
        if (DataManager.Instance.dialogData.ContainsKey(branchId))
        {
            if (DataManager.Instance.dialogData[branchId].ContainsKey(nodeId))
            {
                return DataManager.Instance.dialogData[branchId][nodeId];
            }
        }
        return null;
    }

    /// <summary>
    /// 点击继续
    /// </summary>
    public void OnContinueClicked()
    {
        if (!isDialogueActive) return;

        DialogDefine currentNode = FindNodeByBranchAndId(currentBranchId, currentNodeId);

        if (currentNode.islast)
        {
            if (!string.IsNullOrEmpty(currentNode.option1) ||
                !string.IsNullOrEmpty(currentNode.option2) ||
                !string.IsNullOrEmpty(currentNode.option3) ||
                !string.IsNullOrEmpty(currentNode.option4))
            {
                uiController.ShowOptions(currentNode);
            }
            else if (currentNode.nextbranchid.HasValue)
            {
                EnterNextBranch(currentNode);
            }
            else
            {
                EndDialogue();
            }
        }
        else
        {
            currentNodeId++;
            ShowCurrentNode();
        }
    }

    /// <summary>
    /// 进入下一个分支
    /// </summary>
    /// <param name="currentNode"></param>
    private void EnterNextBranch(DialogDefine currentNode)
    {
        int targetBranchId = (int)currentNode.nextbranchid.Value;
        currentBranchId = targetBranchId;
        currentNodeId = 1;
        ShowCurrentNode();
    }

    /// <summary>
    /// 选中选项
    /// </summary>
    /// <param name="optionIndex"></param>
    public void OnOptionSelected(int optionIndex)
    {
        if (!isDialogueActive) return;

        DialogDefine currentNode = FindNodeByBranchAndId(currentBranchId, currentNodeId);
        float? targetBranchId = null;

        switch (optionIndex)
        {
            case 1:
                targetBranchId = currentNode.option1next;
                break;
            case 2:
                targetBranchId = currentNode.option2next;
                break;
            case 3:
                targetBranchId = currentNode.option3next;
                break;
            case 4:
                targetBranchId = currentNode.option4next;
                break;
        }

        if (targetBranchId.HasValue && targetBranchId.Value > 0)
        {
            currentBranchId = (int)targetBranchId.Value;
            currentNodeId = 1;
        }
        else
        {
            currentNodeId++;
        }

        ShowCurrentNode();
    }

    /// <summary>
    /// 结束对话
    /// </summary>
    private void EndDialogue()
    {
        isDialogueActive = false;
        uiController.HideDialogueUI();
        DialogScenceManager.Instance.LeaveDialogSystem();
        Debug.Log("对话结束");
    }

    /// <summary>
    /// 是否激活状态
    /// </summary>
    /// <returns></returns>
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}