using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.OS;
using Android.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scannit.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Scannit.Droid
{
    [Activity(Label = "Scannit",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionMain, "android.nfc.action.TECH_DISCOVERED" },
        Categories = new[] { Intent.CategoryLauncher })]
    [MetaData("android.nfc.action.TECH_DISCOVERED", Resource = "@xml/nfc_tech_filter")]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private Scannit.App _crossPlatApp;
        private NfcAdapter _nfcAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            Startup.Init(ConfigureServices);
            _crossPlatApp = new App();
            LoadApplication(_crossPlatApp);

            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);
            if (_nfcAdapter == null)
            {
                // TODO: Tell the main app that sadness is in our future

            }

            if (_nfcAdapter.IsEnabled == false)
            {
                // TODO: Tell the main app that sadness is in the future unlessa action is taken.
            }

            HandleIntent(Intent);
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {

        }

        protected override void OnResume()
        {
            base.OnResume();
            SetupForegroundDispatch(this, _nfcAdapter);
        }

        protected override void OnPause()
        {
            StopForegroundDispatch(this, _nfcAdapter);
            base.OnPause();
        }

        protected override void OnNewIntent(Intent intent)
        {
            HandleIntent(intent);
        }

        private void HandleIntent(Intent intent)
        {
           if (intent.Action == NfcAdapter.ActionTechDiscovered)
            {
                Tag tag = (Tag)intent.GetParcelableExtra(NfcAdapter.ExtraTag);
                IsoDep card = IsoDep.Get(tag);
                AndroidSmartCard smartCard = new AndroidSmartCard(card);
                MessagingCenter.Send(new CardAddedMessage { Card = smartCard }, "");
            }
        }

        public static void SetupForegroundDispatch(MainActivity mainActivity, NfcAdapter nfcAdapter)
        {
            if (nfcAdapter == null)
            {
                return;
            }

            Intent intent = new Intent(mainActivity.ApplicationContext, mainActivity.Class);
            intent.SetFlags(ActivityFlags.SingleTop);
            PendingIntent pendingIntent = PendingIntent.GetActivity(mainActivity.ApplicationContext, 0, intent, 0);

            IntentFilter[] filters = new IntentFilter[1];
            string[][] techList = new string[][]
            {
                new string[] {Java.Lang.Class.FromType(typeof(IsoDep)).Name}
            };

            // Notice that this is the same filter as in our manifest.
            filters[0] = new IntentFilter();
            filters[0].AddAction(NfcAdapter.ActionTechDiscovered);
            filters[0].AddCategory(Intent.CategoryDefault);

            nfcAdapter.EnableForegroundDispatch(mainActivity, pendingIntent, filters, techList);
        }

        private void StopForegroundDispatch(MainActivity mainActivity, NfcAdapter nfcAdapter)
        {
            nfcAdapter?.DisableForegroundDispatch(mainActivity);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}