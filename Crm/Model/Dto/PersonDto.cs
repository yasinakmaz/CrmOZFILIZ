namespace Crm.Model.Dto
{
    public record PersonDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; } = string.Empty;
    }
}
