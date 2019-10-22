using Scannit.ViewModels;
using SkiaSharp;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Scannit.Views
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        private const string NoCardDataName = "NoCardData";
        private const string CardDataName = "CardData";
        private MainViewModel Viewmodel;

        public MainPage()
        {
            InitializeComponent();
            if (!App.IsInDesignMode)
            {
                Viewmodel = (MainViewModel)Startup.ServiceProvider.GetService(typeof(MainViewModel));
                BindingContext = Viewmodel;
            }
            GoToState(NoCardDataName);
            Viewmodel.PropertyChanged += Viewmodel_PropertyChanged;
        }

        private void Viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Viewmodel.Card))
            {
                if (Viewmodel.Card != null)
                {
                    GoToState(CardDataName);
                }
                else
                {
                    GoToState(NoCardDataName);
                }
            }
        }

        private async void SettingsButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private void GoToState(string stateName)
        {
            VisualStateManager.GoToState(CardDataStackLayout, stateName);
            VisualStateManager.GoToState(NoCardDataGrid, stateName);
        }

        private void SkiaCanvas_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            SKPaint paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Blue,
                StrokeWidth = 5,
                StrokeCap = SKStrokeCap.Square,
                PathEffect = SKPathEffect.CreateDash(new float[] { 30, 10 }, 0)
            };

            SKPath path = new SKPath();
            SKRoundRect roundRect = new SKRoundRect(new SKRect(info.Width * .1f, info.Height * .2f, info.Width * .9f, info.Height * .8f), 10, 10);
            path.AddRoundRect(roundRect);
            canvas.DrawPath(path, paint);
        }
    }
}
