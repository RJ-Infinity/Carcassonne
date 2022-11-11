using PGL;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Carcassonne
{
    public partial class Form1 : PGLForm
    {
        private Layer layer_Background;
        private Layer layer_Display;
        private Layer layer_HUD;
        private Layer layer_NCD;
        private Layer layer_Settings;
        private PanAndZoom PanAndZoom;
        private Dictionary<States, List<Layer>> StatesLayers = new();
        public Form1()
        {
            foreach(States state in Enum.GetValues(typeof(States)))
            {
                StatesLayers.Add(state, new());
            }
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            layer_Background = new("layer_Background");
            layer_Display = new("layer_Display");
            layer_HUD = new("layer_HUD");
            layer_NCD = new("layer_NCD");
            layer_Settings = new("layer_Settings");

            StatesLayers[States.Playing].Add(layer_Background);
            StatesLayers[States.Playing].Add(layer_Display);
            StatesLayers[States.Playing].Add(layer_HUD);
            StatesLayers[States.Playing].Add(layer_NCD);

            StatesLayers[States.Settings].Add(layer_Settings);

            layer_Background.Draw += Layer_Background_Draw;
            layer_Display.Draw += Layer_Display_Draw;
            layer_HUD.Draw += Layer_HUD_Draw;
            layer_NCD.Draw += Layer_NCD_Draw;
            layer_Settings.Draw += Layer_Settings_Draw;

            PanAndZoom = new PanAndZoom();
            PanAndZoom.MaxZoom = 2;

            SkiaSurface.MouseWheel += SkiaSurface_MouseWheel;
            SkiaSurface.MouseDown += SkiaSurface_MouseDown;

            base.OnLoad(e);

            setState(States.Playing);

            this.Text = "Carcassonne";
        }

        private void Layer_Settings_Draw(object? sender, EventArgs_Draw e)
        {
            using SKPaint paint = new();
            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Red;
            e.Canvas.DrawRect(e.Bounds, paint);
        }
        private void FindLocation(SKPoint pos)
        {

        }
        private void SkiaSurface_MouseDown(object? sender, MouseEventArgs e)
        {
            Location loc = FindLocation(e.Location.ToSKPoint());
        }

        private void setState(States state)
        {
            Layers.Clear();
            Layers.AddRange(StatesLayers[state]);
            foreach (Layer layer in Layers)
            {
                layer.Invalidate();
            }
            UpdateDrawing();
        }
        private void SkiaSurface_MouseWheel(object? sender, MouseEventArgs e)
        {
            PanAndZoom.ZoomAt(
                (e.Delta / (float)SystemInformation.MouseWheelScrollDelta) * 0.1f,
                e.Location.ToSKPoint()
            );
            layer_Background.Invalidate();
            layer_Display.Invalidate();
            layer_HUD.Invalidate();
            layer_NCD.Invalidate();
            UpdateDrawing();
        }

        private void Layer_NCD_Draw(object? sender, EventArgs_Draw e)
        {
        }

        private void Layer_HUD_Draw(object? sender, EventArgs_Draw e)
        {
        }

        private void Layer_Display_Draw(object? sender, EventArgs_Draw e)
        {
        }

        private void Layer_Background_Draw(object? sender, EventArgs_Draw e)
        {
            // Create a diagonal gradient fill from Blue to Green to use as the background
            SKPoint topLeft = new(e.Bounds.Left, e.Bounds.Top);
            SKPoint bottomRight = new(e.Bounds.Right, e.Bounds.Bottom);
            SKColor[] gradColors = new[] { SKColors.LightBlue, SKColors.LightGreen };

            using SKPaint gradientPaint = new();
            using SKShader shader = SKShader.CreateLinearGradient(topLeft, bottomRight, gradColors, SKShaderTileMode.Clamp);
            gradientPaint.Shader = shader;
            gradientPaint.Style = SKPaintStyle.Fill;
            e.Canvas.DrawRect(e.Bounds, gradientPaint);

            using SKPaint paint = new();
            paint.Color = SKColors.Gray; // Very dark gray
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 1;
            //e.Canvas.DrawLine(new(0,0), new(100,100), paint);
            // Draw the Horizontal Grid Lines
            int i = ((int)PanAndZoom.Offset.Y % (int)(100 * PanAndZoom.Zoom));
            while (i < e.Bounds.Height)
            {
                SKPoint leftPoint = new(e.Bounds.Left, i);
                SKPoint rightPoint = new(e.Bounds.Right, i);

                e.Canvas.DrawLine(leftPoint, rightPoint, paint);

                i += (int)(100 * PanAndZoom.Zoom);
            }

            // Draw the Vertical Grid Lines
            i = ((int)PanAndZoom.Offset.X % (int)(100 * PanAndZoom.Zoom));
            while (i < e.Bounds.Width)
            {
                SKPoint topPoint = new(i, e.Bounds.Top);
                SKPoint bottomPoint = new(i, e.Bounds.Bottom);

                e.Canvas.DrawLine(topPoint, bottomPoint, paint);

                i += (int)(100 * PanAndZoom.Zoom);
            }
        }
        private enum States
        {
            Playing,
            PlayMenu,
            Settings,
        }
        private struct Location
        {
        }
    }
}