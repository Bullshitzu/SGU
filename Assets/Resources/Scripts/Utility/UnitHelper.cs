using UnityEngine;
using System.Collections;

public partial class Utility {
    public class UnitHelper {

        public class Fahrenheit {
            public static double ToKelvin (double fahrenheitDeg) {
                return (fahrenheitDeg + 459.67) * 5f / 9f;
            }
            public static double ToCelsius (double fahrenheitDeg) {
                return (fahrenheitDeg + -32) * 5f / 9f;
            }
        }
        public class Kelvin {
            public static double ToFahrenheit (double kelvinDeg) {
                return kelvinDeg * 9f / 5f - 459.67;
            }
            public static double ToCelsius (double kelvinDeg) {
                return kelvinDeg - 273.15;
            }
        }
        public class Celsius {
            public static double ToFahrenheit (double celsiusDeg) {
                return celsiusDeg * 9f / 5f + 32;
            }
            public static double ToKelvin (double celsiusDeg) {
                return celsiusDeg + 273.15;
            }
        }

    }
}