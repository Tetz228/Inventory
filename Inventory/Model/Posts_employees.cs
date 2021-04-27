namespace Inventory.Model
{
    using System.Linq;

    using DevExpress.Mvvm;

    public partial class Posts_employees:BindableBase
    {
        private int _fkPost;

        public int Id_post_employee { get; set; }
        public int Fk_employee { get; set; }

        public int Fk_post
        {
            get => _fkPost;
            set
            {
                _fkPost = value;
                RaisePropertyChanged(nameof(Fk_post));

                if (Fk_employee != 0)
                {
                    using var db = new InventoryEntities();

                    Post = db.Posts.FirstOrDefault(post => post.Id_post == Fk_post);
                }
            }
        }

        public virtual Employee Employee { get; set; }
        public virtual Post Post { get; set; }
    }
}
