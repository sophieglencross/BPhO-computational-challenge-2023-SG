using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace challenge_3_orbits_animation
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

            public Planet(string name, double mass, double smaxis, double radius, double rotperiod, double orbperiod, double ecc, System.Drawing.Color colour)
            {
                this.name = name;
                this.mass = mass;
                this.smaxis = smaxis;
                this.radius = radius;
                this.rotperiod = rotperiod;
                this.orbperiod = orbperiod;
                this.ecc = ecc;
                this.colour = colour;

            }
        }
        static void Main(string[] args)
        {
            Planet Mercury = new Planet("Mercury", 0.055, 0.387, 0.38, 58.65, 0.24, 0.21, System.Drawing.Color.Purple);
            Planet Venus = new Planet("Venus", 0.815, 0.723, 0.95, 243.02, 0.62, 0.01, System.Drawing.Color.Pink);
            Planet Earth = new Planet("Earth", 1.000, 1.000, 1.00, 1.00, 1.00, 0.02, System.Drawing.Color.Green);
            Planet Mars = new Planet("Mars", 0.107, 1.523, 0.53, 1.03, 1.88, 0.09, System.Drawing.Color.Red);
            Planet Jupiter = new Planet("Jupiter", 317.85, 5.20, 11.21, 0.41, 11.86, 0.05, System.Drawing.Color.Yellow);
            Planet Saturn = new Planet("Saturn", 95.16, 9.58, 9.45, 0.44, 29.63, 0.06, System.Drawing.Color.Orange);
            Planet Uranus = new Planet("Uranus", 14.50, 19.29, 4.01, 0.72, 84.75, 0.05, System.Drawing.Color.Teal);
            Planet Neptune = new Planet("Neptune", 17.20, 30.25, 3.88, 0.67, 166.34, 0.01, System.Drawing.Color.Blue);


            //Planet[] planets = { Mercury, Venus, Earth, Mars , Jupiter, Saturn, Uranus, Neptune };
            Planet[] planets = { Jupiter, Saturn, Uranus, Neptune };
            int maximages = 100;
            double maxperiod = planets[planets.Length - 1].orbperiod;
            for (int imagenumber = 0; imagenumber < maximages; imagenumber++)
            {
                double simtime = maxperiod * imagenumber / maximages;
                Plot plt = new Plot(600, 600);
                foreach (Planet body in planets)
                {
                    double[] xpoints = new double[1000];
                    double[] ypoints = new double[1000];
                    for (int step = 0; step < 1000; step++)
                    {
                        double theta = (2 * Math.PI * step) / 1000;// Angle have gone round of full circle
                        double r = (body.smaxis * (1 - Math.Pow(body.ecc, 2))) / (1 - (body.ecc * Math.Cos(theta))); // Polar equation of an ellipse r= a(1-e^2)/(1-e cos(t))
                        xpoints[step] = r * Math.Cos(theta);
                        ypoints[step] = r * Math.Sin(theta);
                    }
                    plt.AddScatter(xpoints, ypoints, markerSize: 2, color:body.colour);


                    //Calculate position of marker that moves
                    double angle = (2 * Math.PI * simtime) / body.orbperiod;
                    double distance = (body.smaxis * (1 - Math.Pow(body.ecc, 2))) / (1 - (body.ecc * Math.Cos(angle)));
                    double x = distance * Math.Cos(angle);
                    double y = distance * Math.Sin(angle);

                    plt.AddMarker(x,y, color:body.colour);
                    
                }
                plt.XLabel("AU");
                plt.YLabel("AU");
                plt.Title("Outer Planets' Orbits");
                plt.AddAnnotation("Time of simulation: " + simtime + " years");
                plt.AddMarker(0, 0, MarkerShape.filledCircle, 10, System.Drawing.Color.Yellow);  // Add circle for the sun
                plt.SaveFig("all"+imagenumber.ToString("D3") + ".png");
                
            }

            // Command to run in command line to create video from images 
            //..\ffmpeg-6.0-essentials_build\bin\ffmpeg.exe  -framerate 30  -i .\bin\Debug\*.png -c:v libx264 -pix_fmt yuv420p out.mp4

        }
    }
}


