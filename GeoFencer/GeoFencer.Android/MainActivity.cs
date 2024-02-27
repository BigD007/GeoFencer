namespace GeoFencer.Droid
{
    using Android;
    using Android.App;
	using Android.Content;
	using Android.Content.PM;
	using Android.OS;
	using Android.Runtime;
	using System;
	using Android.Gms.Maps;
	using Android.Gms.Maps.Model;
	using Android.Graphics;
	using GeoFencer;
    using GeoFencer.Droid;

    [Activity(Label = "GeoFencer", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		public static Context MainContext { get; set; }

		private GoogleMap googleMap;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);
			MainContext = this;

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			global::Xamarin.FormsMaps.Init(this, savedInstanceState);	
			LoadApplication(new App());
		}

		const int RequestLocationId = 0;

		readonly string[] LocationPermissions =
		{
			Manifest.Permission.AccessCoarseLocation,
			Manifest.Permission.AccessFineLocation
		};

		protected override void OnStart()
		{
			base.OnStart();

			if ((int)Build.VERSION.SdkInt >= 23)
			{
				if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
				{
					RequestPermissions(LocationPermissions, RequestLocationId);
				}
				else
				{
					// Permissions already granted - display a message.
				}
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}