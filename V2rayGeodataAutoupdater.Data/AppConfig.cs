using System;
using System.IO;

namespace V2rayGeodataAutoupdater.Data
{
    public class AppConfig : IEquatable<AppConfig>
    {
        public bool IsShowConsole { get; set; } = true;
        public bool IsUpdateGeoIp { get; set; } = false;
        public string GeoIpSource { get; set; } = string.Empty;
        public string GeoIpSourceReserve { get; set; } = string.Empty;
        public bool IsUpdateGeoSite { get; set; } = false;
        public string GeoSiteSource { get; set; } = string.Empty;
        public string GeoSiteSourceReserve { get; set; } = string.Empty;
        public bool IsLaunchClient { get; set; } = false;
        public string ClientPath { get; set; } = string.Empty;
        public string ClientParameter { get; set; } = string.Empty;
        public bool IsDataSaveElsewhere { get; set; } = false;
        protected string _dataSavePath = string.Empty;
        public string DataSavePath
        {
            get
            {
                if (IsDataSaveElsewhere)
                {
                    return ClientPath;
                }
                else
                {
                    return Path.GetDirectoryName(ClientPath);
                }
            }

            set
            {
                _dataSavePath = value;
            }
        }

        public AppConfig()
        {
        }

        public AppConfig(AppConfig source)
        {
            CopyValue(source);
        }

        public void CopyValue(AppConfig source)
        {
            IsShowConsole = source.IsShowConsole;
            IsUpdateGeoIp = source.IsUpdateGeoIp;
            GeoIpSource = source.GeoIpSource;
            GeoIpSourceReserve = source.GeoIpSourceReserve;
            IsUpdateGeoSite = source.IsUpdateGeoSite;
            GeoSiteSource = source.GeoSiteSource;
            GeoSiteSourceReserve = source.GeoSiteSourceReserve;
            IsLaunchClient = source.IsLaunchClient;
            ClientPath = source.ClientPath;
            ClientParameter = source.ClientParameter;
            IsDataSaveElsewhere = source.IsDataSaveElsewhere;
            DataSavePath = source.DataSavePath;
        }

        public bool Equals(AppConfig other)
        {
            return IsShowConsole == other.IsShowConsole && IsUpdateGeoIp == other.IsUpdateGeoIp
                && GeoIpSource == other.GeoIpSource && GeoIpSourceReserve == other.GeoIpSourceReserve
                && IsUpdateGeoSite == other.IsUpdateGeoSite && GeoSiteSource == other.GeoSiteSource
                && GeoSiteSourceReserve == other.GeoSiteSourceReserve && IsLaunchClient == other.IsLaunchClient
                && ClientPath == other.ClientPath && ClientParameter == other.ClientParameter
                && IsDataSaveElsewhere == other.IsDataSaveElsewhere && DataSavePath == other.DataSavePath;
        }

        public override bool Equals(object obj)
        {
            return (obj != null) && (obj is AppConfig) && Equals(obj as AppConfig);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
