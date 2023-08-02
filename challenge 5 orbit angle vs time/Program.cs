using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace challenge_5_orbit_angle_vs_time
{
    internal class Program
    {
        struct Planet
        {
            public string name;
            
            public double orbperiod; // Orbital period years
            public double ecc; // Eccentricity of orbit
            

            public Planet(string name, double orbperiod, double ecc)
            {
                this.name = name;
                this.orbperiod = orbperiod;
                this.ecc = ecc;
                

            }
        }
        static void Main(string[] args)
        {
            Planet pluto = new Planet("Pluto", 248.348, 0.25);
            Planet halleysComet = new Planet("Halley's Comet", 76,0.97);
            Planet body = halleysComet;

            int numOrbits = 3;
            double max_theta = numOrbits* 2 * Math.PI;
            double dtheta = 0.001;
            double[] times = new double[(int)(max_theta/dtheta)+1];
            double[] angles = new double[(int)(max_theta / dtheta) + 1];
            for (int i = 0; i < times.Length; i++)
            {
                double theta = i * dtheta;
                angles[i] = theta;
                double integrand = IntegrateSimpson(dtheta, 0, theta, Integral, body.ecc);
                double time = body.orbperiod * Math.Pow(1 - Math.Pow(body.ecc, 2), 1.5) * (1 / (2 * Math.PI)) * integrand; // t=((1-e^2)^(3/2)) * (P/2pi) * integrate(theta0 to theta) dtheta/(1-ecos(theta))^2
                times[i] = time;
                Console.WriteLine(Convert.ToString(theta) +" "+ Convert.ToString(time));
            }
            times[0] = 0;
            

            double[] eventimes = new double[times.Length];
            double maxtime = times[times.Length - 1];
            for(int i =0; i < times.Length; i++)
            {
                eventimes[i] = i * maxtime / eventimes.Length;
            }
            double[] angleFromTime = new double[eventimes.Length];
            for(int j = 0; j < eventimes.Length; j++)
            {
                int minindex = 0;
                double time = eventimes[j];
                while (time >= times[minindex]) minindex++;  // Find the highest value that is less than the time.   
                minindex--;
                int maxindex = minindex + 1;
                double fractionOfInterval = (time - times[minindex]) / (times[maxindex] - times[minindex]);
                angleFromTime[j] = fractionOfInterval * (angles[maxindex] - angles[minindex]) + angles[minindex];
                Console.WriteLine(minindex);
            }
            Plot plt = new Plot(1200, 1200);
            plt.AddScatter(eventimes, angleFromTime, markerSize:1, color:System.Drawing.Color.Red);

            double[] circleAngles = new double[eventimes.Length];
            for (int i = 0; i < eventimes.Length; i++)
            {
                circleAngles[i] =2*Math.PI* eventimes[i] / body.orbperiod;
            }
            plt.AddScatter(eventimes, circleAngles, markerSize: 1, color:System.Drawing.Color.Blue);
            plt.XLabel("Time (years)");
            plt.YLabel("Angle (rad)");
            plt.AddAnnotation("Red: "+body.name+"\nBlue: Circular orbit with same period as "+body.name);
            plt.Title("Angle vs time graph for "+body.name);
            plt.SaveFig("Angle vs time "+body.name+".png");
            Console.ReadKey();
            
        }

        static double IntegrateSimpson(double stripWidth, double lowerbound, double upperbound, Func<double, double, double> function, double ecc)
        {
            int numStrips = (int)((upperbound - lowerbound) / stripWidth);   // N = (b-a)/h
            if (numStrips % 2 == 1) numStrips += 1;  // N must be even
            stripWidth = (upperbound - lowerbound) / numStrips;  // Adjust strip width to compensate for rounding

            int[] coefficients = new int[numStrips + 1];
            coefficients[0] = 1;
            coefficients[numStrips] = 1;
            for(int i = 1; i<numStrips; i++)
            {
                if(i % 2 == 0)
                {
                    coefficients[i] = 2;
                }
                else
                {
                    coefficients[i] = 4;
                }
            }
            double output=0;
            

            for (int i = 0; i <= numStrips; i++)
            {
                double theta = lowerbound + (i * stripWidth);
                output += coefficients[i]*function(theta, ecc);
            }
            output *= stripWidth * 1 / 3;
            return output;
        }
        static double Integral(double theta, double ecc)
        {
            double output = 1 / Math.Pow((1 - ecc * Math.Cos(theta)),2) ;
            return output;
        }
    }
}