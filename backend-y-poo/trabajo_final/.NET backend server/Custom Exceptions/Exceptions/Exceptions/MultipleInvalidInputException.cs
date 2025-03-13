using Custom_Exceptions.Exceptions.BaseException;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_Exceptions.Exceptions.Exceptions
{
    public class MultipleInvalidInputException : InvalidInputException
    {
        public Campo_Mensaje_Error[] errores { get; set; }
        public MultipleInvalidInputException(Campo_Mensaje_Error[] errores)
        {
            ExceptionStatusCode = StatusCodes.Status422UnprocessableEntity;
            this.errores = errores;
        }
    }



    public class Campo_Mensaje_Error
    {
        public string Campo { get; set; }
        public string Error { get; set; }
    }
}
