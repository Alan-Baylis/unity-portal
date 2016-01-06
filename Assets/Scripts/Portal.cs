using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public RenderTexture viewTexture { get; private set; }
    public Portal exitPortal;
    public GameObject player;

    Camera camera;
    Material material;
    GameObject frame;
    int viewTextureResolution = 512;

    // Use this for initialization
    void Start() {
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
        // Material
        material = new Material(Shader.Find("Unlit/Texture"));
        // Frame
        frame = transform.Find("Frame").gameObject;
        frame.GetComponent<MeshRenderer>().material = material;

        UpdateCameraView();
    }

    // Update is called once per frame
    void Update() {
        material.SetTexture("_MainTex", exitPortal.viewTexture); // refactor this, only needs to be called once, when initialized.
        // Parallax
        Vector3 portalToExitPlayer = player.transform.position - exitPortal.transform.position;
        float dot = Vector3.Dot(-transform.right, portalToExitPlayer);
        Vector3 camPos = camera.transform.localPosition;
        camera.transform.localPosition = new Vector3(dot / transform.localScale.x, camPos.y, camPos.z);
    }

    public void UpdateCameraView() {
        float fieldOfView = Camera.main.fieldOfView;
        camera.fieldOfView = fieldOfView;
        float y = frame.transform.lossyScale.y / 2;
        float theta = (fieldOfView / 2) * Mathf.Deg2Rad;
        float h = y / Mathf.Sin(theta);
        float distance = h * Mathf.Cos(theta);
        Vector3 camPos = camera.transform.localPosition;
        camera.transform.localPosition = new Vector3(camPos.x, camPos.y, -distance / transform.localScale.z);
        camera.nearClipPlane = distance;
    }

    void OnTriggerEnter(Collider other) {
        var portable = other.GetComponent<Portable>();
        if (portable != null) {
            if (portable.isTeleported) return;
            GameObject clone = (GameObject)Instantiate(other.gameObject);
            var portableClone = clone.GetComponent<Portable>();
            portableClone.isTeleported = true;
            portable.clone = clone;
            portable.UpdateCloneTransform(gameObject, exitPortal.gameObject);
        }
    }

    void OnTriggerStay(Collider other) {
        var portable = other.GetComponent<Portable>();
        if (portable != null) {
            if (portable.isTeleported) return;
            portable.UpdateCloneTransform(gameObject, exitPortal.gameObject);
        }
    }

    void OnTriggerExit(Collider other) {
        var portable = other.GetComponent<Portable>();
        if (portable != null) {
            Vector3 portalToObjDir = (other.transform.position - transform.position).normalized;
            float side = Vector3.Dot(transform.forward, portalToObjDir);
            if (side < 0) {
                Destroy(other.gameObject);
            }
            if(portable.isTeleported) {
                portable.isTeleported = false;
            }
        }
    }
}