namespace Inventory.Model
{
    using System.Collections.Generic;

    using DevExpress.Mvvm;

    public partial class Posts_employees:BindableBase
    {
        public Posts_employees()
        {
            using var db = new InventoryEntities();
            CollectionPosts = new List<Post>(db.Posts);
        }

        public List<Post> CollectionPosts { get; set; }

        public int Id_post_employee { get; set; }
        public int Fk_employee { get; set; }
        public int Fk_post { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Post Post { get; set; }
    }
}
