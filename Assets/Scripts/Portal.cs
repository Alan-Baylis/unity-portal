using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public RenderTexture viewTexture { get; private set; }
    public Portal exitPortal;
    GameObject player;
    Camera myCamera, mainCamera;
    Material material;
    int viewTextureResolution = 512;

    void Awake() {
        // Player
        player = GameObject.Find("Player");
        if (player == null) throw new UnityException("Missing Player!");
        // Camera
        GameObject camObject = transform.Find("Camera").gameObject;
        if (camObject == null) throw new UnityException("Missing Portal Camera!");
        myCamera = camObject.GetComponent<Camera>();
        mainCamera = Camera.main;
        // Render Texture
        viewTexture = new RenderTexture(Screen.width, Screen.height, 16);
        myCamera.targetTexture = viewTexture;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UpdateCamera();
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
        Vector3 localExitToPlayer = exitPortal.transform.InverseTransformPoint(player.transform.position);
        myCamera.transform.localPosition = new Vector3(-localExitToPlayer.x, localExitToPlayer.y, -localExitToPlayer.z);

        // camera rotation
        Vector3 playerEulerRot = player.transform.eulerAngles;
        float rotationY = playerEulerRot.y + 180f;
        Vector3 eulerRot = new Vector3(playerEulerRot.x, rotationY, playerEulerRot.z);
        myCamera.transform.rotation = Quaternion.Euler(eulerRot);

        // field of view
        myCamera.fieldOfView = mainCamera.fieldOfView;

        /*
        // oblique near clipping plane
        Vector3 clipPlane = transform.position + transform.forward;
        camera.projectionMatrix = camera.CalculateObliqueMatrix(new Vector4(clipPlane.x, clipPlane.y, clipPlane.z, 1.0f));
        */
    }
}