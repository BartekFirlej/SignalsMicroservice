using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using BackendSignals.Requests;

namespace BackendSignals.Models
{
    public class Flight
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("FlightID")]
        public string FlightID { get; set; }

        [BsonElement("OperatorID")]
        public int OperatorID { get; set; }

        [BsonElement("TeamID")]
        public int TeamID { get; set; }

        [BsonElement("PlatoonID")]
        public int PlatoonID { get; set; }

        [BsonElement("X")]
        public int X { get; set; }

        [BsonElement("Y")]
        public int Y { get; set; }

        [BsonElement("Z")]
        public int Z { get; set; }

        [BsonElement("BeginTime")]
        public DateTime BeginTime { get; set; }

        [BsonElement("Comment")]
        public string? Comment { get; set; }

        [BsonElement("Measurements")]
        public List<Measurement> Measurements { get; set; } = new List<Measurement>();

        public Flight(FlightBeginRequest request)
        {
            FlightID = request.FlightID;
            OperatorID = request.OperatorID;
            TeamID = request.TeamID;
            PlatoonID = request.PlatoonID;
            X = request.X;
            Y = request.Y;
            Z = request.Z;
            BeginTime = request.BeginTime;
            Comment = request.Comment;
            Measurements = new List<Measurement>();
        }

        public void AddMeasurement(Measurement measurement)
        {
            this.Measurements.Add(measurement);
        }
    }
}
