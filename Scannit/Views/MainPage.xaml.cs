using Scannit.ViewModels;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace Scannit.Views
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        private const string NoCardDataName = "NoCardData";
        private const string CardDataName = "CardData";
        private const string ThrobAnimationName = "ThrobAnimation";
        private Animation _promptThrobAnimation;
        private MainViewModel ViewModel;

        public MainPage()
        {
            InitializeComponent();
            ViewModel = (MainViewModel)Startup.ServiceProvider.GetService(typeof(MainViewModel));
            BindingContext = ViewModel;
            GoToState(NoCardDataName);
            ViewModel.PropertyChanged += Viewmodel_PropertyChanged;
            _promptThrobAnimation = new Animation
            {
                { 0, 0.5, new Animation(val => CanvasContainer.Scale = val, 1, 1.1) },
                { 0.5, 1.0, new Animation(val => CanvasContainer.Scale = val, 1.1, 1) }
            };
            _promptThrobAnimation.Commit(CanvasContainer, ThrobAnimationName, rate: 16, length: 3000, repeat: () => true);
#if DEBUG
            this.ToolbarItems.Add(new ToolbarItem("DEBUG Add Card", null, OnDebugAddCardClicked, ToolbarItemOrder.Primary));
#endif
        }

        private void Viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Card))
            {
                if (ViewModel.Card != null)
                {
                    GoToState(CardDataName);
                    CanvasContainer.AbortAnimation(ThrobAnimationName);
                }
                else
                {
                    GoToState(NoCardDataName);
                    CanvasContainer.Animate(ThrobAnimationName, _promptThrobAnimation, rate: 16, length: 3000, repeat: () => true);
                }
            }
        }

        private async void SettingsButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private void GoToState(string stateName)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                VisualStateManager.GoToState(CardDataStackLayout, stateName);
                VisualStateManager.GoToState(NoCardDataGrid, stateName);
            });
        }

        private void SkiaCanvas_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            // Draw the four corners of the "card outline".
            Debug.WriteLine("Skia painting.");
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
                StrokeJoin = SKStrokeJoin.Round
            };

            SKPath path = new SKPath();

            const float cornerOffset = 50f;
            // Top left
            SKPoint tlBottomLeft = new SKPoint(info.Width * 0.1f, info.Height * 0.1f + cornerOffset);
            SKPoint tlTopLeft = new SKPoint(info.Width * 0.1f, info.Height * 0.1f);
            SKPoint tlTopRight = new SKPoint(info.Width * 0.1f + cornerOffset, info.Height * 0.1f);
            path.MoveTo(tlBottomLeft);
            path.CubicTo(tlTopLeft, tlTopLeft, tlTopRight);

            // Top right
            SKPoint trTopLeft = new SKPoint(info.Width * 0.9f - cornerOffset, info.Height * 0.1f);
            SKPoint trTopRight = new SKPoint(info.Width * 0.9f, info.Height * 0.1f);
            SKPoint trBottomRight = new SKPoint(info.Width * 0.9f, info.Height * 0.1f + cornerOffset);
            path.MoveTo(trTopLeft);
            path.CubicTo(trTopRight, trTopRight, trBottomRight);

            // Bottom right
            SKPoint brTopRight = new SKPoint(info.Width * 0.9f, info.Height * 0.9f - cornerOffset);
            SKPoint brBottomRight = new SKPoint(info.Width * 0.9f, info.Height * 0.9f);
            SKPoint brBottomLeft = new SKPoint(info.Width * 0.9f - cornerOffset, info.Height * 0.9f);
            path.MoveTo(brTopRight);
            path.CubicTo(brBottomRight, brBottomRight, brBottomLeft);

            // Bottom left
            SKPoint blBottomRight = new SKPoint(info.Width * 0.1f + cornerOffset, info.Height * 0.9f);
            SKPoint blBottomLeft = new SKPoint(info.Width * 0.1f, info.Height * 0.9f);
            SKPoint blTopLeft = new SKPoint(info.Width * 0.1f, info.Height * 0.9f - cornerOffset);
            path.MoveTo(blBottomRight);
            path.CubicTo(blBottomLeft, blBottomLeft, blTopLeft);

            canvas.DrawPath(path, paint);
        }

        private void OnDebugAddCardClicked()
        {
            ViewModel.Card = new TravelCardViewModel(ScannitSharp.TravelCard.CreateTravelCard(
                appInfo: new byte[0],
                controlInfo: new byte[0],
                periodPass: new byte[0],
                storedValue: new byte[0],
                eTicket: new byte[0],
                history: new byte[0]
            ));
        }
    }
}
