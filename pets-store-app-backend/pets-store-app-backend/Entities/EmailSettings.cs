using Core.Entities;

namespace Entities
{
    public class EmailSettings:IEntity
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsSsl { get; set; }
        public string Name { get; set; }
    }
}
