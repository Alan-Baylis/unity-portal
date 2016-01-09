using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public GameObject target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target.transform, Vector3.up);
	}
}
