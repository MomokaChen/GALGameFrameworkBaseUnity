using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnterDialog : MonoBehaviour
{
    public void OnButtonClicked()
    {
        DialogScenceManager.Instance.EnterDialogSystem(1,1);
    }

}
