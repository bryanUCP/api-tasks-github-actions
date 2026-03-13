using TaskApi.Controllers;
using TaskApi.Models;
using TaskApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace TaskApi.Tests.Controllers;
 
public class TaskControllerTests
{
 
  private readonly TasksController _controller;
  private readonly Mock<ITaskRepository> _mockRepo;
 
  public TaskControllerTests()
  {
    _mockRepo = new Mock<ITaskRepository>();
    _controller = new TasksController(_mockRepo.Object);
  }
 
  //getAll
  [Fact]
  public void GetAll_HayTareas_RetornaOkConListaDeTareas()
  {
    _mockRepo.Setup(repo => repo.GetAll()).Returns(new List<TaskItem>
    {
      new TaskItem { Id = 1, Title = "Tarea 1", Description = "Descripción de la tarea 1", IsCompleted = false },
      new TaskItem { Id = 2, Title = "Tarea 2", Description = "Descripción de la tarea 2", IsCompleted = true }
    });
 
    _controller.GetAll().Should().BeOfType<OkObjectResult>()
      .Which.Value.Should().BeAssignableTo<IEnumerable<TaskItem>>()
      .Which.Should().HaveCount(2);
 
   
  }
 
  //getById
  [Fact]
  public void GetById_TareaExiste_RetornaOkConTarea()
  {
    _mockRepo.Setup(repo => repo.GetById(1)).Returns(new TaskItem { Id = 1, Title = "Tarea 1", Description = "Descripción de la tarea 1", IsCompleted = false });
 
    _controller.GetById(1).Should().BeOfType<OkObjectResult>()
      .Which.Value.Should().BeAssignableTo<TaskItem>()
      .Which.Id.Should().Be(1);
 
  }
 
  // no existe el id
  [Fact]
  public void GetById_TareaNoExiste_RetornaNotFound()
  {
    _mockRepo.Setup(repo => repo.GetById(999)).Returns((TaskItem?)null);
 
    _controller.GetById(999).Should().BeOfType<NotFoundResult>();
  }
}