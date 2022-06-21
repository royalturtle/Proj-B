using System;
using UnityEngine;

public static class MathUtils {
    public static float Logistic(float x, float L=1.0f, float k=1.0f, float t=0.0f) {
        return L / (1 + Mathf.Exp(-k * (x - t)));
    }

    public static double Linear(double x, double a, double b) {
        return x * a + b;
    }

    public static double Logarithmic(double x, double a, double b) {
        return Math.Log10(x) * a + b;

    }

    public static double Exponential(double x, double a, double b) {
        return a * Math.Pow(b, (int)x);
    }

    public static double Square(double x1, double x2) {
        return Math.Sqrt(x1 * x2);
    }
}
