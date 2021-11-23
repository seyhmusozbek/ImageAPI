using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAPI.Entities
{
    public class ImageDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string fileName { get; set; }
        public DateTimeOffset saveDate { get; set; }
        public string fileType { get; set; }
        public string filter { get; set; }
        public string colorType { get; set; }
        public string imageSize { get; set; }
        public int bitDepth { get; set; }
        public double gamma { get; set; }


    }
}
