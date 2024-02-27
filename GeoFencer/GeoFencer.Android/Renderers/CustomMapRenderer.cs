using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GeoFencer.Droid.Renderers;
using GeoFencer.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GeoFencer.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<CustomPin> _customPins;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                _customPins = formsMap.CustomPins;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //if (e.PropertyName.Equals("VisibleRegion") && !_isDrawn)
            //{
            //    PopulateMap();
            //    OnGoogleMapReady();
            //}
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            //

            var customPin = (Element as CustomMap).CustomPins.FirstOrDefault(p => p.Position == pin.Position);
            if (customPin != null)
            {
                if (customPin.IconType == "cust")
                {
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
                }
                else/* if (customPin.IconType == "agent")*/
                {
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.agents));
                }
            }
            else
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
            return marker;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            if (!string.IsNullOrWhiteSpace(customPin.Url))
            {
                var url = Android.Net.Uri.Parse(customPin.Url);
                var intent = new Intent(Intent.ActionView, url);
                intent.AddFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }
                if (customPin.IconType == "cust")
                    return null;

                //if (customPin.Name.Equals("Xamarin"))
                //{
                //    view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);
                //}
                //else
                //{
                //    view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                //}

                view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);

                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoImage = view.FindViewById<ImageView>(Resource.Id.InfoWindowImage);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
                var infoAccept = view.FindViewById<TextView>(Resource.Id.InfoWindowAccept);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }
                if (infoAccept != null)
                {
                    infoAccept.Text = customPin.IsInRange?$"Tap to receive a call from {customPin.Name}":"This agent is not within your location. Please call 0800EnterpriseLife for our call center experience";
                    if (customPin.IsInRange)
                        infoAccept.SetTextColor(Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Blue));
                    else
                        infoAccept.SetTextColor(Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red));
                }

                //if (infoImage != null)
                //{
                //    string imgPath = DownloadImage(customPin.Url).Result;
                //    Android.Net.Uri ImagePath = Android.Net.Uri.Parse(imgPath);
                //    infoImage.SetImageURI(ImagePath);
                //}

                return view;
            }
            return null;
        }

        public async Task<string> DownloadImage(string webUrl)
        {
            var client = new WebClient();
            byte[] imageData = await client.DownloadDataTaskAsync(webUrl);
            var documentspath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var localfilename = "image.jpg";
            var localpath = Path.Combine(documentspath, localfilename);
            File.WriteAllBytes(localpath, imageData);
            return localpath;
        }
   
    //        private Bitmap getBitmapFromUri(Uri uri, Context context) throws IOException
    //        {
    //            ParcelFileDescriptor parcelFileDescriptor =
    //            context.getContentResolver().openFileDescriptor(uri, "r");
    //        FileDescriptor fileDescriptor = parcelFileDescriptor.getFileDescriptor();
    //        Bitmap image = BitmapFactory.decodeFileDescriptor(fileDescriptor);
    //        parcelFileDescriptor.close();
    //    return image;
    //}

    public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in _customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}