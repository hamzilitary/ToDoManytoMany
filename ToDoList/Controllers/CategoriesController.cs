using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      List<Category> allCats = Category.GetAll();
      return View("Index", allCats);
    }


    [HttpGet("/categories/new")]
    public ActionResult CreateCatForm()
    {
      return View();
    }

    [HttpPost("/categories")]
    public ActionResult CreateCategory()
    {
      Category newCategory = new Category(Request.Form["name"]);
      newCategory.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/categories/{id}")]
    public ActionResult Detail(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Item> allItems = selectedCategory.GetItems();
      model.Add("category", selectedCategory);
      model.Add("items", allItems);
      return View(model);
    }

    [HttpPost("/categories/{id}/items")]
    public ActionResult CreateItem(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category foundCategory = Category.Find(id);
        List<Item> categoryItems = foundCategory.GetItems();
        Item newItem = new Item(Request.Form["new-item"], Request.Form["new-date"]);
        categoryItems.Add(newItem);
        newItem.Save();
        foundCategory.AddItem(newItem);
        model.Add("items", categoryItems);
        model.Add("category", foundCategory);
        return View("Detail", model);
    }

    [HttpPost("/categories/{id}/delete")]
    public ActionResult DeleteCategory(int id)
    {
      Category thisCategory = Category.Find(id);
      thisCategory.DeleteCategoryItems();
      thisCategory.Delete();
      List<Category> allCats = Category.GetAll();
      return View("Index", allCats);
    }
  }
}
