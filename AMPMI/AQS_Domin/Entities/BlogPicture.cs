﻿namespace Domin.Entities;
public partial class BlogPicture
{
    public Guid Id { get; set; }

    public int? BlogId { get; set; }

    public virtual Blog? Blog { get; set; }
}