namespace CliniPetApi_p.Models
{
    public class MascotaModel
    {
        public int IDMascota { get; set; }
        public string NombreMascota { get; set; }
        public string Especie { get; set; }
        public decimal Peso { get; set; }
        public string Edad { get; set; }
        public string CedulaCliente { get; set; }  // Referencia al cliente propietario
        public int RazaID { get; set; }
        public string Genero { get; set; }

        // Propiedad para la foto, que será un archivo cargado
        public IFormFile Foto { get; set; }  // Aquí se manejará el archivo de imagen
    }
}
