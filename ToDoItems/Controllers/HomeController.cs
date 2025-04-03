using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoItems.Models;

namespace ToDoItems.Controllers
{
    public class HomeController : Controller
    {
        private static string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ToDoItem;Integrated Security=true;TrustServerCertificate=yes;";
        ToDoItemManager mgr = new ToDoItemManager(_connectionString);

        public IActionResult Index()
        {
            return View(new ItemViewModel
            {
                Items = mgr.GetUncompletedItems()
            });
        }

        [HttpPost]
        public IActionResult MarkAsCompleted(int id)
        {
            mgr.MarkAsCompleted(id);
            return Redirect("/home/index");
        }

        public IActionResult Completed()
        {
            return View(new ItemViewModel
            {
                Items = mgr.GetCompletedItems()
            });
        }

        public IActionResult Categories()
        {
            return View(new CategoryViewModel
            {
                Categories = mgr.GetCategories()
            });
        }

        public IActionResult ItemsForCategory(int Id)
        {
            return View(new ItemViewModel
            {
                Items = mgr.GetItemsForCategory(Id)
            });
        }

        public IActionResult NewCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveNewCategory(Category c)
        {
            mgr.AddCategory(c);
            return Redirect("/home/categories");
        }

        public IActionResult EditCategory(int id)
        {
            return View(new EditViewModel
            {
                Category = mgr.GetCategoryById(id)
            });
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            mgr.UpdateCategory(category);
            return Redirect("/home/categories");
        }

        public IActionResult NewItem()
        {
            return View(new CategoryViewModel
            {
                Categories = mgr.GetCategories()
            });
        }

        [HttpPost]
        public IActionResult SaveNewItem(ToDoItem item)
        {
            mgr.AddItem(item);
            return Redirect("/home/index");
        }

    }
}
