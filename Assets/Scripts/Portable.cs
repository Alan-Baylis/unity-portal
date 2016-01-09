using UnityEngine;
using System.Collections;

public class Portable : MonoBehaviour {

    public bool isTeleported { get; set; }
    public GameObject clone { get; set; }

    Quaternion flipY = Quaternion.Euler(0, 180f, 0);

    // Use this for initialization
    void Start () {
	
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
}
