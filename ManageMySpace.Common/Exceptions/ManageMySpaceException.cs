using System;

namespace ManageMySpace.Common.Exceptions
{
    public class ManageMySpaceException : Exception
    {
        public string Code { get; set; }

        public ManageMySpaceException()
        {
                
        }

        public ManageMySpaceException(string code) : this(code, string.Empty) { }

        public ManageMySpaceException(string code, string message) : this (null, code, message) { }

        public ManageMySpaceException(Exception innerException, string code, string message) : base(message, innerException)
        {
            Code = code;
        }
    }
}
