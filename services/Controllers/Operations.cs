using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace BYServices.Controllers
{
    public class Operations : ApiController
    {

        public Operations()
        {
        }

        public void ValidateSessionId(string sessionId)
        {
            BYEntities db = new BYEntities();
            if (string.IsNullOrEmpty(sessionId))
            {
                throw BuildHttpResponseException("Invalid SessionId", "ERR_INV_AUTH");
            }
            User us = db.Users.SingleOrDefault(x => x.SessionId == sessionId);
            if (us == null)
            {
                throw BuildHttpResponseException("Invalid SessionId(no user)", "ERR_INV_AUTH");
            }
        }

        public HttpResponseMessage PerformOperation(Action action)
        {
            try
            {
                action();
                var msg = Request.CreateResponse(HttpStatusCode.OK);
                return msg;
            }
            catch (Exception ex)
            {
                if (ex is HttpResponseException)
                {
                    throw ex;
                }
                throw this.BuildHttpResponseException(ex.Message);
            }
        }

        public HttpResponseMessage PerformOperation<T>(Func<T> action)
        {
            try
            {
                var result = action();
                var msg = Request.CreateResponse(HttpStatusCode.OK, result);
                return msg;
            }
            catch (Exception ex)
            {
                if (ex is HttpResponseException)
                {
                    throw ex;
                }
                throw this.BuildHttpResponseException(ex.Message);
            }
        }

        public HttpResponseException BuildHttpResponseException(string errMsg, string errCode)
        {
            var error = new HttpError(errMsg);
            error["ERR_CODE"] = errCode;
            var resp = Request.CreateResponse(HttpStatusCode.NotFound, error);
            return new HttpResponseException(resp);
        }

        public HttpResponseException BuildHttpResponseException(string msg)
        {
            return this.BuildHttpResponseException("The server returned an unexpected error " + msg, "ERR_GENERAL");
        }
    }
}