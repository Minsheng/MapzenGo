using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResetApp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown("1"))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
