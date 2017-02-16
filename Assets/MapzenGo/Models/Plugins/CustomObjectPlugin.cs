using System;
using System.Collections;
using System.Collections.Generic;
using MapzenGo.Helpers;
using MapzenGo.Models;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using VRTK;

namespace MapzenGo.Models.Plugins
{
    public class CustomObjectPlugin : Plugin
    {
        private List<Vector2d> _customObjects = new List<Vector2d>();
        //public Material dataObjectMat;
        public GameObject dataCube;
        //public Material particleMaterial;
        //public List<ParticleSystem> particleList = new List<ParticleSystem>();
        //public List<GameObject> particleObjList = new List<GameObject>();
        public GameObject tooltip;
        public GameObject dataParticle;
        public List<GameObject> tooltips;
        
        public int maxCubePerRow;

        //private readonly List<Vector2d> _customObjects = new List<Vector2d>()
        //{
        //    new Vector2d(40.753176, -73.982229),
        //    new Vector2d(40.769759, -73.975537),
        //    new Vector2d(40.740304, -73.972425),
        //    new Vector2d(40.728664, -74.032011),
        //};

        public override void Create(Tile tile)
        {
            base.Create(tile);

            renderData(tile);
        }

        private void renderData(Tile t)
        {
            List<Row> recrData = t.RecrData;

            if (recrData != null)
            {
                for (int i = 0; i < recrData.Count; i++)
                {
                    renderVisualization(i, recrData[i], maxCubePerRow, t);
                }
            }
        }

        private void renderVisualization(int index, Row row, int maxPerRow, Tile tile)
        {
            //Debug.Log("The PROBLMATIC row is " + row.Location);
            int visits = int.Parse(row.Visits);
            string coorStr = row.Coordinates;
            List<string> coordList = coorStr.Split(',').ToList<string>();
            double lat = double.Parse(coordList[0]);
            double lng = double.Parse(coordList[1]);
            float utilRate = float.Parse(row.Avg_Utilization_Rate);
            float scaledY = 1000 * utilRate;
            float scaledX = 200;
            float scaledZ = 200;
            float spacingFactor = 1.5f;

            Vector2d meters = GM.LatLonToMeters(lat, lng);

            if (tile.Rect.Contains(meters))
            {
                //Debug.Log(tile.Rect.Center);
                GameObject dataContainer = new GameObject(row.Location);
                dataContainer.transform.position = (meters - tile.Rect.Center).ToVector3();
                dataContainer.transform.position += new Vector3((index+1)*scaledX*spacingFactor, 0, 0);

                GameObject dataObj = Instantiate(dataCube) as GameObject;

                //GameObject dataCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                dataObj.transform.localScale = new Vector3(scaledX, scaledY, scaledZ);

                // fix vertical positions of cubes
                dataObj.transform.position += new Vector3(0, scaledY / 2, 0);

                // apply custom material using unity editor
                //dataCube.GetComponent<MeshRenderer>().material = dataObjectMat;

                // attach cube to container
                dataObj.transform.SetParent(dataContainer.transform, false);

                // attach container to tile
                dataContainer.transform.SetParent(tile.transform, false);

                createTooltips(row, dataContainer, dataObj);

                //GameObject particleObj = Instantiate(dataParticle) as GameObject;
                //ParticleSystem ps = particleObj.GetComponentInChildren<ParticleSystem>();

                //ps.startLifetime = 10.0f;
                //ps.maxParticles = int.Parse(row.Visits);
                //particleObj.GetComponent<Rotater>().RotationPerSecond += new Vector3(0, float.Parse(row.Avg_Utilization_Rate) * 10, 0);
                //particleObj.transform.parent = go.transform;

                // Visualize data as particle system
                //ParticleSystem system = go.AddComponent<ParticleSystem>();
                //go.GetComponent<ParticleSystemRenderer>().material = particleMaterial;
                //system.startLifetime = 10.0f;
                //system.startSpeed = float.Parse(row.Avg_Utilization_Rate);
                //system.maxParticles = int.Parse(row.Visits);

                //go.transform.position = (meters - tile.Rect.Center).ToVector3();
                //go.transform.SetParent(tile.transform, false);
                //go.transform.Rotate(-90, 0, 0); // Rotate so the system emits upwards.

                //particleList.Add(system);
                //particleObjList.Add(particleObj);
            }
        }

        private void createTooltips(Row row, GameObject parentContainer, GameObject cube)
        {
            float tooltipScale = 100;
            // Create tooltips for each row
            var tooltipText = "<b>Location</b>: " + row.Location +
                "\n<b>Avg. Utilization Rate</b>: " + row.Avg_Utilization_Rate +
                "\n<b>Total Visits</b>: " + row.Visits +
                "\n<b>Total Waitlist</b>: " + row.Course_Waitlist;

            //var tooltipText = "Course: " + row.Course_Name +
            //    "\nLocation: " + row.Location +
            //    "\nAvg. Utilization Rate: " + row.Utilization_Rate +
            //    "\nVisits in 2015: " + row.Visits;

            //Debug.Log("Render data: "+ "Course: " + row.Course_Name +
            //    " | Location: " + row.Location + " | Visits in 2015: " + row.Visits);

            GameObject tooltipObj = Instantiate(tooltip) as GameObject;
            tooltipObj.transform.localScale = new Vector3(tooltipScale, tooltipScale, tooltipScale);
            tooltipObj.transform.SetParent(parentContainer.transform, false);
            Vector3 parentPos = cube.transform.position;
            tooltipObj.transform.position += new Vector3(0, parentPos.y*2 + 2, 0);
            VRTK_ObjectTooltip script = tooltipObj.GetComponent<VRTK_ObjectTooltip>();
            script.drawLineTo = cube.transform;
            script.UpdateText(tooltipText);

            //var frontTxt = tooltipObj.transform.FindChild("DataTooltipCanvas/UITextFront").GetComponent<Text>();
            //var backTxt = tooltipObj.transform.FindChild("DataTooltipCanvas/UITextReverse").GetComponent<Text>();

            //frontTxt.horizontalOverflow = HorizontalWrapMode.Wrap;
            //backTxt.horizontalOverflow = HorizontalWrapMode.Wrap;

            //frontTxt.text = tooltipText;
            //backTxt.text = tooltipText;

            //tooltipObj.SetActive(false);
            tooltips.Add(tooltipObj);
        }

        //void Start()
        //{
        //    if (GetComponent<VRTK_ControllerEvents>() == null)
        //    {
        //        Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
        //        return;
        //    }

        //    //Setup controller event listeners
        //    GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        //    GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);
        //}

        //private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
        //{
        //    Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
        //            + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
        //}

        //private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
        //{
        //    DebugLogger(e.controllerIndex, "GRIP", "pressed", e);
        //    Debug.Log("Tooltips count: " + tooltips.Count);

        //    if (tooltips.Count > 0)
        //    {
        //        foreach (GameObject tt in tooltips)
        //        {
        //            tt.SetActive(true);
        //        }
        //    }
        //}

        //private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
        //{
        //    //DebugLogger(e.controllerIndex, "GRIP", "released", e);

        //    if (tooltips.Count > 0)
        //    {
        //        foreach (GameObject tt in tooltips)
        //        {
        //            tt.SetActive(false);
        //        }
        //    }
        //}
    }
}
