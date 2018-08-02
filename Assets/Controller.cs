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

    public void OnZoomInClicked()
    {
        model.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void OnZoomOutClicked()
    {
        model.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void OnObjectRotationChanged(Slider slider)
    {
        // model.transform.localRotation = new Quaternion(0.0f, slider.value, 0.0f, 0.0f);
        model.transform.rotation = Quaternion.Euler(0f, slider.value, 0f);
    }

    public void OnLightPositionChanged(Slider slider)
    {
        lightParent.rotation = Quaternion.Euler(slider.value, 0f, 0f);
    }

    public void OnMovePositionClicked(string position)
    {
        if(position == "up")
            model.transform.position += Vector3.up * 0.3f;
        else if (position == "down")
            model.transform.position -= Vector3.up * 0.3f;
        else if (position == "left")
            model.transform.position += Vector3.left * 0.3f;
        else if (position == "right")
            model.transform.position += Vector3.right * 0.3f;
        else if (position == "backward")
            model.transform.position -= Vector3.forward * 0.3f;
        else if (position == "forward")
            model.transform.position += Vector3.forward * 0.3f;
    }

    //public void OnMoveDownClicked()
    //{
    //    model.transform.position += Vector3.down * 10.0f;
    //}
    //public void OnMoveLeftClicked(string st)
    //{
    //    model.transform.position += Vector3.left * 10.0f;
    //}
    //public void OnMoveRightClicked()
    //{
    //    model.transform.position += Vector3.right * 10.0f;
    //}
    //public void OnMoveBackwardClicked()
    //{
    //    model.transform.position += Vector3.backward * 10.0f;
    //}
    //public void OnMoveForwardClicked()
    //{
    //    model.transform.position += Vector3.forward * 10.0f;
    //}
}