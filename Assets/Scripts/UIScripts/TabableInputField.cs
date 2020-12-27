using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class TabableInputField : MonoBehaviour, ISelectHandler {

    [SerializeField] private TabableInputFieldController controller;

    private void Start() {
        if (controller == null) {
            throw new Exception("Error: All TabableInputField require a TabableInputFieldController reference");
        }
        if (!controller.ContainsField(this)) {
            throw new Exception("Error: TabableInputField controller must contain this TabableInputField");
        }
    }

    public void OnSelect(BaseEventData eventData) {
        controller.SelectField(this);
    }
}
