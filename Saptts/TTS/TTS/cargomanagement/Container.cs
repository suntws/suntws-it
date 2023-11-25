using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTS.cargomanagement
{
    public class Container
    {
        private double length = 0.0, height = 0.0, width = 0.0, volume;
        private string containerType = "";

        #region properties
        public double GetLength { get { return length; } }
        public double GetHeight { get { return height; } }
        public double GetWidth { get { return width; } }
        public double GetVolume { get { return volume; } }
        public string GetName { get { return containerType; } }
        #endregion

        public Container(double len, double width, double height)
        {
            try
            {
                length = len;
                this.height = height;
                this.width = width;
                volume = MathFunctions.GetVolume(l: len, w: width, h: height);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}