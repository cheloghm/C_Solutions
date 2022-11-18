FROM mongo
RUN mkdir -p /app
WORKDIR /app
COPY titanic.csv /app
COPY script.sh /app
CMD ["mongo", "&&", "mongoimport", "-d", "titanic", "-c", "passengersCollection", "--file", "titanic.csv", "--headerline"]