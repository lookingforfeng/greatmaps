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

        //public override PureImage GetTileImage(GPoint pos, int zoom)
        //{
        //    try
        //    {
        //        string url = MakeTileImageUrl(pos, zoom, LanguageStr);
        //        return GetTileImageUsingHttp(url);

        //        //PureImageProxy TileImageProxy;
        //        //MemoryStream data = Stuff.CopyStream(responseStream, false);
        //        //PureImage ret = TileImageProxy.FromStream(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = MakeTileImageUrl(pos, zoom, LanguageStr);
                return GetTileImageUsingHttp(url);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            var num = (pos.X + pos.Y) % 4 + 1;
            //string url = string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
            string url = string.Format(UrlFormat, pos.X, pos.Y, zoom);
            return url;
        }

        //static readonly string UrlFormat = "http://webrd04.is.autonavi.com/appmaptile?x={0}&y={1}&z={2}&lang=zh_cn&size=1&scale=1&style=7";
        static readonly string UrlFormat = "http://webrd01.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={0}&y={1}&z={2}";
    }

}