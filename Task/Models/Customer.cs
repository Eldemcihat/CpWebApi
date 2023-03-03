using System;
using System.Collections.Generic;

namespace Task;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Token { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string Msisdn { get; set; } = null!;
}
