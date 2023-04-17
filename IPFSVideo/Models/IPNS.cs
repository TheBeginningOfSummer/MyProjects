using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPFSVideo.Models
{
    public class IPNS
    {
        public string? Name { get; set; }
        public string? Id { get; set; }

        public IPNS(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public IPNS()
        {
            //System.Text.Json.JsonSerializer.DeserializeAsync<>

        }

    }
}
