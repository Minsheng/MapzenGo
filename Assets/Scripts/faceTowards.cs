using UnityEngine;
using System.Collections;

public class faceTowards : MonoBehaviour {
    // object tooltip always facing player
    public int damping = 2;
    public Transform target;

    // Use this for initialization
    void Start() {
    }
	
	// Update is called once per frame
	void Update () {
        lookAtPlayer();
    }

    private void lookAtPlayer()
    {
        if (target)
        {
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
    }
}
