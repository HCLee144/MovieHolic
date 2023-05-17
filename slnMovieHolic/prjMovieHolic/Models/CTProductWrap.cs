namespace prjMovieHolic.Models
{
    public class CTProductWrap
    {
        private TProduct _product;
        public CTProductWrap()
        {
            _product = new TProduct();
        }
        public TProduct product
        {
            get { return _product; }
            set { _product = value; }
        }
        public int FProductId
        {
            get { return _product.FProductId; }
            set { _product.FProductId = value; }
        }
        public string FProductName
        {
            get { return _product.FProductName; }
            set { _product.FProductName = value; }
        }
        public int FProductPrice
        {
            get { return _product.FProductPrice; }
            set { _product.FProductPrice = value; }
        }
        public byte[] FImage
        {
            get { return _product.FImage; }
            set { _product.FImage = value; }
        }
        public string FImagePath
        {
            get { return _product.FImagePath; }
            set { _product.FImagePath = value; }
        }
        public IFormFile photo {get; set;}

    }
}
