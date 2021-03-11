namespace Inventory.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public partial class Posts_employees
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

        public static void AddPostEmployee(InventoryEntities db, int idEmployee)
        {
            foreach (var post in Employee.PostsEmployees)
                post.Fk_employee = idEmployee;

            db.Posts_employees.AddRange(Employee.PostsEmployees);
            db.SaveChanges();
        }

        public static void EditPostEmployee(InventoryEntities db, int idEmployee)
        {
            foreach (var post in Employee.PostsEmployees)
            {
                if (post.Id_post_employee == 0)
                {
                    post.Fk_employee = idEmployee;
                    db.Posts_employees.Add(post);
                    db.SaveChanges();
                }
                else
                {
                    var postEmployee = db.Posts_employees.Where(postEmp => postEmp.Id_post_employee == post.Id_post_employee).ToList();
                    foreach (var item in postEmployee)
                    {
                        item.Fk_post = post.Fk_post;
                        db.SaveChanges();
                    }
                }
            }
        }

        public static void DeletePostEmployee(Posts_employees selectPostEmp)
        {
            if (selectPostEmp.Id_post_employee != 0)
            {
                using var db = new InventoryEntities();
                var findPostEmployee = db.Posts_employees.SingleOrDefault(postEmployee =>
                    postEmployee.Id_post_employee == selectPostEmp.Id_post_employee);

                if (findPostEmployee != null)
                {
                    db.Posts_employees.Remove(findPostEmployee);
                    db.SaveChanges();
                }
            }

            Employee.PostsEmployees.Remove(selectPostEmp);
        }
    }
}
