using App.Domain.Shared;

namespace App.Domain
{
    public class User : Entity<int>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Credit { get; set; }
    }
}