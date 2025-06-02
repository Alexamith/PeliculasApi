using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Validaciones
{
    public class TipoArchivoValidacion : ValidationAttribute
    {
        private readonly string[] tipoValidos;

        public TipoArchivoValidacion(GrupoTipoArchivo grupoTipoArchivo)
        {

            if (grupoTipoArchivo == GrupoTipoArchivo.Imagen)
            {
                tipoValidos = new string[] { "image/jpeg", "image/png" , "image/gif" };
            }

        }

        public TipoArchivoValidacion(string[] tipoValidos)
        {
            this.tipoValidos = tipoValidos;
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (!tipoValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo de archivo debe ser uno de los siguientes: {string.Join(", ", tipoValidos)}");
            }
            return ValidationResult.Success;

        }
    }
}
