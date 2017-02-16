using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using VRTK;
using MapzenGo.Models.Plugins;
using System.Linq;

public class ToggleTooltips : MonoBehaviour
{
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
        //DebugLogger(e.controllerIndex, "GRIP", "pressed", e);

        //GameObject customObjects = GameObject.Find("CustomObjects");

        var cop = GetComponent("CustomObjectPlugin");

        if (cop != null)
        {
            Debug.Log("Found script");
        }
        //if (refScript != null)
        //{
        //    List<GameObject> tooltips = refScript.tooltips;

        //    if (tooltips.Count > 0)
        //    {
        //        foreach (GameObject tt in tooltips)
        //        {
        //            tt.SetActive(true);
        //        }
        //    }
        //}
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(e.controllerIndex, "GRIP", "released", e);

        //GameObject customObjects = GameObject.Find("CustomObjects");

        //Plugin script = customObjects.GetComponent<MapzenGo.Models.Plugins.CustomObjectPlugin>();

        //foreach (GameObject tt in script.tooltips)
        //{
        //    tt.SetActive(false);
        //}
    }
}