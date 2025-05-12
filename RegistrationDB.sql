USE master;
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'RegistrationDB')
DROP DATABASE RegistrationDB
CREATE DATABASE RegistrationDB

USE RegistrationDB

-- Drop tables if they exist
DROP TABLE IF EXISTS Booking;
DROP TABLE IF EXISTS Event;
DROP TABLE IF EXISTS Venue;

-- Create the Venue table
CREATE TABLE Venue (
    VenueId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    VenueName VARCHAR(50) NOT NULL,
    Location VARCHAR(50) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl VARCHAR(MAX)
);

-- Create the Event table
CREATE TABLE Event (
    EventId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    EventName VARCHAR(50) NOT NULL,
    EventDate DATE NOT NULL,
    Description VARCHAR(MAX) NOT NULL,
    VenueId INT NOT NULL,  -- Added a comma here
    FOREIGN KEY (VenueId) REFERENCES Venue (VenueId) -- Foreign key for Venue
);

-- Create the Booking table
CREATE TABLE Booking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    EventId INT NOT NULL,
    VenueId INT NOT NULL,
    BookingDate DATE NOT NULL,  -- This is the correct column name
    FOREIGN KEY (VenueId) REFERENCES Venue (VenueId), -- Foreign key for Venue
    FOREIGN KEY (EventId) REFERENCES Event (EventId) -- Foreign key for Event
);

-- Insert sample data into the Venue table
INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES ('Venue 1', 'Location 1', 100, 'image1.jpg');
       

-- Insert sample data into the Event table
INSERT INTO Event (EventName, EventDate, Description, VenueId)
VALUES ('Event 1', '2025-03-10', 'Description for Event 1', 1);

-- Insert sample data into the Booking table
INSERT INTO Booking (EventId, VenueId, BookingDate)
VALUES (1, 1, '2025-03-10');

--TABLE MANIPULATION
SELECT * FROM Venue
SELECT * FROM Event
SELECT * FROM Booking