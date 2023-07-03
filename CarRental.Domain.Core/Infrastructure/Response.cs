using CarRental.Domain.Core.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Core.Infrastructure
{
    public class Response
    {
        public static KeyValuePair<HttpResponseMessage, T> Failure<T>(HttpStatusCode code, string error)
        {
            var http = new HttpResponseMessage(code);
            http.Content = new StringContent(error);

            return new KeyValuePair<HttpResponseMessage, T>(http, default(T));
        }

        public static KeyValuePair<HttpResponseMessage, T> Sucess<T>(T entity)
        {
            var http = new HttpResponseMessage(HttpStatusCode.OK);

            return new KeyValuePair<HttpResponseMessage, T>(http, entity);
        }
    }
}
