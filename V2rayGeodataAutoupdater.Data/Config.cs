using System;

namespace V2rayGeodataAutoupdater.Data
{
    public class Config
    {
        public bool IsShowConsole { get; set; } = true;
        public bool IsUpdateGeoIp { get; set; } = false;
        public string GeoIpSource { get; set; } = string.Empty;
        public string GeoIpSourceReserve { get; set; } = string.Empty;
        public bool IsUpdateGeoSite { get; set; } = false;
        public string GeoSiteSource { get; set; } = string.Empty;
        public string GeoSiteSourceReserve { get; set; } = string.Empty;
        public bool IsLanchClient { get; set; } = false;
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
                    return _dataSavePath;
                }
                else
                {
                    return ClientPath;
                }
            }

            set
            {
                _dataSavePath = value;
            }
        }
    }
}
