namespace VRTK
{
    using UnityEngine;
    using System.Collections;
    using System;

    public class MoveWorld : MonoBehaviour
    {

        void Start()
        {
            if (GetComponent<VRTK_ControllerEvents>() == null)
            {
                Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
                return;
            }

            //Setup controller event listeners
            GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
            GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
        }

        private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
        {
            Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
                    + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
        }

        private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "pressed", e);
            GameObject go = GameObject.Find("World");

            if (go)
            {
                iTween.MoveTo(go, iTween.Hash("path", iTweenPath.GetPath("BirdEyePos"), "easeType", iTween.EaseType.easeInOutSine, "time", 0.5));
            }
        }

        private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "released", e);
            GameObject go = GameObject.Find("World");

            if (go)
            {
                iTween.MoveTo(go, iTween.Hash("path", iTweenPath.GetPath("OriginPos"), "easeType", iTween.EaseType.easeInOutSine, "time", 0.5));
            }
        }
    }
}
