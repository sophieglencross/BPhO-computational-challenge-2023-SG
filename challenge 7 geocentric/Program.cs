using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace challenge_7_geocentric
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
            public System.Drawing.Color colour;
            public double orbinclination; // Angle of orbit to ecliptic

            public Planet(string name, double mass, double smaxis, double radius, double rotperiod, double orbperiod, double ecc, double betadegrees, System.Drawing.Color colour)
            {
                this.name = name;
                this.mass = mass;
                this.smaxis = smaxis;
                this.radius = radius;
                this.rotperiod = rotperiod;
                this.orbperiod = orbperiod;
                this.ecc = ecc;
                this.orbinclination = Math.PI * 2 * betadegrees / 180;  // Convert degrees to radians
                this.colour = colour;


            }
        }
        static void Main(string[] args)
        {
            Planet Mercury = new Planet("Mercury", 0.055, 0.387, 0.38, 58.65, 0.24, 0.21, 7.00, System.Drawing.Color.Purple);
            Planet Venus = new Planet("Venus", 0.815, 0.723, 0.95, 243.02, 0.62, 0.01, 3.39, System.Drawing.Color.Pink);
            Planet Earth = new Planet("Earth", 1.000, 1.000, 1.00, 1.00, 1.00, 0.02, 0.00, System.Drawing.Color.Green);
            Planet Mars = new Planet("Mars", 0.107, 1.523, 0.53, 1.03, 1.88, 0.09, 1.85, System.Drawing.Color.Red);
            Planet Jupiter = new Planet("Jupiter", 317.85, 5.20, 11.21, 0.41, 11.86, 0.05, 1.31, System.Drawing.Color.Brown);
            Planet Saturn = new Planet("Saturn", 95.16, 9.58, 9.45, 0.44, 29.63, 0.06, 2.49, System.Drawing.Color.Orange);
            Planet Uranus = new Planet("Uranus", 14.50, 19.29, 4.01, 0.72, 84.75, 0.05, 0.77, System.Drawing.Color.Teal);
            Planet Neptune = new Planet("Neptune", 17.20, 30.25, 3.88, 0.67, 166.34, 0.01, 1.77, System.Drawing.Color.Blue);
            Planet Pluto = new Planet("Pluto", 0.003, 39.509, 0.187, 6.387, 248.348, 0.25, 17.5, System.Drawing.Color.BlueViolet);

            List<Planet> planets = new List<Planet> { Uranus, Neptune, Pluto };

            Planet centre = Uranus;
            if (planets.Contains(centre)) planets.Remove(centre);

            int numsteps = 1233; // No common factors with number of periods
            int numorbits = 30;
            double maxtime = numorbits * (planets.Max(planet => planet.orbperiod));  // Find max orbperiod of planets
            double timestep = maxtime / numsteps;
            Plot plt = new Plot(800, 800);
            double time = 0;
            foreach (Planet planet in planets)
            {
                double[] xcoords = new double[numsteps];
                double[] ycoords = new double[numsteps];
                for(int i =0;i<numsteps;i++)
                {
                    double theta1 = 2 * Math.PI * time / centre.orbperiod;
                    double r1 = (centre.smaxis * (1 - Math.Pow(centre.ecc, 2))) / (1 - (centre.ecc * Math.Cos(theta1))); // Polar equation of an ellipse r= a(1-e^2)/(1-e cos(t))
                    double x1 = r1 * Math.Sin(theta1);
                    double y1 = r1 * Math.Cos(theta1);

                    double theta2 = 2 * Math.PI * time / planet.orbperiod;
                    double r2 = (planet.smaxis * (1 - Math.Pow(planet.ecc, 2))) / (1 - (planet.ecc * Math.Cos(theta2))); // Polar equation of an ellipse r= a(1-e^2)/(1-e cos(t))
                    double x2 = r2 * Math.Sin(theta2);
                    double y2 = r2 * Math.Cos(theta2);

                    xcoords[i] = x2 - x1;
                    ycoords[i] = y2 - y1;

                    
                    
                    time += timestep;
                }
                plt.AddScatter(xcoords, ycoords, color: planet.colour);
            }


            // Add sun
            double[] xsuncoords = new double[numsteps];
            double[] ysuncoords = new double[numsteps];
            for (int i = 0; i < numsteps; i++)
            {
                double theta1 = 2 * Math.PI * time / centre.orbperiod;
                double r1 = (centre.smaxis * (1 - Math.Pow(centre.ecc, 2))) / (1 - (centre.ecc * Math.Cos(theta1))); // Polar equation of an ellipse r= a(1-e^2)/(1-e cos(t))
                double x1 = r1 * Math.Sin(theta1);
                double y1 = r1 * Math.Cos(theta1);

                

                xsuncoords[i] = 0 - x1;
                ysuncoords[i] = 0 - y1;



                time += timestep;
            }
            plt.AddScatter(xsuncoords, ysuncoords, color: System.Drawing.Color.Yellow);

            plt.AddMarker(0, 0, color: centre.colour);
            string file = centre.name;
            foreach (Planet planet in planets) file += planet.name;
            
            plt.Title("Centre: " + centre.name);
            plt.SaveFig(file + ".png");
        }
    }
}
