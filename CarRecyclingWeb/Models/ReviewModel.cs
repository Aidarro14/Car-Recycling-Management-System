using System;

namespace CarRecyclingWeb.Models
{
    public class ReviewModel
    {
        public string Name { get; set; }
        public int Rating { get; set; } // Например, от 1 до 5
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}