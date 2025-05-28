namespace L2CodePackagingAPI.DTOs
{
    public class BoxPackagingDto
    {
        public string Id { get; set; } = string.Empty;
        public List<string> Produtos { get; set; } = new List<string>();
        public DimensionsDto Dimensoes { get; set; } = new DimensionsDto();
        public int VolumeUtilizado { get; set; }
        public int VolumeTotal { get; set; }
        public double TaxaOcupacao { get; set; }
    }
}
