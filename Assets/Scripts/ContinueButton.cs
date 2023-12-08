using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public static Action ContinueGameAction;

    public void OnContinueButtonPressed()
    {
        ContinueGameAction?.Invoke();
    }
}
