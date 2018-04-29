using System.Collections.Generic;

namespace JokerKS.WLFU.Entities.Menu
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool IsSelected { get; set; }
        public int Level { get; set; }
        public int? ParentId { get; set; }
        public bool HasChildren { get; set; }
        public int Position { get; set; }
        public IList<MenuItem> Children { get; set; }
    }
}