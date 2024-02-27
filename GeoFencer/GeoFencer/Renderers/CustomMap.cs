using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace GeoFencer.Renderers
{
    public class CustomMap : Map
    {
        public CustomMap():this(new List<CustomPin> { })
        {
        }

        public CustomMap(List<CustomPin> customPins)
        {
            CustomPins = customPins;
        }

        public List<CustomPin> CustomPins { get; set; }
    }

    public class CustomPin : Pin
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string IconType { get; set; }
        public bool IsInRange { get; set; }
        public int AgentId { get;   set; }
    }
}
