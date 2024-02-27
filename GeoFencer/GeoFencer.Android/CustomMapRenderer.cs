//using System;
//using System.Collections.Generic;
//using Android.Content;
//using Android.Gms.Maps;
//using Android.Gms.Maps.Model;
//using Android.Widget;
//using GeoFencer;
//using GeoFencer.Droid;
//using Xamarin.Forms;
//using Xamarin.Forms.Maps;
//using Xamarin.Forms.Maps.Android;
//using static GeoFencer.Page1;

//[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
//namespace GeoFencer.Droid
//{
//    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
//    {
//        List<CustomPin> customPins;

//        public CustomMapRenderer(Context context) : base(context)
//        {
//        }

//        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
//        {
//            base.OnElementChanged(e);

//            if (e.OldElement != null)
//            {
//                NativeMap.InfoWindowClick -= OnInfoWindowClick;
//            }

//            if (e.NewElement != null)
//            {
//                var formsMap = (CustomMap)e.NewElement;
//                customPins = formsMap.CustomPins;
//            }
//        }

//        protected override void OnMapReady(GoogleMap map)
//        {
//            base.OnMapReady(map);

//            NativeMap.InfoWindowClick += OnInfoWindowClick;
//            NativeMap.SetInfoWindowAdapter(this);
//        }
   
//    }
//}
