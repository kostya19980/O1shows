using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotMyShows.ViewModel.UserProfileService;
using O1shows.ViewModels;
using System;

namespace O1shows.Services
{
    public abstract class AbstractJsonConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            T target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected static bool FieldExists(JObject jObject, string name, JTokenType type)
        {
            JToken token;
            return jObject.TryGetValue(name, out token) && token.Type == type;
        }
    }

    public class UserEventConverter : AbstractJsonConverter<UserEvent>
    {
        protected override UserEvent Create(Type objectType, JObject jObject)
        {
            if (FieldExists(jObject, "watchStatus", JTokenType.String))
            {
                return new SeriesEvent();
            }
            else if (FieldExists(jObject, "raiting", JTokenType.Integer))
            {
                return new RaitingEvent();
            }
            else if (FieldExists(jObject, "userName", JTokenType.String))
            {
                return new FriendEvent();
            }
            else
            {
                return new EpisodeEvent();
            }

            throw new InvalidOperationException();
        }
    }
}
