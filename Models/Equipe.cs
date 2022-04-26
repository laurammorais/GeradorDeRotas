﻿using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
	public class Equipe
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string NomeEquipe { get; set; }
        public List<Pessoa> Pessoas { get; set; } = new List<Pessoa>();
        [BsonIgnore]
        public List<string> Cpfs { get; set; }
        public string Cidade { get; set; }
        public string Servico { get; set; }

        public void SetId(string id) => Id = id;
    }
}