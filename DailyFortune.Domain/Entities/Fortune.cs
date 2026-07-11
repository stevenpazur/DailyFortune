using System;
using System.Collections.Generic;
using System.Text;

namespace DailyFortune.Domain.Entities;

public class Fortune
{
    public int Id { get; set; }

    public string Category { get; set; } = "";

    public string Text { get; set; } = "";
}