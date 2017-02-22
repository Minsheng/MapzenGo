using System;
using System.Collections.Generic;
using System.IO;
using MapzenGo.Models.Plugins;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models.Plugins
{
    public class MapImagePlugin : Plugin
    {
        public enum TileServices
        {
            Default,
            Satellite,
            Terrain,
            Toner,
            Watercolor
        }

        public TileServices TileService = TileServices.Default;

        private string[] TileServiceUrls = new string[] {
            "http://b.tile.openstreetmap.org/",
            "http://b.tile.openstreetmap.us/usgs_large_scale/",
            "http://tile.stamen.com/terrain-background/",
            "http://a.tile.stamen.com/toner/",
            "https://stamen-tiles.a.ssl.fastly.net/watercolor/"
        };

        public string RelativeCachePath = "../CachedMapImage/";
        protected string CacheFolderPath;

        public override void Create(Tile tile)
        {
            base.Create(tile);

            var go = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
            go.name = "map";
            go.SetParent(tile.transform, true);
            go.localScale = new Vector3((float)tile.Rect.Width, (float)tile.Rect.Width, 1);
            go.rotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
            go.localPosition = Vector3.zero;
            go.localPosition -= new Vector3(0, 1, 0);
            var rend = go.GetComponent<Renderer>();
            rend.material = tile.Material;

            var url = TileServiceUrls[(int)TileService] + tile.Zoom + "/" + tile.TileTms.x + "/" + tile.TileTms.y + ".png";

            CacheFolderPath = Path.Combine(Application.dataPath, RelativeCachePath);
            var tilePath = CacheFolderPath + "_" + tile.TileTms.x + "_" + tile.TileTms.y + ".png";

            if (File.Exists(tilePath))
            {
                // read from cached image files
                if (rend)
                {
                    byte[] fileData = File.ReadAllBytes(tilePath);
                    Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
                    tex.LoadImage(fileData);

                    //rend.material.mainTexture = new Texture2D(512, 512, TextureFormat.DXT5, false);
                    rend.material.mainTexture = tex;
                    rend.material.color = new Color(1f, 1f, 1f, 1f);
                }
            }
            else
            {
                ObservableWWW.GetWWW(url).Subscribe(
                success =>
                {
                    if (rend)
                    {
                        rend.material.mainTexture = new Texture2D(512, 512, TextureFormat.RGB24, false);
                        rend.material.color = new Color(1f, 1f, 1f, 1f);
                        success.LoadImageIntoTexture((Texture2D)rend.material.mainTexture);

                        // Cache image
                        CacheFolderPath = Path.Combine(Application.dataPath, RelativeCachePath);

                        File.WriteAllBytes(CacheFolderPath + "_" + tile.TileTms.x + "_" + tile.TileTms.y + ".png", success.bytes);
                    }
                },
                error =>
                {
                    Debug.Log(error);
                });
            }
        }
    }
}
