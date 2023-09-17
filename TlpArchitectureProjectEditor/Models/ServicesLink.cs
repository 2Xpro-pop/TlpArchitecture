using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace TlpArchitectureProjectEditor.Models;
public class ServicesLink
{
    [BsonId]
    public Guid Id
    {
        get; set;
    }

    public Guid FirstServiceStartInfoId
    {
        get; set;
    }

    public Guid SecondServiceStartInfoId
    {
        get; set;
    }
}
