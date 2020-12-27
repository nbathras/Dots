using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(EventSystem))]
public class TabableInputFieldController : MonoBehaviour {

    [SerializeField] private List<TabableInputField> list;
    [SerializeField] private GraphicRaycaster raycaster;

    private EventSystem system;
    private PointerEventData pointerEventData;

    private int index;

    private void Awake() {
        index = -1;
    }

    private void Start() {
        system = EventSystem.current;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            pointerEventData = new PointerEventData(system) {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results) {
                TabableInputField selectedField = result.gameObject.GetComponent<TabableInputField>();

                if (selectedField != null && list.Contains(selectedField)) {
                    index = list.IndexOf(selectedField);
                    break;
                } else {
                    index = -1;
                }
            }
        }

        if (index != -1 && Input.GetKeyDown(KeyCode.Tab)) {
            index = (index + 1) % list.Count;
            TabableInputField next = list[index];

            InputField inputField = next.GetComponent<InputField>();
            if (inputField != null) {
                inputField.OnPointerClick(new PointerEventData(system));
            }

            system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
        }
    }

    public bool ContainsField(TabableInputField field) {
        return list.Contains(field);
    }

    public void SelectField(TabableInputField field) {
        index = list.IndexOf(field);
    }
}
