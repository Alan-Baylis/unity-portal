using UnityEngine;
using System.Collections;

public class Portable : MonoBehaviour {

    public bool isTeleported { get; set; }
    public GameObject clone { get; set; }

    MeshRenderer myRenderer;
    Material material;
    Quaternion flipY = Quaternion.Euler(0, 180f, 0);

    // Use this for initialization
    void Start () {
        // create new material instance
        myRenderer = GetComponent<MeshRenderer>();
        material = new Material(myRenderer.material);
        myRenderer.material = material;	  
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void UpdateCloneTransform(GameObject entrancePortal, GameObject exitPortal) {
        if (clone == null) return;

        Transform tran = transform;
        Transform cloneTran = clone.transform;
        Transform exitTran = exitPortal.transform;

        Quaternion exitPortalRotation = exitTran.rotation * flipY;
        Vector3 entranceToSelf = tran.position - entrancePortal.transform.position;

        cloneTran.position = exitTran.position + exitPortalRotation * entranceToSelf;
        cloneTran.rotation = exitPortalRotation * tran.rotation;
    }

    public void SetClipPlane(Vector3 position, Vector3 normal) {
        material.SetVector("_ClipPlanePos", position);
        material.SetVector("_ClipPlaneNorm", normal);
    }

    public void SetClipPlaneActive(bool isActive) {
        int boolToInt = isActive ? 1 : 0;
        material.SetInt("_IsClipPlaneActive", boolToInt);
    }
}
