using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public RenderTexture viewTexture { get; private set; }
    public Portal exitPortal;

    GameObject player;
    Camera playerCamera;
    Camera camera;
    Material material;
    GameObject frame;
    int viewTextureResolution = 512;

    // Use this for initialization
    void Start() {
        // Player
        player = GameObject.Find("Player");
        if (player == null) throw new UnityException("Missing Player!");
        playerCamera = Camera.main;
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
    }

    // Update is called once per frame
    void Update() {
        material.SetTexture("_MainTex", exitPortal.viewTexture); // refactor this, only needs to be called once, when initialized.
        // Parallax
        UpdateCamera();
        Vector3 lookDir = transform.position + (transform.position - player.transform.position).normalized;
        frame.transform.LookAt(lookDir, Vector3.up);
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
            if (portable.isTeleported) {
                portable.isTeleported = false;
            }
        }
    }

    void UpdateCamera() {
        // camera position
        Vector3 playerFromExitPos = exitPortal.transform.InverseTransformPoint(player.transform.position);
        camera.transform.localPosition = new Vector3(-playerFromExitPos.x, playerFromExitPos.y, -playerFromExitPos.z);

        // camera rotation
        camera.transform.LookAt(transform.position, Vector3.up);

        // field of view
        float opp = frame.transform.lossyScale.y / 2;
        float adj = Vector3.Distance(exitPortal.transform.position, player.transform.position);
        camera.fieldOfView = (2 * Mathf.Atan(opp / adj) * (180f / Mathf.PI));
        
        /*
        // oblique near clipping plane
        Vector3 clipPlane = transform.position + transform.forward;
        camera.projectionMatrix = camera.CalculateObliqueMatrix(new Vector4(clipPlane.x, clipPlane.y, clipPlane.z, 1.0f));
        */
        
    }
}