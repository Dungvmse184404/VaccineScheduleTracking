using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Controllers;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Children;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using VaccineScheduleTracking.API_Test.Services.Children;
using Xunit;
using Moq;

public class ChildControllerTests
{
    private readonly VaccineScheduleDbContext _dbContext;
    private readonly ChildController _controller;
    private readonly IMapper _mapper;
    private readonly IChildService _childService;
    private readonly IAccountService _accountService;

    public ChildControllerTests()
    {
        // Khởi tạo DbContext In-Memory
        var options = new DbContextOptionsBuilder<VaccineScheduleDbContext>()
            .UseInMemoryDatabase(databaseName: "VaccineTestDB")
            .Options;

        _dbContext = new VaccineScheduleDbContext(options);
        _dbContext.Database.EnsureCreated();

        // Khởi tạo dịch vụ giả lập
        var accountServiceMock = new Mock<IAccountService>();
        var childServiceMock = new Mock<IChildService>();
        var mapperMock = new Mock<IMapper>();

        _accountService = accountServiceMock.Object;
        _childService = childServiceMock.Object;
        _mapper = mapperMock.Object;

        _controller = new ChildController(_accountService, _childService, _mapper);

        // Thiết lập user giả lập (đóng vai parent)
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "Parent")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetChildren_ReturnsOk_WithChildrenList()
    {
        // Arrange
        var parentId = 1;
        var children = new List<Child>
        {
            new Child { ChildID = 1, Firstname = "John", Lastname = "Doe", ParentID = parentId },
            new Child { ChildID = 2, Firstname = "Jane", Lastname = "Doe", ParentID = parentId }
        };

        _dbContext.Children.AddRange(children);
        _dbContext.SaveChanges();

        var childDtos = children.Select(c => new ChildDto
        {
            ChildID = c.ChildID,
            Firstname = c.Firstname,
            Lastname = c.Lastname
        }).ToList();

        var mockChildService = new Mock<IChildService>();
        mockChildService.Setup(service => service.GetParentChildren(parentId))
                        .ReturnsAsync(children);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<List<ChildDto>>(It.IsAny<List<Child>>()))
                  .Returns(childDtos);

        var controller = new ChildController(_accountService, mockChildService.Object, mockMapper.Object);

        // Act
        var result = await controller.GetChildren();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnChildren = Assert.IsType<List<ChildDto>>(okResult.Value);
        Assert.Equal(2, returnChildren.Count);
    }


    [Fact]
    public async Task CreateChildProfile_ReturnsOk_WhenChildIsAdded()
    {
        // Arrange
        var addChildDto = new AddChildDto
        {
            Firstname = "Alice",
            Lastname = "Brown",
            //BirthDate = DateTime.UtcNow
        };

        var newChild = new Child
        {
            ChildID = 3,
            Firstname = "Alice",
            Lastname = "Brown",
            ParentID = 1
        };

        var mockChildService = new Mock<IChildService>();
        mockChildService.Setup(service => service.AddChild(It.IsAny<Child>()))
                        .ReturnsAsync(newChild);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<Child>(addChildDto)).Returns(newChild);
        mockMapper.Setup(m => m.Map<ChildDto>(newChild))
                  .Returns(new ChildDto
                  {
                      ChildID = newChild.ChildID,
                      Firstname = newChild.Firstname,
                      Lastname = newChild.Lastname
                  });

        var controller = new ChildController(_accountService, mockChildService.Object, mockMapper.Object);

        // Act
        var result = await controller.CreateChildProfile(addChildDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnChild = Assert.IsType<Child>(okResult.Value);
        Assert.Equal("Alice", returnChild.Firstname);
    }

    [Fact]
    public async Task DeleteChildProfile_ReturnsOk_WhenChildIsDeleted()
    {
        // Arrange
        var child = new Child { ChildID = 1, Firstname = "John", Lastname = "Doe", ParentID = 1 };
        _dbContext.Children.Add(child);
        _dbContext.SaveChanges();

        var mockChildService = new Mock<IChildService>();
        mockChildService.Setup(service => service.DeleteChild(1))
                        .ReturnsAsync(child);

        var controller = new ChildController(_accountService, mockChildService.Object, _mapper);

        // Act
        var result = await controller.DeleteChildProfile(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var message = Assert.IsType<string>(okResult.Value);
        Assert.Contains("John", message);
    }
}
