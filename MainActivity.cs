using Android.App;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using Android.Content;
using Android.Icu.Util;
using System.Collections.Generic;
using System.Text;

namespace alarmTest
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btn_set_alarm;
        TextView txt_timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Init();
        }

        private void Init()
        {
            btn_set_alarm = FindViewById<Button>(Resource.Id.btnTimer);
            txt_timer = FindViewById<TextView>(Resource.Id.txt_timer);

            //Event
            btn_set_alarm.Click += delegate
            {
                SetAlarm(10);
            };
        }

        private void SetAlarm(int number)
        {
            var am = GetSystemService(Context.AlarmService) as AlarmManager;
            for (int i = 1; i <= number; i++)
            {
                var requestCode = 200 + i;
                var diff = i * 10;
                var timer = DateTime.Now.AddSeconds(diff);
                var fireTime= DateTimeOffset.Now.AddSeconds(diff).ToUnixTimeMilliseconds();
                txt_timer.Text +=  $" \n {requestCode} => {timer.ToString("hh:mm:ss")} => {fireTime}";

                var intent = new Intent(this, typeof(MyAlarmReceiver));
                intent.PutExtra("REQUEST_CODE", requestCode);
                intent.AddFlags(ActivityFlags.IncludeStoppedPackages);
                intent.AddFlags(ActivityFlags.ReceiverForeground);
                var pi = PendingIntent.GetBroadcast(this, requestCode, intent, 0);

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
                    am.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, fireTime, pi);
                else
                    am.SetExact(AlarmType.RtcWakeup, fireTime, pi);
            }

            txt_timer.Text += "\n timer set";
            Toast.MakeText(this, "Alarm has been set", ToastLength.Short).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}