using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace computational_challenge_1_keplers_third_law
{
    internal class Program
    {
        struct Planet
        {
            public string name;
            public double mass; // Earth masses
            public double smaxis; // Semi major axis of orbit AU
            public double radius; // Radius of planet
            public double rotperiod; // Rotational period days
            public double orbperiod; // Orbital period years

            public Planet(string name, double mass, double smaxis, double radius, double rotperiod, double orbperiod)
            {
                this.name = name;
                this.mass = mass;
                this.smaxis = smaxis;
                this.radius = radius;
                this.rotperiod = rotperiod;
                this.orbperiod = orbperiod;

            }
        }
        static void Main(string[] args)
        {
            Planet Mercury = new Planet("Mercury", 0.055, 0.387, 0.38, 58.65, 0.24);
            Planet Venus = new Planet("Venus", 0.815, 0.723, 0.95, 243.02, 0.62);
            Planet Earth = new Planet("Earth", 1.000, 1.000, 1.00, 1.00, 1.00);
            Planet Mars = new Planet("Mars", 0.107, 1.523, 0.53, 1.03, 1.88);
            Planet Jupiter = new Planet("Jupiter", 317.85, 5.20, 11.21, 0.41, 11.86);
            Planet Saturn = new Planet("Saturn", 95.16, 9.58, 9.45, 0.44, 29.63);
            Planet Uranus = new Planet("Uranus", 14.50, 19.29, 4.01, 0.72, 84.75);
            Planet Neptune = new Planet("Neptune", 17.20, 30.25, 3.88, 0.67, 166.34);

            //Planet[] planets = { Mercury, Venus, Earth, Mars };
            Planet[] planets = { Jupiter, Saturn, Uranus, Neptune };
            double[] t_squared = new double[planets.Length];
            double[] r_cubed = new double[planets.Length];
            for (int i = 0; i < planets.Length; i++)
            {
                t_squared[i] = Math.Pow(planets[i].orbperiod, 2);
                r_cubed[i] = Math.Pow(planets[i].smaxis, 3);
            }

            ScottPlot.Plot plt = new ScottPlot.Plot(600, 400);
            ScottPlot.Statistics.LinearRegressionLine line = new ScottPlot.Statistics.LinearRegressionLine(r_cubed, t_squared);
            plt.Title("Kepler's Third Law for Outer planets");
            plt.AddScatter(r_cubed, t_squared);
            plt.AddLine(line.slope, line.offset, (r_cubed.Min(), r_cubed.Max()));
            plt.XLabel("Radius of orbit (AU) cubed");
            plt.YLabel("Time period (years) squared");

            plt.SaveFig("Keplers3outer.png");

        }

    }
}
