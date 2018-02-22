using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {

    [HttpGet("/categories/{categoryID}/items/new")]
    public ActionResult CreateItemForm(int categoryId)
    {
      Category foundCategory = Category.Find(categoryId);
      return View(foundCategory);
    }

//not currently used
    [HttpGet("/items/{id}")]
    public ActionResult Detail(int id)
    {
      Item item = Item.Find(id);
      return View(item);
    }

    [HttpGet("/items/{id}/update")]
    public ActionResult UpdateForm(int id)
    {
      Item thisItem = Item.Find(id);
      return View(thisItem);
    }

    [HttpPost("/items/{id}/update")]
    public ActionResult UpdateItem(int id)
    {
      Item thisItem = Item.Find(id);
      thisItem.Edit(Request.Form["newname"]);
      return RedirectToAction("Detail", "categories", new {Id = thisItem.GetCategoryId()});
    }

    [HttpGet("/items/{id}/delete")]
    public ActionResult DeleteItem(int id)
    {
      Item thisItem = Item.Find(id);
      //int catId = thisItem.GetCategoryId();
      thisItem.Delete();
      return RedirectToAction("Detail", "categories", new {Id = thisItem.GetCategoryId()});
    }
  }
}
