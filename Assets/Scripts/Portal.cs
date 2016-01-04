using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public RenderTexture viewTexture { get; private set; }
    public Portal exitPortal;
    public GameObject player;

    Camera camera;
    Material material;
    int viewTextureResolution = 512;

	// Use this for initialization
	void Start () {
        // Player
        player = GameObject.Find("Player");
        if (player == null) throw new UnityException("Missing Player!");
        // Camera
        GameObject camObject = transform.Find("Camera").gameObject;
        if (camObject == null) throw new UnityException("Missing Portal Camera!");
        camera = camObject.GetComponent<Camera>();
        int width = (int)(Mathf.Abs(transform.lossyScale.x) * viewTextureResolution);
        int height = (int)(Mathf.Abs(transform.lossyScale.y) * viewTextureResolution);
        viewTexture = new RenderTexture(width, height, 16);
        camera.targetTexture = viewTexture;
        UpdateCameraView();
        // Material
        material = new Material(Shader.Find("Unlit/Texture"));
        GetComponent<MeshRenderer>().material = material;
	}
	
	// Update is called once per frame
	void Update () {
	    if (exitPortal != null) {
            material.SetTexture("_MainTex", exitPortal.viewTexture);
            UpdateCameraView();
        }
        Vector3 incident = (exitPortal.transform.position - player.transform.position).normalized;
        Vector3 cameraDir = Vector3.Reflect(incident, exitPortal.transform.forward);
        camera.transform.LookAt(camera.transform.position + cameraDir, Vector3.up);
	}

    public void UpdateCameraView() {
        float fieldOfView = Camera.main.fieldOfView;
        camera.fieldOfView = fieldOfView;
        float y = transform.lossyScale.y / 2;
        float theta = (fieldOfView / 2) * Mathf.Deg2Rad;
        float h = y / Mathf.Sin(theta);
        float distance = h * Mathf.Cos(theta);
        Vector3 camPos = camera.transform.localPosition;
        camera.transform.localPosition = new Vector3(camPos.x, camPos.y, distance);
        camera.nearClipPlane = distance;
    }
}