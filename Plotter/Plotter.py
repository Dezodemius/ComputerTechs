import sys
import matplotlib.pyplot as plt

path = sys.argv[1]
f = open(path, "r")
data = f.read()
f.close()

data = [float(x) for x in data.replace(",", ".").split()]

plt.plot(data)
plt.show()
