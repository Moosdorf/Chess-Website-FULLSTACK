﻿namespace DataLayer.Models.Chess
{
    public class MoveModel
    {
        public string Move { get; set; } = null!;
        public char? Promotion { get; set; } = null;
    }
}
