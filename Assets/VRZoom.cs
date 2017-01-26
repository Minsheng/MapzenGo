using UnityEngine;
using System.Collections;

public class VRZoom : MonoBehaviour {
    public Camera camera;

    public int zoom = 20;
    public int normal = 60;
    public float smooth = 5.0f;

    private bool zooming = false;

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device dev;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        dev = SteamVR_Controller.Input((int)trackedObj.index);

        if (dev.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("Button pressed");
            zooming = !zooming;
        }

        if (zooming)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoom, Time.deltaTime * smooth);
        } else
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, normal, Time.deltaTime * smooth);
        }
    }
}
