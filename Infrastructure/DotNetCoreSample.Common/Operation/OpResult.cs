using System;
using System.Net;

namespace  AuthService.Common.Operation
{
    public class OpResult
    {
        protected bool _hasSuccessBeenChecked;

        private bool _succeeded;
        public string message { get; set; }
        public HttpStatusCode status { get; set; }
        public int errorCode { get; set; }

        public bool Succeeded
        {
            get
            {
                _hasSuccessBeenChecked = true;
                return _succeeded;
            }
            set { _succeeded = value; }
        }

        public Exception Exception { get; set; }

        public OpResult()
        {
        }

        public OpResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public OpResult(Exception ex)
        {
            Succeeded = false;
            Exception = ex;
        }

        public OpResult(bool succedded, Exception ex)
        {
            Succeeded = succedded;
            Exception = ex;
        }
    }

    public class OpResult<Tout> : OpResult
    {
        private Tout _result;

        public Tout Result
        {
            get
            {
                //if (!_hasSuccessBeenChecked) throw new Exception("Please check the Succeeded before accessing the result");
                return _result;
            }
            set { _result = value; }
        }

        //public Tout Result { get; set; }

        public OpResult(Tout result)
        {
            Succeeded = true;
            Result = result;
            status = HttpStatusCode.OK;
            message = "Success";
        }

        public OpResult(Exception ex)
        {
            Succeeded = false;
            Exception = ex;
            message = ex.Message;
            status = HttpStatusCode.ExpectationFailed;
        }
    }
}