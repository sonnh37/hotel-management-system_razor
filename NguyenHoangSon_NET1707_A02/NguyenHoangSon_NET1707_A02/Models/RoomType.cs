using System;
using System.Collections.Generic;

namespace NguyenHoangSon_NET1707_A02.Models;

public partial class RoomType
{
    public int RoomTypeId { get; set; }

    public string RoomTypeName { get; set; } = null!;

    public string? TypeDescription { get; set; }

    public string? TypeNote { get; set; }

    public virtual ICollection<RoomInformation> RoomInformations { get; set; } = new List<RoomInformation>();
}
