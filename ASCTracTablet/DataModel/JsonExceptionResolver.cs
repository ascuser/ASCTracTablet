using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ASCTracTablet.DataModel
{
    class JsonExceptionResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member,
                                            MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(ASCTracFunctionStruct.NoteType) &&
                property.PropertyName == "noteTypeDesc")
            {
                property.ShouldSerialize = instanceOfProblematic => false;
            }

            return property;
        }
    }
}