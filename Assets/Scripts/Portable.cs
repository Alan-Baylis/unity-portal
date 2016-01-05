using UnityEngine;
using System.Collections;

public class Portable : MonoBehaviour {

    public bool isTeleported { get; set; }
    public GameObject clone { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateCloneTransform(GameObject entrancePortal, GameObject exitPortal) {
        if (clone == null) return;
        Vector3 entranceToSelf = transform.position - entrancePortal.transform.position;
        clone.transform.position = exitPortal.transform.rotation * entranceToSelf + exitPortal.transform.position;
        clone.transform.rotation = transform.rotation * exitPortal.transform.rotation * Quaternion.Euler(0, 180f, 0);
    }
}
