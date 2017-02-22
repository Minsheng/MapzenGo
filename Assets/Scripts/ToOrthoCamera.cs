using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class ToOrthoCamera : MonoBehaviour {

    private Matrix4x4 ortho;
    private Matrix4x4 perspective;
           
    public float fov = 60f;
    public float near = .05f;
    public float far = 5000f;
    public float orthographicSize = 50f;
    public float transitionTime = .5f;
 
    private float aspect;
    private Boolean orthoOn;

    void Awake()
    {
        aspect = (Screen.width + 0.0f) / (Screen.height + 0.0f);

        perspective = GetComponent<Camera>().projectionMatrix;

        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        orthoOn = false;
    }

    void Start()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
            return;
        }

        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);
    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        orthoOn = !orthoOn;
        BlendToMatrix(ortho, transitionTime);
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        orthoOn = !orthoOn;
        BlendToMatrix(perspective, transitionTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            orthoOn = !orthoOn;
            if (orthoOn)
                BlendToMatrix(ortho, transitionTime);
            else
                BlendToMatrix(perspective, transitionTime);
        }
    }

    public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }

    private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            GetComponent<Camera>().projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration);
            yield return 1;
        }
        GetComponent<Camera>().projectionMatrix = dest;
    }

    public Coroutine BlendToMatrix(Matrix4x4 targetMatrix, float duration)
    {
        StopAllCoroutines();
        return StartCoroutine(LerpFromTo(GetComponent<Camera>().projectionMatrix, targetMatrix, duration));
    }
}
