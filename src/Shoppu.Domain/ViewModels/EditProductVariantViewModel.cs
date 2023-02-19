namespace Shoppu.Domain.ViewModels
{
    public class EditProductVariantViewModel : CreateProductVariantViewModel
    {
        public int ProductVariantId { get; set; }
        public bool IsAccessible { get; set; }
        public string ProductCode { get; set; }
        public List<string>? ImagePaths { get; set; }
    }
}