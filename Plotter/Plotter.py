import sys
import matplotlib.pyplot as plt

if len(sys.argv) <= 1:
    path = input("Enter file path: ")
else:
    path = sys.argv[1]

if path != "":
    f = open(path, "r")
    lines = f.readlines()
    result = [[], []]
    for i in range(2):
        for x in lines:
            result[i].append(float(x.replace('\n', '').replace(',', '.').split()[i]))
    f.close()

    plt.scatter(result[0], result[1], color='black')
    plt.savefig("d:\pics\{}.png".format(sys.argv[2]))
    #plt.show()
