using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassonne
{
    public class PanAndZoom
    {
        public PanAndZoom()
        {
            MaxZoom = 15;
            MinZoom = 0.15f;
        }
        private SKPoint offset = new(0,0);
        private float zoom = 1;
        public float Zoom
        {
            get => zoom;
        }
        public SKPoint Offset
        {
            get => offset;
        }
        public float MaxZoom { get; set; }
        public float MinZoom { get; set; }
        public void Pan(SKPoint p) => offset += p;
        public void ZoomAt(float amount, SKPoint c)
        {
            SKPoint InitialWorldPos = ScreenToWorld(c);
            zoom += amount;
            Console.WriteLine(MaxZoom);
            Console.WriteLine(MinZoom);
            Console.WriteLine(zoom);
            if (zoom > MaxZoom)
            {
                zoom = MaxZoom;
            }
            if (zoom < MinZoom)
            {
                zoom = MinZoom;
            }
            offset += c - WorldToScreen(InitialWorldPos);
        }
        public SKPoint WorldToScreen(SKPoint w)
        {
            SKPoint rv = w;
            rv.X *= zoom;
            rv.Y *= zoom;
            rv += offset;
            return rv;
        }
        public SKPoint ScreenToWorld(SKPoint s)
        {
            SKPoint rv = (s - offset);
            rv.X /= zoom;
            rv.Y /= zoom;
            return rv;
        }
    }
}
