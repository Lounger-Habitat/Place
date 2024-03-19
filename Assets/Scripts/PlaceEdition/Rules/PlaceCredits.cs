using System;
using UnityEngine;

public static class PlaceCredits
{
    public static GaussianFunction gaussianFunction = new GaussianFunction(0, 1, 0.6);
    // 计算游戏积分
    public static int CalculateScore(float teamScore)
    {
        int score = 0;
        score = (int)teamScore;
        return score;
    }

    public static int GS_Calculate(int A , int B)
    {
        // must A >= B 
        int diff = A - B;
        double ratio = 0;
        if (diff == 0)
        {
            ratio = 0;
        }else if (diff > 0)
        {
            ratio = ((double)A / (double)B) - 1;
        }else if (diff < 0)
        {
            Debug.Log("错误");
            ratio = 0;
        }
        double beta = gaussianFunction.Calculate(ratio);
        return (int)((double)A - beta * (double)B);
    }

    
}


public class GaussianFunction
{
    private double mu; // 均值
    private double sigma; // 标准差
    private double bias; // 一点偏置

    // 构造函数，初始化均值和标准差
    public GaussianFunction(double mu, double sigma,double bias)
    {
        this.mu = mu;
        this.sigma = sigma;
        this.bias = bias;
    }

    // 计算高斯函数值的方法
    public double Calculate(double x)
    {
        double denominator = sigma * Math.Sqrt(2 * Math.PI);
        double exponent = -Math.Pow(x - mu, 2) / (2 * Math.Pow(sigma, 2));
        return (1 / denominator) * Math.Exp(exponent) + bias;
    }
}