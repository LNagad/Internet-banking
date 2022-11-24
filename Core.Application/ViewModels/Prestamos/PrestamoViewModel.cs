using Core.Application.ViewModels.Products;


namespace Core.Application.ViewModels.Prestamos
{
    public class PrestamoViewModel
    {
        public string Id { get; set; }

        public string NumeroPrestamo { get; set; }

        public double Monto { get; set; }

        public double Pago { get; set; }

        public double Debe { get; set; } //1 , 0

        public double Balance { get; set; }

        //navigation property
        public string IdProduct { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
