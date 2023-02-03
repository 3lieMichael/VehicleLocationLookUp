# Vehicle Position Challenge

Hello out there whereever you are evaluating this code from. My name is Elie Michael Ngandu and I would like to start by saying thank you for giving me the opportunity to complete this challange.
I hope that my solution impressed you enough that we get the chance to meet in the near future. :)

# Application details

The application is a .Net 6 console application. 
My solution to the challenge is divided in 3 parts as followed: 
  * File reading
  * Organize the vehicles data into clusters
  * Find closest vehicles

# File reading

This is done in the DataReader.cs static class. It has the public method named ReadDataFromFile that works has followed:
  * Read data from the binary file VehiclePositions.dat and returns the data as a collection of Record objects stored in a ConcurrentQueue
  * Returns null is the file does not exists
  * It uses memory-mapped files and multithreading to efficiently read the data
  * The method uses a memory-mapped file to access the contents of the data file in memory. It then creates a view accessor for the entire contents of the file and uses an unsafe pointer to read the data into a byte array
  * The data is then split into multiple chunks based on the number of processors and runs a separate task for each chunk to read the data
  * It also uses ReadStringUntilNull helper method to read a null-terminated string from a binary reader

# Organize the vehicles data into clusters

This is done by the DataOrganizer.cs class and has single method called DataToClusters. It simply takestakes in a list of Record objects and returns a dictionary where each key is a string representation of a latitude and longitude value and each value is a list of Record objects that correspond to that key.

# Find closest vehicles

The LocationLookUp.cs class is used to find the closest vehicle to a set of positions. The FindClosestLocation method takes in a dictionary of vehicle groups where the keys are string representations of coordinates and the values are lists of records for vehicles at those coordinates. It then itterates through a list of positions and calculates the closest vehicle to each position.

The CalculateDistance method is used to calculate the distance between two positions using the Haversine formula. The method takes in two positions, converts their latitudes and longitudes to radians, and calculates the distance in kilometers using the formula.