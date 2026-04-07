using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogScenceManager : MonoSingleton<DialogScenceManager>
{
    public void EnterDialogSystem(int branchId, int nodeId)
    {
        StartCoroutine(LoadSceneAndStartDialog(branchId, nodeId));
    }

    private IEnumerator LoadSceneAndStartDialog(int branchId, int nodeId)
    {
        yield return SceneManager.LoadSceneAsync("GalSystemTest");
        yield return null;

        DialogueManager.Instance.StartDialogue(branchId, nodeId);
    }

    public void LeaveDialogSystem()
    {
        StartCoroutine(LoadSceneAndLeaveDialog());
    }

    private IEnumerator LoadSceneAndLeaveDialog()
    {
        yield return SceneManager.LoadSceneAsync("Main");
        yield return null;
    }
}
