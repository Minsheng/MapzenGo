using System;
using System.Collections.Generic;
using MapzenGo.Helpers;
using MapzenGo.Models;
using UnityEngine;

namespace MapzenGo.Models.Plugins
{
    public class CustomObjectPlugin : Plugin
    {
        public TextAsset file;
        List<Row> rowList = new List<Row>();
        bool isLoaded = false;
        private List<Vector2d> _customObjects = new List<Vector2d>();
        public Material dataObjectMat;

        public class Row
        {
            public string Course_Name;
            public string Location;
            public string Postal_Code;
            public string Ward;
            public string Avg_Utilization_Rate;
            public string Course_Waitlist;
            public string Visits;
            public string Lat;
            public string Lng;
        }

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

            Load(file);

            foreach (var row in rowList)
            {
                var lat = Double.Parse(row.Lat);
                var lng = Double.Parse(row.Lng);
                var utilRate = float.Parse(row.Avg_Utilization_Rate);
                var visits = int.Parse(row.Visits);

                var meters = GM.LatLonToMeters(lat, lng);

                if (tile.Rect.Contains(meters))
                {
                    var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    go.transform.position = (meters - tile.Rect.Center).ToVector3();
                    go.transform.position += new Vector3(0, visits, 0);
                    go.transform.localScale = Vector3.one * 100 * utilRate;
                    go.transform.SetParent(tile.transform, false);
                    go.GetComponent<MeshRenderer>().material = dataObjectMat;
                }
            }

            //foreach (var pos in _customObjects)
            //{
            //    var meters = GM.LatLonToMeters(pos.x, pos.y);

            //    if (tile.Rect.Contains(meters))
            //    {
            //        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //        go.transform.position = (meters - tile.Rect.Center).ToVector3();
            //        go.transform.localScale = Vector3.one * 1000;
            //        go.transform.SetParent(tile.transform, false);
            //    }
            //}
        }

        public bool IsLoaded()
        {
            return isLoaded;
        }

        public List<Row> GetRowList()
        {
            return rowList;
        }

        public void Load(TextAsset csv)
        {
            rowList.Clear();
            string[][] grid = CsvParser2.Parse(csv.text);
            for (int i = 1; i < grid.Length; i++)
            {
                Row row = new Row();
                row.Course_Name = grid[i][0];
                row.Location = grid[i][1];
                row.Postal_Code = grid[i][2];
                row.Ward = grid[i][3];
                row.Avg_Utilization_Rate = grid[i][4];
                row.Course_Waitlist = grid[i][5];
                row.Visits = grid[i][6];
                row.Lat = grid[i][7];
                row.Lng = grid[i][8];

                rowList.Add(row);
            }
            isLoaded = true;
        }

        public int NumRows()
        {
            return rowList.Count;
        }

        public Row GetAt(int i)
        {
            if (rowList.Count <= i)
                return null;
            return rowList[i];
        }

        public Row Find_Course_Name(string find)
        {
            return rowList.Find(x => x.Course_Name == find);
        }
        public List<Row> FindAll_Course_Name(string find)
        {
            return rowList.FindAll(x => x.Course_Name == find);
        }
        public Row Find_Location(string find)
        {
            return rowList.Find(x => x.Location == find);
        }
        public List<Row> FindAll_Location(string find)
        {
            return rowList.FindAll(x => x.Location == find);
        }
        public Row Find_Postal_Code(string find)
        {
            return rowList.Find(x => x.Postal_Code == find);
        }
        public List<Row> FindAll_Postal_Code(string find)
        {
            return rowList.FindAll(x => x.Postal_Code == find);
        }
        public Row Find_Ward(string find)
        {
            return rowList.Find(x => x.Ward == find);
        }
        public List<Row> FindAll_Ward(string find)
        {
            return rowList.FindAll(x => x.Ward == find);
        }
        public Row Find_Avg_Utilization_Rate(string find)
        {
            return rowList.Find(x => x.Avg_Utilization_Rate == find);
        }
        public List<Row> FindAll_Avg_Utilization_Rate(string find)
        {
            return rowList.FindAll(x => x.Avg_Utilization_Rate == find);
        }
        public Row Find_Course_Waitlist(string find)
        {
            return rowList.Find(x => x.Course_Waitlist == find);
        }
        public List<Row> FindAll_Course_Waitlist(string find)
        {
            return rowList.FindAll(x => x.Course_Waitlist == find);
        }
        public Row Find_Visits(string find)
        {
            return rowList.Find(x => x.Visits == find);
        }
        public List<Row> FindAll_Visits(string find)
        {
            return rowList.FindAll(x => x.Visits == find);
        }
        public Row Find_Lat(string find)
        {
            return rowList.Find(x => x.Lat == find);
        }
        public List<Row> FindAll_Lat(string find)
        {
            return rowList.FindAll(x => x.Lat == find);
        }
        public Row Find_Lng(string find)
        {
            return rowList.Find(x => x.Lng == find);
        }
        public List<Row> FindAll_Lng(string find)
        {
            return rowList.FindAll(x => x.Lng == find);
        }
    }
}
