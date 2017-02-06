using System;
using System.Collections.Generic;
using MapzenGo.Helpers;
using MapzenGo.Models;
using UnityEngine;
using UnityEngine.UI;

namespace MapzenGo.Models.Plugins
{
    public class CustomObjectPlugin : Plugin
    {
        private List<Vector2d> _customObjects = new List<Vector2d>();
        public Material dataObjectMat;
        //public Material particleMaterial;
        //public List<ParticleSystem> particleList = new List<ParticleSystem>();
        //public List<GameObject> particleObjList = new List<GameObject>();
        public GameObject tooltip;
        public GameObject dataParticle;
        
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
            int visits = int.Parse(row.Visits);
            double lat = double.Parse(row.Lat);
            double lng = double.Parse(row.Lng);
            float utilRate = float.Parse(row.Avg_Utilization_Rate);
            float scaledY = 4 * visits;
            float scaledX = 400;
            float scaledZ = 400;
            float cubeSpacing = 10.0f;

            Vector2d meters = GM.LatLonToMeters(lat, lng);

            if (tile.Rect.Contains(meters))
            {
                //Debug.Log(tile.Rect.Center);
                GameObject dataContainer = new GameObject(row.Course_Name + " at " + row.Location);
                dataContainer.transform.position = (meters - tile.Rect.Center).ToVector3();
                //float x = dataContainer.transform.position.x;
                //dataContainer.transform.position += new Vector3(x * (index + 1)+ cubeSpacing, 0, 0);

                GameObject dataCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                dataCube.transform.localScale = new Vector3(scaledX, scaledY, scaledZ);

                // fix vertical positions of cubes
                dataCube.transform.position += new Vector3(0, scaledY / 2, 0);

                // apply custom material using unity editor
                dataCube.GetComponent<MeshRenderer>().material = dataObjectMat;

                // attach cube to container
                dataCube.transform.SetParent(dataContainer.transform, false);

                // attach container to tile
                dataContainer.transform.SetParent(tile.transform, false);

                // Create tooltips for each row
                var tooltipText = "Course: " + row.Course_Name +
                    "\nLocation: " + row.Location +
                    "\nAvg. Utilization Rate: " + row.Avg_Utilization_Rate +
                    "\nVisits in 2015: " + row.Visits;

                //Debug.Log("Render data: "+ "Course: " + row.Course_Name +
                //    " | Location: " + row.Location + " | Visits in 2015: " + row.Visits);

                GameObject tooltipObj = Instantiate(tooltip) as GameObject;
                tooltipObj.transform.SetParent(dataContainer.transform, false);
                Vector3 parentPos = dataCube.transform.position;
                tooltipObj.transform.position += new Vector3(0, parentPos.y * 2 + 20, 0);
                var frontTxt = tooltipObj.transform.FindChild("DataTooltipCanvas/UITextFront").GetComponent<Text>();
                var backTxt = tooltipObj.transform.FindChild("DataTooltipCanvas/UITextReverse").GetComponent<Text>();

                frontTxt.horizontalOverflow = HorizontalWrapMode.Wrap;
                backTxt.horizontalOverflow = HorizontalWrapMode.Wrap;

                frontTxt.text = tooltipText;
                backTxt.text = tooltipText;

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
    }
}
