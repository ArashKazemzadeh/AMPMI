namespace AQS_Aplication.Dtos
{
    public record CreateProductDto(string Name, string Brand, string Description, long Price, string ImageAdress);
    public record ReadProductDto(long Id, string Name, string Brand, string Description, long Price, string ImageAdress , bool Displayed);
    public record UpdateProductDto(long Id, string Name, string Brand, string Description, long Price);
    public record DeleteProductDto(long Id);
}
