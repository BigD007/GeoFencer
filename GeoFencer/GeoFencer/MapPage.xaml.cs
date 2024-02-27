using GeoFencer.Renderers;
using GeoFencer.Services.Geofences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Map = Xamarin.Forms.Maps.Map;

namespace GeoFencer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MapPage : ContentPage
    {

        CancellationTokenSource cts;
        Position _position;
        private double _fencingZoneKM = 0.5;
        private string custPhoneNo = "08023779750";
        public MapPage()
        {
            InitializeComponent();

            DisplayCurLoc();


        }

        //async void getloc() => _position=await  DisplayCurLoc();
        async void DisplayCurLoc()
        {
            var agents = new List<CustomPin>();

            try
            {
                var p = new Position(36.632163215984164, 3.355993186168763);

                cts = new CancellationTokenSource();
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    p = new Position(location.Latitude, location.Longitude);
                    //MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromMeters(300));
                    //customMap.MoveToRegion(mapSpan);
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    //return p;
                }

                //CustomPin pin = new CustomPin
                //{
                //    Label = "Botosoft Technology",
                //    Address = "Omofade Crescent Omole Phase 1",
                //    Type = PinType.Place,
                //    Position = p,
                //    Name = "Xamarin",
                //    Url = "http://xamarin.com/about/",
                //    IconType = "cust"
                //};

                agents.Add(await GetLocationAsync(0, "Mr Alimensa", null, _fencingZoneKM, location, true, custPhoneNo));
                agents.Add(await GetLocationAsync(1, "Agent 007 Agbo", "30 Adeyemo Akapo Street Omole Phase 1", _fencingZoneKM, location, false));
                agents.Add(await GetLocationAsync(2, "Agent Babasola Ado", "2 Lateef Jakande Rd, Agidingbi 101233, Lagos", _fencingZoneKM, location, false));
                agents.Add(await GetLocationAsync(2, "Agbaakin Olusola", "3 Norus Cl, Isheri 100213, Omole Phase 1, Lagos", _fencingZoneKM, location, false));
                agents.Add(await GetLocationAsync(2, "Sarah Okpara", "178 Lola Holloway St, Omole Phase 1 101233, Ikeja", _fencingZoneKM, location, false));

                // Instantiate a Circle
                Circle circle = new Circle
                {
                    Center = p,
                    Radius = new Distance(_fencingZoneKM * 1000),
                    StrokeColor = Color.FromHex("#88FF0000"),
                    StrokeWidth = 8,
                    FillColor = Color.FromHex("#88FFC0CB")
                };
                CustomMap customMap = new CustomMap(agents)
                {
                    IsShowingUser = true

                };

                //  customMap.CustomPins = new List<CustomPin> { pin };
                customMap.MapElements.Add(circle);
                agents.ForEach(x => {
                    customMap.Pins.Add(x);
                });

                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(p, Distance.FromMiles(1.0)));

                // Add the Circle to the map's MapElements collection
                Content = customMap;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            //return new Position(36.632163215984164, 3.355993186168763);
        }

        async Task<CustomPin> GetLocationAsync(int id, string name, string address, double geoZone, Location currentLocation, bool isCust, string phoneNumber=null)
        {
            CustomPin pin = new CustomPin();
            Position p;
            Location addLoc = null;

            if (string.IsNullOrEmpty(address) & isCust)
            {
                p = new Position(currentLocation.Latitude, currentLocation.Longitude);
                pin = new CustomPin
                {
                    Label = name,
                    Address = address,
                    Type = PinType.Generic,
                    Position = p,
                    Name = name,
                    IconType =   "cust"  ,
                    AgentId = 0,
                    Url = phoneNumber,
                };
            }
            else
            {
                var geolocation = await Geocoding.GetLocationsAsync(address);
                if (geolocation != null)
                    addLoc = geolocation.FirstOrDefault();
            }

            if (addLoc != null)
            {
                pin = new CustomPin
                {
                    Label = name,
                    Address = address,
                    Type = PinType.Place,
                    Position = new Position(addLoc.Latitude, addLoc.Longitude),
                    Name = name,
                    Url = "http://xamarin.com/about/",
                    IconType = isCust ? "cust" : "agent",
                    AgentId = id,
                };

                var dist = LocationExtensions.CalculateDistance(currentLocation, addLoc, DistanceUnits.Kilometers);
                if (Math.Abs(dist) > geoZone)
                    pin.IsInRange = false;
                else
                    pin.IsInRange = true;
            }

            return pin;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IGeofencesService GeofencesService = DependencyService.Get<IGeofencesService>();

            //List<Position> list = GetGeofences();
            //Location location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(3)));
            //GeofencesService.SetGeofences(list, new Position(location.Latitude, location.Longitude))
            //GeofencesService.SetGeofences(list, new Position(6.6392508, 3.3505177));

        }

        //    private List<Position> GetGeofences()
        //    {
        //        return new List<Position>
        //        {
        //            new Position(36.681062, -6.139933), // Alcazar, Jerez de la Frontera
        //new Position(40.416905, -3.703512), // Puerta del Sol, Madrid
        //new Position(0, 0),
        //        };
        //    }

    }
}