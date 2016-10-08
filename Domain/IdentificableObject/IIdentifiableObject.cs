namespace Domain.IdentificableObject
{
    public interface IIdentifiableObject
    {
        int Id { get; set; }
        int? Secuencia { get; set; }
    }
}