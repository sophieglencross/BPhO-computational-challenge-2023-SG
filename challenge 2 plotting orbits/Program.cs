using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace challenge_2_plotting_orbits
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
            public double ecc; // Eccentricity of orbit

            public Planet(string name, double mass, double smaxis, double radius, double rotperiod, double orbperiod, double ecc)
            {
                this.name = name;
                this.mass = mass;
                this.smaxis = smaxis;
                this.radius = radius;
                this.rotperiod = rotperiod;
                this.orbperiod = orbperiod;
                this.ecc = ecc;

            }
        }
        static void Main(string[] args)
        {
            Planet Mercury = new Planet("Mercury", 0.055, 0.387, 0.38, 58.65, 0.24, 0.21);
            Planet Venus = new Planet("Venus", 0.815, 0.723, 0.95, 243.02, 0.62, 0.01);
            Planet Earth = new Planet("Earth", 1.000, 1.000, 1.00, 1.00, 1.00, 0.02);
            Planet Mars = new Planet("Mars", 0.107, 1.523, 0.53, 1.03, 1.88, 0.09);
            Planet Jupiter = new Planet("Jupiter", 317.85, 5.20, 11.21, 0.41, 11.86, 0.05);
            Planet Saturn = new Planet("Saturn", 95.16, 9.58, 9.45, 0.44, 29.63, 0.06);
            Planet Uranus = new Planet("Uranus", 14.50, 19.29, 4.01, 0.72, 84.75, 0.05);
            Planet Neptune = new Planet("Neptune", 17.20, 30.25, 3.88, 0.67, 166.34,0.01);

            Planet[] planets = { Mercury, Venus, Earth, Mars };// Jupiter, Saturn, Uranus, Neptune };

            ScottPlot.Plot plt = new ScottPlot.Plot(600, 600);
            foreach(Planet body in planets)
            {
                double[] xpoints = new double[1000];
                double[] ypoints = new double[1000];
                for (int step = 0; step<1000; step++)
                {
                    double theta = (2 * Math.PI * step) / 1000;// Angle have gone round of full circle
                    double r = (body.smaxis * (1 - Math.Pow(body.ecc, 2)))/(1-(body.ecc*Math.Cos(theta))); // Polar equation of an ellipse r= a(1-e^2)/(1-e cos(t))
                    xpoints[step] = r * Math.Cos(theta);
                    ypoints[step] = r * Math.Sin(theta);
                }
                plt.AddScatter(xpoints, ypoints, markerSize: 2);
                
            }
            plt.XLabel("AU");
            plt.YLabel("AU");
            plt.Title("Inner Planets' Orbits");
            plt.AddMarker(0, 0, MarkerShape.filledCircle, 10, System.Drawing.Color.Yellow);  // Add circle for the sun
            plt.SaveFig("innerorbits.png");
        }
    }
}
