using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Shared
{
    public abstract class Entity<TKey> : IEntity
    {
        public Entity()
        {
            this.CreatedTime = DateTimeOffset.Now;
        }
        public TKey Id { get; set; }
        public  DateTimeOffset CreatedTime { get; set; }
    }
}
