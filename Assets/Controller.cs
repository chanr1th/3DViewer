using SFB;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class Controller
{
    [SerializeField]
    private Transform lightParent;

    private Transform cameraTransform;
    private Vector3 deltaMousePosition;
    private Vector3 lastMousePosition;
}

public partial class Controller : MonoBehaviour
{
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.tag == "UI")
        {
            return;
        }

        cameraTransform.position += cameraTransform.forward * Input.mouseScrollDelta.y;

        if (Input.GetMouseButton(0))
        {
            deltaMousePosition = Input.mousePosition - lastMousePosition;

            cameraTransform.RotateAround(Vector3.zero, cameraTransform.up, Input.GetAxis("Mouse X") * deltaMousePosition.magnitude);
            cameraTransform.RotateAround(Vector3.zero, -cameraTransform.right, Input.GetAxis("Mouse Y") * deltaMousePosition.magnitude);
        }

        cameraTransform.LookAt(Vector3.zero);
        lastMousePosition = Input.mousePosition;
    }
}

public partial class Controller
{
    private GameObject model = null;

    public void OnImportClicked() {
        StandaloneFileBrowserWindows standaloneFileBrowserWindows = new StandaloneFileBrowserWindows();
        standaloneFileBrowserWindows.OpenFilePanelAsync("Import", string.Empty, new ExtensionFilter[] { new ExtensionFilter("Obj File", "obj") }, false, files => {

            if (files.Length > 0) {
                if (model != null) Destroy(model);
                model = OBJLoader.LoadOBJFile(files[0]);
                MeshRenderer meshRenderer = model.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer != null) {
                    Vector3 position = model.transform.position;
                    position.y = meshRenderer.bounds.extents.y;
                    model.transform.position = position;
                }
            }

        });
    }

    public void OnLightPositionChanged(Slider slider)
    {
        lightParent.rotation = Quaternion.Euler(slider.value, 0f, 0f);
    }
}