namespace Domin.Entities;
public partial class SiteAdmin
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
