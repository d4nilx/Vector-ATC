using System;
using System.Collections.Generic;

namespace VectorATC.Core
{
    public enum FlightCategory 
    {
        Overflight, 
        Arrival,    
        Departure   
    }
    
    public class Aircraft
    {
        public string Callsign { get; set; }
        
        public float X { get; set; }
        public float Y { get; set; }
        public float Heading { get; set; }
        public float Speed { get; set; }
        public int Altitude { get; set; }

        public float TargetHeading { get; set; }
        public float TargetSpeed { get; set; }
        public int TargetAltitude { get; set; }

        public FlightPlan FlightPlan { get; set; }
        public List<(float x, float y)> TrailPositions { get; private set; } = new List<(float, float)>();
        private int _trailLength = 5;

        public Aircraft(string callsign, AircraftType type, float x, float y, float heading, float speed, int altitude)
        {
            Callsign = callsign;
            X = x;
            Y = y;
            Heading = heading;
            Speed = speed;
            Altitude = altitude;

            TargetHeading = heading;
            TargetSpeed = speed;
            TargetAltitude = altitude;
        }

        public void Move(float delta)
        {
            float speedRate = 10f * delta; 
            if (Speed < TargetSpeed) Speed = Math.Min(Speed + speedRate, TargetSpeed);
            if (Speed > TargetSpeed) Speed = Math.Max(Speed - speedRate, TargetSpeed);

            float climbRate = 20f * delta;
            if (Altitude < TargetAltitude) Altitude = (int)Math.Min(Altitude + climbRate, TargetAltitude);
            if (Altitude > TargetAltitude) Altitude = (int)Math.Max(Altitude - climbRate, TargetAltitude);

            if (Math.Abs(Heading - TargetHeading) > 0.1f)
            {
                float turnRate = 3f * delta;
                
                float diff = TargetHeading - Heading;
                diff = (diff + 540) % 360 - 180;

                if (diff > 0)
                    Heading += Math.Min(turnRate, diff);
                else
                    Heading += Math.Max(-turnRate, diff);

                Heading = (Heading + 360) % 360;
            }

            double radians = Heading * (Math.PI / 180.0);
            float visualSpeed = Speed / 20f;
            X += (float)(visualSpeed * Math.Sin(radians) * delta);
            Y -= (float)(visualSpeed * Math.Cos(radians) * delta);
        }

        public void RecordTrailPosition()
        {
            TrailPositions.Add((X, Y));
            if (TrailPositions.Count > _trailLength)
            {
                TrailPositions.RemoveAt(0);
            }
        }
        
        public FlightCategory Category { get; set; } = FlightCategory.Overflight;
    }
}