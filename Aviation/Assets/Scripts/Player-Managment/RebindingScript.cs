using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class RebindingScript : MonoBehaviour
{
    [SerializeField] private InputActionReference Shooting = null;
    [SerializeField] private PlayerBehaviourScript playerController = null;
    [SerializeField] private TMP_Text bindingDisplayNameText = null;
    [SerializeField] private GameObject startRebindingObject = null;
    [SerializeField] private GameObject waitingForInputObject = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebinding()
    {
        startRebindingObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        playerController.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = Shooting.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        //int bindingIndex = Shooting.action.GetBindingIndexForControl(Shooting.action.controls[0]);

        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(
            Shooting.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        startRebindingObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        playerController.PlayerInput.SwitchCurrentActionMap("Gameplay");
    }
}
