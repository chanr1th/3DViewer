using SFB;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class Controller
{
    [SerializeField]
    private Transform lightParent;

    [SerializeField]
    private Material lineMat;

    [SerializeField]
    private Color color;

    private Transform cameraTransform;
    private Vector3 deltaMousePosition;
    private Vector3 lastMousePosition;
    private MeshRenderer meshRenderer;
    private Transform modelTransform;
    private bool enableVertices;
    private List<Vector3> vertices;
}

public partial class Controller : MonoBehaviour
{
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastMousePosition = Input.mousePosition;
        enableVertices = false;
        vertices = new List<Vector3>();

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

    private void OnRenderObject()
    {
        if (enableVertices)
        {
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                Vector3 from = modelTransform.TransformPoint(vertices[i]);
                Vector3 to = modelTransform.TransformPoint(vertices[i + 1]);

                GL.Begin(GL.LINES);
                lineMat.SetPass(0);
                GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));
                GL.Vertex3(from.x, from.y, from.z);
                GL.Vertex3(to.x, to.y, to.z);
                GL.End();
            }
        }
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
                modelTransform = model.transform;

                meshRenderer = model.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer != null) {
                    Vector3 position = modelTransform.position;
                    position.y = meshRenderer.bounds.extents.y;
                    modelTransform.position = position;

                    model.GetComponentInChildren<MeshFilter>().mesh.GetVertices(vertices);
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

    public void OnShow3DOject(bool isOn)
    {
        if (meshRenderer == null) return;
        meshRenderer.enabled = isOn;
    }

    public void OnEnableVertices(bool isOn)
    {
        enableVertices = isOn;
    }
}