namespace GMap.NET.MapProviders.LocalTileMap_google
{
    using System;
    using GMap.NET;
    using GMap.NET.MapProviders;
    using Projections;
    using System.IO;

    public abstract class LocalTileMapProviderBase : GMapProvider
    {
        public LocalTileMapProviderBase()
        {
            MaxZoom = null;
            RefererUrl = "http://www.amap.com/";
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { this };
                }
                return overlays;
            }
        }
    }

    public class LocalTileMapProvider : LocalTileMapProviderBase
    {
        public static readonly LocalTileMapProvider Instance;

        readonly Guid id = new Guid("F8C8CA39-7C20-4B3F-A41E-A4A15465B7A0");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "LocalTileMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static LocalTileMapProvider()
        {
            Instance = new LocalTileMapProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            //try
            //{
            //string url = MakeTileImageUrl(pos, zoom, LanguageStr);
            //return GetTileImageUsingHttp(url);
            return ReadImageFile(pos, zoom);

            //MemoryStream data = Stuff.CopyStream(responseStream, false);
            //PureImage ret = TileImageProxy.FromStream(data);
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }

        public PureImage ReadImageFile(GPoint pos, int zoom)
        {
            string tile_file = string.Format(tile_file_path, zoom, pos.X, pos.Y);
            if (!File.Exists(tile_file))
            {
                return null;//文件不存在
            }
            //FileStream fs = File.OpenRead(tile_file); //OpenRead

            byte[] data = File.ReadAllBytes(tile_file);
            MemoryStream ms = new MemoryStream(data);

            //int filelength = 0;
            //filelength = (int)fs.Length; //获得文件长度 
            //Byte[] image = new Byte[filelength]; //建立一个字节数组 
            //fs.Read(image, 0, filelength); //按字节流读取 

            PureImage ret = GMapProvider.TileImageProxy.FromStream(ms);
            if (ms.Length > 0)
            {
                ret = TileImageProxy.FromStream(ms);

                if (ret != null)
                {
                    ret.Data = ms;
                    ret.Data.Position = 0;
                }
                else
                {
                    ms.Dispose();
                }
            }
            ms = null;

            //fs.Close();
            return ret;
            //Bitmap bit = new Bitmap(result);
            //return bit;
        }

        static readonly string tile_file_path = "C:\\Users\\Administrator\\Desktop\\234\\123123_瓦片：谷歌\\{0}\\{1}\\{2}.png";

        //        public override PureImage GetTileImage(GPoint pos, int zoom)
        //{
        //    try
        //    {
        //        string url = MakeTileImageUrl(pos, zoom, LanguageStr);
        //        return GetTileImageUsingHttp(url);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //string MakeTileImageUrl(GPoint pos, int zoom, string language)
        //{
        //    var num = (pos.X + pos.Y) % 4 + 1;
        //    //string url = string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
        //    string url = string.Format(UrlFormat, pos.X, pos.Y, zoom);
        //    return url;
        //}

        ////static readonly string UrlFormat = "http://webrd04.is.autonavi.com/appmaptile?x={0}&y={1}&z={2}&lang=zh_cn&size=1&scale=1&style=7";
        //static readonly string UrlFormat = "http://webrd01.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={0}&y={1}&z={2}";
    }

}