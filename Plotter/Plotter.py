import sys
import matplotlib.pyplot as plt

if len(sys.argv) <= 1:
    path = input("Enter file path: ")
else:
    path = sys.argv[1]

if path != "":
    f = open(path, "r")
    data = f.read()
    f.close()
    
    data = [float(x) for x in data.replace(",", ".").split()]
    
    plt.plot(data)
    plt.show()
