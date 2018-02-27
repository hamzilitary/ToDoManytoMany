using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class ItemTests : IDisposable
  {
    public void Dispose()
    {
      Item.DeleteAll();
      Category.DeleteAll();
    }

    public ItemTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
    }

    [TestMethod]
    public void GetDescription_FetchTheDescription_String()
    {
      //arrange
      string controlDesc = "Go to store";
      Item newItem = new Item("Go to store", "02-27-18");

      //act
      string result = newItem.GetDescription();

      //assert
      Assert.AreEqual(result, controlDesc);
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Item.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Save_SavesToDatabase_ItemList()
    {
      //arrange
      Item testItem = new Item("Mow the lawn", "02-27-18");

      //act
      testItem.Save();
      List<Item> result = Item.GetAll();
      List<Item> testList = new List<Item>{testItem};

      //assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
    {
      //Arrange, act
      Item firstItem = new Item("Mow the lawn", "02-27-18");
      Item secondItem = new Item("Mow the lawn", "02-27-18");

      //Assert
      Assert.AreEqual(firstItem, secondItem);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //arrange
      Item testItem = new Item("Mow the lawn", "02-27-18");

      //act
      testItem.Save();
      Item savedItem = Item.GetAll()[0];

      int result = savedItem.GetId();
      int testId = testItem.GetId();

      //assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsItemInDatabase_Item()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn", "02-27-18");
      testItem.Save();

      //act
      Item foundItem = Item.Find(testItem.GetId());

      //assert
      Assert.AreEqual(testItem, foundItem);
    }



    [TestMethod]
    public void Edit_UpdatesItemInDatabase_String()
    {
      //arrange
      string firstDescription = "Walk the Dog";
      Item testItem = new Item(firstDescription, "02-27-18", 1);
      testItem.Save();
      string secondDescription = "Mow the Lawn";

      //act
      testItem.Edit(secondDescription);
      string result = Item.Find(testItem.GetId()).GetDescription();

      //Assert
      Assert.AreEqual(secondDescription, result);
    }

    [TestMethod]
    public void Delete_DeleteItemInDatabase_Void()
    {
      //arrange
      Item testItem1 = new Item("Pet a cat", "02-27-18", 1);
      testItem1.Save();
      List<Item> originalList = Item.GetAll(); // should be 1 item
      Item testItem2 = new Item("Pet a dog", "02-27-18", 2);
      testItem2.Save();

      //act
      testItem2.Delete();
      List<Item> newList = Item.GetAll();

      //Assert
      CollectionAssert.AreEqual(originalList, newList);
    }

    [TestMethod]
    public void AddCategory_AddsCategoryToItem_CategoryList()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn", "02-27-18");
      testItem.Save();

      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      //Act
      testItem.AddCategory(testCategory);

      List<Category> result = testItem.GetCategories();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetCategories_ReturnsAllItemCategories_CategoryList()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn", "02-27-18");
      testItem.Save();

      Category testCategory1 = new Category("Home stuff");
      testCategory1.Save();

      Category testCategory2 = new Category("Work stuff");
      testCategory2.Save();

      //Act
      testItem.AddCategory(testCategory1);
      List<Category> result = testItem.GetCategories();
      List<Category> testList = new List<Category> {testCategory1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeletesItemAssociationsFromDatabase_ItemList()
    {
      //Arrange
      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      string testDescription = "Mow the lawn";
      Item testItem = new Item(testDescription, "02-27-18");
      testItem.Save();

      //Act
      testItem.AddCategory(testCategory);
      testItem.Delete();

      List<Item> resultCategoryItems = testCategory.GetItems();
      List<Item> testCategoryItems = new List<Item> {};

      //Assert
      CollectionAssert.AreEqual(testCategoryItems, resultCategoryItems);
    }

    [TestMethod]
    public void Date_ReturnsDate_True()
    {
      DateTime testDate = new DateTime(2018, 02, 27);
      var testDateFormatted = testDate.ToString("MM/dd/yy");
      var resultDate = "02/27/18";

      Assert.AreEqual(testDateFormatted, resultDate);
   }

  }
}
