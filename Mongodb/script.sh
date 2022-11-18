#! /bin/sh 
mongoimport -d titanic -c passengersCollection --type csv --file titanic.csv --headerline