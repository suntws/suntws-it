using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTS.cargomanagement
{
    public static class MathFunctions
    {
        public static double GetVolume(double l, double w, double h)
        {
            try
            {
                return (l * w * h);
            }
            catch (ArithmeticException e)
            {
                throw e;
            }
        }

        public static double GetPercentage(double X_is, double Y_of)
        {
            try
            {
                return (X_is / Y_of) * 100;
            }
            catch (ArithmeticException e)
            {
                throw e;
            }
        }
    }
}