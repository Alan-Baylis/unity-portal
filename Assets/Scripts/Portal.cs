using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public RenderTexture viewTexture { get; private set; }
    public Portal exitPortal;
    public float x = 1.0f;
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
        UpdateRenderTarget();
    }

    // Update is called once per frame
    void Update() {
        // update position, rotation, fov, render target
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
            // enable clip plane
            portable.SetClipPlaneActive(true);
        }
    }

    void OnTriggerStay(Collider other) {
        var portable = other.GetComponent<Portable>();
        if (portable != null) {
            if (portable.isTeleported) return;
            portable.UpdateCloneTransform(gameObject, exitPortal.gameObject);
            // update clip plane
            portable.SetClipPlane(transform.position, transform.forward);
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
            // disable clip plane
            portable.SetClipPlaneActive(false);
        }
    }

    void UpdateCamera() {
        // camera position
        Vector3 localExitToPlayer = exitPortal.transform.InverseTransformPoint(player.transform.position);
        myCamera.transform.localPosition = new Vector3(-localExitToPlayer.x, localExitToPlayer.y, -localExitToPlayer.z);

        // camera rotation
        Vector3 playerEulerRot = player.transform.eulerAngles;
        float rotationX = playerEulerRot.x + transform.eulerAngles.x;
        myCamera.transform.rotation = Quaternion.Euler(rotationX, playerEulerRot.y + 180f, playerEulerRot.z);

        // field of view
        myCamera.fieldOfView = mainCamera.fieldOfView;

        // render target size
        if(Screen.width != viewTexture.width || Screen.height != viewTexture.height) {
            UpdateRenderTarget();
        }

        /*
        // oblique near clipping plane
        Vector3 planeNormal = myCamera.transform.InverseTransformDirection(transform.forward);
        float planeDist = myCamera.transform.localPosition.magnitude;
        Vector3 clipPlane = new Vector4(planeNormal.x, planeNormal.y, planeNormal.z, -planeDist);
        myCamera.projectionMatrix = myCamera.CalculateObliqueMatrix(clipPlane);
        */
    }

    void UpdateRenderTarget() {
        viewTexture = new RenderTexture(Screen.width, Screen.height, 16);
        myCamera.targetTexture = viewTexture;
    }
}