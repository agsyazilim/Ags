using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Common
{
    public class HavaDurumuModel : BaseAgsModel
    {

        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public string visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public string dt { get; set; }
        public Sys sys { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string cod { get; set; }

        #region Nesnet
        public class Coord: BaseAgsModel
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Weather: BaseAgsModel
        {
            public string id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class Main: BaseAgsModel
        {
            public double temp { get; set; }
            public string pressure { get; set; }
            public string humidity { get; set; }
            public string temp_min { get; set; }
            public string temp_max { get; set; }
        }

        public class Wind: BaseAgsModel
        {
            public double speed { get; set; }
            public string deg { get; set; }
        }

        public class Clouds: BaseAgsModel
        {
            public string all { get; set; }
        }

        public class Sys: BaseAgsModel
        {
            public string type { get; set; }
            public string id { get; set; }
            public string message { get; set; }
            public string country { get; set; }
            public string sunrise { get; set; }
            public string sunset { get; set; }
        }


        #endregion



    }
   
}