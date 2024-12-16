using Microsoft.AspNetCore.Mvc;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Utils.ExceptionHandler
{
    public class ExceptionHandler
    {
        private ControllerBase _controller { get; set; }

        public ExceptionHandler(ControllerBase controller) { 
            _controller = controller;
        }


        public ActionResult InputValidationExceptionHandler(InputValidationException inputEx)
        {
            //Estoy en duda si el codigo seria 400 o 422
            Console.WriteLine(inputEx.Message);
            return _controller.StatusCode(
                StatusCodes.Status400BadRequest,
                inputEx.Message
            );
        }
        public ActionResult NotFoundExceptionHandler(NotFoundException notFoundEx)
        {
            //Estoy en duda si el codigo seria 200, 204 o 404
            Console.WriteLine(notFoundEx.Message);
            return _controller.StatusCode(
                StatusCodes.Status404NotFound,
                new { message = notFoundEx.Message }
            );
        }
        public ActionResult AlreadyExistsExceptionHandler(AlreadyExistsException alreadyExistsEx)
        {
            //Aca tampoco se si el codigo es 400, 409, 422
            Console.WriteLine(alreadyExistsEx.Message);
            return _controller.StatusCode(
                StatusCodes.Status422UnprocessableEntity,
                new { message = alreadyExistsEx.Message }
            );
        }
        public ActionResult DefaultExceptionHandler(Exception ex)
        {
            //si no es error de input, seguro es error del servidor
            Console.WriteLine(ex.Message);
            return _controller.StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = ex.Message }
            );
        }
        public ActionResult InvalidCredentialsExceptionHandler (InvalidCredentialsException invalidCredsEx)
        {
            //creeria que es un error 401, ya que se intenta acceder a la info del usuario
            Console.WriteLine(invalidCredsEx.Message);
            return _controller.StatusCode(
                StatusCodes.Status401Unauthorized,
                new { message = invalidCredsEx.Message }
            );
        }

        public ActionResult NotAvaiableExceptionHandler(NotAvaiableException notAvaiableEx)
        {
            //Se trata de un recurso que no se puede acceder porque no está disponible.
            //El servidor entiende la request del cliente (no hay errores de formato o de otro tipo)
            //Sin embargo, hay un conflicto con el recurso, en este caso, no está disponible
                //--> Por lo tanto, el status code debería ser 409 - Conflict
            Console.WriteLine(notAvaiableEx.Message);
            return _controller.StatusCode(
                StatusCodes.Status409Conflict,
                new { message = notAvaiableEx.Message }
            );
        }

        public ActionResult SinPermisoExceptionHandler(SinPermisoException sinPermisoEx)
        {
            //El usuario intenta prestar un libro a otro usuario, pero eso no se puede.
            //Solo esta permitido prestar al mismo usuario que esta logueado.
            //Osea, el usuario logeado solo tiene permiso para prestarse a si mismo.
            //No tiene permiso para prestar a los demas usuarios.
            Console.WriteLine(sinPermisoEx.Message);
            return _controller.StatusCode(
                StatusCodes.Status403Forbidden,
                new { message = sinPermisoEx.Message }
            );
        }

    }
}
