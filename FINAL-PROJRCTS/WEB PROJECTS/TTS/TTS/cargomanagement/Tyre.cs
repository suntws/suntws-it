using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTS.cargomanagement
{
    public enum DimensionsIn { mm, cm, inch, ft, meter }
    public class Tyre
    {
        private double outerDia = 0.0, innDia = 0.0, treadWidth = 0.0, volume = 0.0, totVolume = 0.0;
        private int qnty, u_Id;
        private string tyreType = "";
        private string config, tyresize, brand, sidewall, rimsize;
        private double wrapWidth = 0.0;
        private bool hasRim = false;
        public int InTyreId = 0; 
        public int OutTyreId = 0;

        #region properties
        public string Config { get { return config; } set { config = value; } }
        public string Tyresize { get { return tyresize; } set { tyresize = value; } }
        public string Brand { get { return brand; } set { brand = value; } }
        public string Sidewall { get { return sidewall; } set { sidewall = value; } }
        public string Rimsize { get { return rimsize; } set { rimsize = value; } }
        public double GetOuterDia { get { return outerDia; } }
        public double GetInnerDia { get { return innDia; } }
        public double GetWidth { get { return treadWidth; } }
        public double GetEachVolume { get { return volume; } }
        public string GetTyreType { get { return tyreType; } }
        public int GetQuantity { get { return qnty; } }
        public double GetTotalVolume { get { return totVolume; } }
        public int GetU_Id { get { return u_Id; } }
        public int SetU_Id { set { u_Id = value; } }
        public double GetTreadWidth { get { return treadWidth; } }
        public int SetQuantity
        {
            set
            {
                qnty = value;
                totVolume = value * volume;
            }

        }
        public double WrapWidth { get { return wrapWidth; } set { wrapWidth = value; } }
        public bool HasRim { get { return hasRim; } set { hasRim = value; } } 

        #endregion

        public Tyre(double OD, double TW, double SW, string tyreType, DimensionsIn unit, int qty, double weight = 0)
        {
            this.tyreType = tyreType;
            treadWidth = TW;
            qnty = qty;
            switch (unit) // convert unit to mm if not in mm by default
            {
                case DimensionsIn.mm:
                    outerDia = OD;
                    innDia = getInnDia(SW, OD);
                    break;
                case DimensionsIn.cm:
                    SW = SW * 10;
                    OD = OD * 10;
                    outerDia = OD;
                    innDia = getInnDia(SW, OD);
                    break;
                case DimensionsIn.inch:
                    SW = SW * 25.4;
                    OD = OD * 25.4;
                    outerDia = OD;
                    innDia = getInnDia(SW, OD);
                    break;
                case DimensionsIn.ft:
                    SW = SW * 304.8;
                    OD = OD * 304.8;
                    outerDia = OD;
                    innDia = getInnDia(SW, OD);
                    break;
                case DimensionsIn.meter:
                    SW = SW * 1000;
                    OD = OD * 1000;
                    outerDia = OD;
                    innDia = getInnDia(SW, OD);
                    break;
            }
            volume = MathFunctions.GetVolume(l: OD, w: TW, h: OD);
            totVolume = volume * qty;
        }

        public Tyre(Tyre tyre)
        {
            this.outerDia = tyre.GetOuterDia;
            this.innDia = tyre.GetInnerDia;
            this.volume = tyre.GetEachVolume;
            this.totVolume = tyre.GetTotalVolume;
            this.qnty = tyre.GetQuantity;
            this.tyreType = tyre.GetTyreType;
            this.u_Id = tyre.GetU_Id;
            this.treadWidth = tyre.GetTreadWidth;
            this.config = tyre.config;
            this.brand = tyre.brand;
            this.tyresize = tyre.tyresize;
            this.sidewall = tyre.sidewall;
            this.rimsize = tyre.rimsize;
        }

        private double getInnDia(double sidewallWidth, double outDia)
        {
            return outerDia - (2 * sidewallWidth);
        }
    }
}