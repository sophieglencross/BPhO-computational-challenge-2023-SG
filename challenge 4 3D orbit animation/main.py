import math
import numpy
import matplotlib.pyplot as plt


class Planet:
    def __init__(self, name, mass, smaxis, radius, rotperiod, orbperiod, ecc, betadegrees, colour):
        self.name = name
        self.mass = mass
        self.smaxis = smaxis
        self.radius = radius
        self.rotperiod = rotperiod
        self.orbperiod = orbperiod
        self.ecc = ecc
        self.orbinclin = math.pi * 2 * betadegrees / 180
        self.colour = colour


mercury = Planet("Mercury", 0.055, 0.387, 0.38, 58.65, 0.24, 0.21, 7.00, "purple")
venus = Planet("Venus", 0.815, 0.723, 0.95, 243.02, 0.62, 0.01, 3.39, "pink")
earth = Planet("Earth", 1.000, 1.000, 1.00, 1.00, 1.00, 0.02, 0.00, "green")
mars = Planet("Mars", 0.107, 1.523, 0.53, 1.03, 1.88, 0.09, 1.85, "red")
jupiter = Planet("Jupiter", 317.85, 5.20, 11.21, 0.41, 11.86, 0.05, 1.31, "brown")
saturn = Planet("Saturn", 95.16, 9.58, 9.45, 0.44, 29.63, 0.06, 2.49, "orange")
uranus = Planet("Uranus", 14.50, 19.29, 4.01, 0.72, 84.75, 0.05, 0.77, "teal")
neptune = Planet("Neptune", 17.20, 30.25, 3.88, 0.67, 166.34, 0.01, 1.77, "blue")
pluto = Planet("Pluto", 0.003, 39.509, 0.187, 6.387, 248.348, 0.25, 17.5,
               "violet")  # Not a planet, but has same properties

# planets = [jupiter, saturn, uranus, neptune, pluto]
planets = [mercury, venus, earth, mars]

numofimages = 100
dotsperorbit = 100
maxperiod = planets[-1].orbperiod
for imagenumber in range(numofimages):
    simtime = maxperiod * imagenumber / numofimages
    # plt = matplotlib
    fig, ax = plt.subplots(subplot_kw={"projection": "3d"})
    ax.plot(0, 0, 0, "o", color="yellow")
    ax.set_title("Inner planets")

    for i, body in enumerate(planets):
        xpoints = list()
        ypoints = list()
        zpoints = list()
        for step in range(dotsperorbit):
            theta = 2 * math.pi * step / dotsperorbit
            r = (body.smaxis * (1 - math.pow(body.ecc, 2))) / (1 - (body.ecc * math.cos(theta)))
            x = (r * math.cos(theta))
            y = (r * math.sin(theta))
            z = x * math.sin(body.orbinclin)
            x *= math.cos(body.orbinclin)
            xpoints.append(x)
            ypoints.append(y)
            zpoints.append(z)
        ax.scatter(xpoints, ypoints, zpoints, s=1, color=body.colour)

        angle = 2 * math.pi * simtime / body.orbperiod
        distance = (body.smaxis * (1 - math.pow(body.ecc, 2))) / (1 - (body.ecc * math.cos(angle)))
        x = distance * math.cos(angle)
        y = distance * math.sin(angle)
        z = x * math.sin(body.orbinclin)
        x *= math.cos(body.orbinclin)
        ax.plot(x, y, z, "o", color=body.colour)

        if planets[0].name == "Mercury":
            ax.set_xlim(-1.5, 1.5)
            ax.set_ylim(-1.5, 1.5)
            ax.set_zlim(-1.5, 1.5)
            ax.text(-1.5, -1.5, 1.5, str(round(simtime, 3)) + " years")
        else:
            ax.set_xlim(-40, 40)
            ax.set_ylim(-40, 40)
            ax.set_zlim(-40, 40)
            ax.text(-40, -40, 40, str(round(simtime, 3)) + " years")
        ax.set_xlabel('X axis')
        ax.set_ylabel('Y axis')
        ax.set_zlabel('Z axis')

    fig.savefig("{:02d}".format(imagenumber) + "inner.png")
    plt.close()

